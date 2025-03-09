using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

using System.Collections.Generic;


using System.Linq;

namespace Stijn.RenderFeature
{
    public class RenderObjectsToTexture : ScriptableRendererFeature
    {
        [System.Serializable]
        public class Settings
        {
            [HideInInspector]
            public string Name = "RenderObjectsToTexture";

            [Header("Default Settings")]
            public RenderObjects.RenderObjectsSettings RenderObjsSettings = new RenderObjects.RenderObjectsSettings();

            


            [Header("Custom Pass")]
            public string CustomPassReference = "_CustomGrabPass";
            [Min(1)]
            public int DownSamplingAmount = 1;

            [Header("Extra")]
            public Color ClearColor = Color.white;

            public Material MaterialOccluder;
            public LayerMask LayerMaskOccluder;
        }

        class CustomRenderPass : ScriptableRenderPass
        {
            private Settings _settings;

            private RTHandle _tempId;

            // UNITY Render objects implemention
            ProfilingSampler _profilingSampler;
            RenderStateBlock _renderStateBlock;
            FilteringSettings _filteringSettings;
            List<ShaderTagId> m_ShaderTagIdList = new List<ShaderTagId>();

            FilteringSettings _occluderFilteringSettings;

            public CustomRenderPass(Settings settings)
            {
                _settings = settings;

                #region Unity Render Objects Setup
                base.profilingSampler = new ProfilingSampler("RenderObjectsToRenderTexture");
                //this.profilingSampler = new ProfilingSampler(nameof(RenderObjectsToRenderTexture));
                _profilingSampler = new ProfilingSampler(_settings.Name);

                RenderQueueRange renderQueueRange = (_settings.RenderObjsSettings.filterSettings.RenderQueueType == RenderQueueType.Transparent)
                    ? RenderQueueRange.transparent
                    : RenderQueueRange.opaque;
                _filteringSettings = new FilteringSettings(renderQueueRange, _settings.RenderObjsSettings.filterSettings.LayerMask);

                _occluderFilteringSettings = new FilteringSettings(renderQueueRange, _settings.LayerMaskOccluder);

                if (_settings.RenderObjsSettings.filterSettings.PassNames != null && _settings.RenderObjsSettings.filterSettings.PassNames.Length > 0)
                {
                    foreach (var passName in _settings.RenderObjsSettings.filterSettings.PassNames)
                    {
                        
                                m_ShaderTagIdList.Add(new ShaderTagId(passName));
                       
                    }
                }
                else
                {
                    m_ShaderTagIdList.Add(new ShaderTagId("SRPDefaultUnlit"));
                    m_ShaderTagIdList.Add(new ShaderTagId("UniversalForward"));
                    m_ShaderTagIdList.Add(new ShaderTagId("UniversalForwardOnly"));
                    m_ShaderTagIdList.Add(new ShaderTagId("LightweightForward"));
                }

                _renderStateBlock = new RenderStateBlock(RenderStateMask.Nothing);

                this.renderPassEvent = _settings.RenderObjsSettings.Event;
                #endregion

                // Setup RenderTexture
                _tempId = RTHandles.Alloc(_settings.CustomPassReference, name: _settings.CustomPassReference);
            }

            public void SetDepthState(bool writeEnabled, CompareFunction function = CompareFunction.Less)
            {
                _renderStateBlock.mask |= RenderStateMask.Depth;
                _renderStateBlock.depthState = new DepthState(writeEnabled, function);
            }

            public void SetStencilState(int reference, CompareFunction compareFunction, StencilOp passOp, StencilOp failOp, StencilOp zFailOp)
            {
                StencilState stencilState = StencilState.defaultValue;
                stencilState.enabled = true;
                stencilState.SetCompareFunction(compareFunction);
                stencilState.SetPassOperation(passOp);
                stencilState.SetFailOperation(failOp);
                stencilState.SetZFailOperation(zFailOp);

                _renderStateBlock.mask |= RenderStateMask.Stencil;
                _renderStateBlock.stencilReference = reference;
                _renderStateBlock.stencilState = stencilState;
            }

            public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
            {
                if (string.IsNullOrEmpty(_settings.CustomPassReference)) { Debug.Log("ISSUE, CUSTOM PASS REF IS NULL"); return; }

                cmd.GetTemporaryRT(Shader.PropertyToID("_TempRenderObjectsToRenderTexture"), renderingData.cameraData.cameraTargetDescriptor);

                RenderTextureDescriptor passTargetDescriptor = renderingData.cameraData.cameraTargetDescriptor;
                passTargetDescriptor.width /= _settings.DownSamplingAmount;
                passTargetDescriptor.height /= _settings.DownSamplingAmount;

                cmd.GetTemporaryRT(Shader.PropertyToID(_settings.CustomPassReference), passTargetDescriptor, FilterMode.Bilinear);
            }

            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
            {
                var cmd = CommandBufferPool.Get(_settings.Name);

                cmd.SetRenderTarget(_tempId);
                cmd.ClearRenderTarget(true, true, _settings.ClearColor);

                SortingCriteria sortingCriteria = (_filteringSettings.renderQueueRange == RenderQueueRange.transparent)
                    ? SortingCriteria.CommonTransparent
                    : renderingData.cameraData.defaultOpaqueSortFlags;

                DrawingSettings drawingSettings = CreateDrawingSettings(m_ShaderTagIdList, ref renderingData, sortingCriteria);
                drawingSettings.overrideMaterial = _settings.RenderObjsSettings.overrideMaterial;
                drawingSettings.overrideMaterialPassIndex = _settings.RenderObjsSettings.overrideMaterialPassIndex;

                DrawingSettings occluderDrawingSettings = CreateDrawingSettings(m_ShaderTagIdList, ref renderingData, sortingCriteria);
                occluderDrawingSettings.overrideMaterial = _settings.MaterialOccluder;
                occluderDrawingSettings.overrideMaterialPassIndex = 0;

                ref CameraData cameraData = ref renderingData.cameraData;

                using (new ProfilingScope(cmd, _profilingSampler))
                {
                    context.ExecuteCommandBuffer(cmd);
                    cmd.Clear();

                    if (_settings.RenderObjsSettings.cameraSettings.overrideCamera)
                    {
                        Camera camera = cameraData.camera;

                        Matrix4x4 projectionMatrix = Matrix4x4.Perspective(_settings.RenderObjsSettings.cameraSettings.cameraFieldOfView, camera.aspect,
                            camera.nearClipPlane, camera.farClipPlane);

                        Matrix4x4 viewMatrix = camera.worldToCameraMatrix;
                        Vector4 cameraTranslation = viewMatrix.GetColumn(3);
                        viewMatrix.SetColumn(3, cameraTranslation + _settings.RenderObjsSettings.cameraSettings.offset);

                        cmd.SetViewProjectionMatrices(viewMatrix, projectionMatrix);
                        context.ExecuteCommandBuffer(cmd);
                    }

                    context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref _filteringSettings,
                        ref _renderStateBlock);
                    if (_settings.MaterialOccluder)
                    {
                        context.DrawRenderers(renderingData.cullResults, ref occluderDrawingSettings, ref _occluderFilteringSettings,
                            ref _renderStateBlock);
                    }

                    if (_settings.RenderObjsSettings.cameraSettings.overrideCamera && _settings.RenderObjsSettings.cameraSettings.restoreCamera)
                    {
                        cmd.Clear();
                        cmd.SetViewProjectionMatrices(cameraData.GetViewMatrix(), cameraData.GetProjectionMatrix());
                    }
                }

                context.ExecuteCommandBuffer(cmd);
                cmd.Clear();

                CommandBufferPool.Release(cmd);
            }

            public override void OnCameraCleanup(CommandBuffer cmd)
            {
                cmd.ReleaseTemporaryRT(Shader.PropertyToID(_settings.CustomPassReference));
            }
        }

        CustomRenderPass _customRenderPass;

        [SerializeField]
        private Settings _settings;

        public override void Create()
        {
            if (_settings == null) _settings = new Settings();

            _settings.Name = this.name;
            _customRenderPass = new CustomRenderPass(_settings);
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            renderer.EnqueuePass(_customRenderPass);
        }
    }
}
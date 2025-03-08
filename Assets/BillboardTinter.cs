using UnityEngine;


[ExecuteInEditMode]
public class BillboardTinter : MonoBehaviour
{
    private MeshRenderer m_Renderer;
    public Color Color = Color.white;
    private void Start()
    {
        m_Renderer = GetComponent<MeshRenderer>();
    }

    private void FixedUpdate()
    {
        m_Renderer.material.SetColor("_tint", Color);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class JuiceManager : MonoBehaviour
{
    public static JuiceManager Instance;

  


    private float shakeDuration = 0f;
    private float maxShakeDuration = 0f;


    [SerializeField] private float _shakeMagnitude = 0.7f;
    [SerializeField] private float _dampingSpeed = 1.0f;
    [SerializeField] private GameObject _camera;
    private Vector3 initialCameraPosition;
    [SerializeField] private AnimationCurve _cameraYShake;

    [SerializeField] private Camera _cameraFOV;
    [SerializeField] private Vector2 _cameraFOVTresh = new Vector2(60,50);




    void Start()
    {
        if (JuiceManager.Instance == null)
        {
            JuiceManager.Instance = this;
        }
        else
        {
            Destroy(this);
        }

        initialCameraPosition = _camera.transform.localPosition;

    }
    void Update()
    {
        if (shakeDuration > 0)
        {
            _camera.transform.localPosition = initialCameraPosition + Vector3.up*_shakeMagnitude*_cameraYShake.Evaluate(1-(shakeDuration/maxShakeDuration));

            shakeDuration -= Time.deltaTime ;
        }
        else
        {
            shakeDuration = 0f;
            _camera.transform.localPosition = initialCameraPosition;
        }
    }

    public void SetCameraFOV(float fov)
    {

        _cameraFOV.fieldOfView = Mathf.Lerp(_cameraFOV.fieldOfView, Mathf.Lerp(_cameraFOVTresh.x, _cameraFOVTresh.y, fov),0.1f) ;


    }

    public void TriggerShake(float duration, float magnitude)
    {
        shakeDuration = duration;
        maxShakeDuration = duration;
        _shakeMagnitude = magnitude;
    }
}
// blabla
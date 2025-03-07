using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceDownAnimation : MonoBehaviour
{
    private Vector3 _baseYScale;
    [SerializeField] private Vector3 _scale;
    void Start()
    {
        _baseYScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {

        gameObject.transform.localScale = 
            new Vector3(
                _baseYScale.x * (1 + (Mathf.Sin(Time.timeSinceLevelLoad * 10) * _scale.x)),
                _baseYScale.y * (1 - (Mathf.Sin(Time.timeSinceLevelLoad * 10 )*_scale.y)),
                _baseYScale.y * (1 + (Mathf.Sin(Time.timeSinceLevelLoad * 10) * _scale.z)));
    }
}

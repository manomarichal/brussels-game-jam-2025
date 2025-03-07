using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField] private GameObject _followedGameObject;
    private Vector3 _differenceVector;
    [SerializeField] private bool _completeFollow = false;
    void Start()
    {
        _differenceVector = transform.position - _followedGameObject.transform.position;
       
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _followedGameObject.transform.position + _differenceVector;
        if (_completeFollow)
        {
            transform.localRotation = _followedGameObject.transform.localRotation;
        }
    }
}

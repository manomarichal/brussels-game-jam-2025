using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class InverseKinematicsPoints : MonoBehaviour
{
    [SerializeField] private GameObject[] _ikEndPoints;
    [SerializeField] private GameObject[] _fakeTargets;


    private List<GameObject> _targets;

    [SerializeField] private GameObject _empty;
    private int _timeTracker;
    private int _pointTracker;
    [SerializeField,Range(0.1f,2f)] private float _stepDistance;
    [SerializeField, Range(0.01f, 0.99f)] private float _hillBillyLerp;

    [SerializeField] private float _footMovementSpeed=2;

    [SerializeField] private float _movesPerFrame;

    [SerializeField] private GameObject[] _idealEndPoints;
    [SerializeField] private float _maxDistance = 3;
    [SerializeField] private float _floorCheckDistance = 0.9f;

    [SerializeField] private LayerMask _layerMask;



    private bool _isAlive = true;

    void Start()
    {
        _targets = new List<GameObject>();
        _timeTracker = 0;
        foreach(GameObject gameObject in _ikEndPoints)
        {
           
            GameObject newTarget = Instantiate(_empty,gameObject.transform.position,Quaternion.identity);
            newTarget.name = "fake " + gameObject.name ;
            newTarget.transform.parent = this.transform ;
            _targets.Add(newTarget);
        }

    }

    void FixedUpdate()
    {
        _timeTracker += 1;

        UpdateAllPoints();
        LerpFakes();
        CheckIdealPositions();

        //if(_timeTracker >= (60 / (_movesPerFrame*_ikEndPoints.Count())))
        //{
        //    UpdatePoint(_pointTracker);
        //    _timeTracker = 0;
        //    _pointTracker++;
        //    if (_pointTracker >= (_ikEndPoints.Count()))
        //    {
        //        _pointTracker = 0;
        //    }
        //}


    }

    private void LerpFakes()
    {
        for (int idx = 0; idx < _fakeTargets.Length; idx++)
        {
            GameObject fakeTarget = _fakeTargets[idx];
            GameObject target = _targets[idx];

            //target.transform.position = Vector3.Lerp(target.transform.position,fakeTarget.transform.position, _hillBillyLerp);

            target.transform.position = Vector3.MoveTowards(target.transform.position, fakeTarget.transform.position, _footMovementSpeed * Time.deltaTime);
            if ((target.transform.position - fakeTarget.transform.position).magnitude > 3)
            {
                target.transform.position = Vector3.Lerp(target.transform.position,fakeTarget.transform.position, 0.9f);


            }

        }
    }

    private void CheckIdealPositions()
    {
        for (int idx = 0; idx < _fakeTargets.Length; idx++)
        {
            GameObject fakeTarget = _fakeTargets[idx];
            GameObject idealEndPoint = _idealEndPoints[idx];

            if ((fakeTarget.transform.position - idealEndPoint.transform.position).magnitude > _maxDistance)
            {
                RaycastHit hit;
                if (Physics.Raycast(idealEndPoint.transform.position+Vector3.up*3, Vector3.down, out hit, 3+ _floorCheckDistance, _layerMask))
                {
                    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                    fakeTarget.transform.position = hit.point;
                    //Debug.Log("Did Hit");
                   

                }


            }

        }
    }

    private void UpdateAllPoints()
    {
        for (int idx = 0; idx < _ikEndPoints.Length; idx++)
        {
            UpdatePoint(idx);
        }
    }

    private void UpdatePoint(int idx)
    {
        GameObject ikEndPoint = _ikEndPoints[idx];
        GameObject target = _targets[idx];

        

        ikEndPoint.transform.position = target.transform.position;
    }

    internal void Death()
    {
        _isAlive = false;
    }
}


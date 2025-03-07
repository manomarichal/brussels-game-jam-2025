using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTorsoRotationScript : MonoBehaviour
{

    [SerializeField] private ConfigurableJoint _thisConfigurableJoint;

    public void Start()
    {
        _thisConfigurableJoint = GetComponent<ConfigurableJoint>();
    }
   

    public void TargetRotationFromEuler(Vector3 target)
    {
        Quaternion RotationQuat = Quaternion.Euler(new Vector3(0,target.x,0));
        _thisConfigurableJoint.targetRotation = RotationQuat;
    }
}

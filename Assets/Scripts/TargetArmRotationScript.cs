using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TargetArmRotationScript : MonoBehaviour
{

    private ConfigurableJoint _thisConfigurableJoint;
    private Quaternion _armRestingPosition;
    private Quaternion _armGrabbingPosition;
    [SerializeField] private bool _isRightArm = true;

    public void Start()
    {
        _thisConfigurableJoint = GetComponent<ConfigurableJoint>();
        _armRestingPosition = Quaternion.Euler(new Vector3(0, 0, 0));
        _armGrabbingPosition= Quaternion.Euler(new Vector3(90, 0, 0));
    }


    public void Update()
    {
        bool NeedsToBeUp = (_isRightArm && Input.GetMouseButton(1) || !_isRightArm && Input.GetMouseButton(0));
        if (NeedsToBeUp)
        {
            _thisConfigurableJoint.targetRotation = _armGrabbingPosition;
        }
        else
        {
            _thisConfigurableJoint.targetRotation = _armRestingPosition;
        }
    }
}

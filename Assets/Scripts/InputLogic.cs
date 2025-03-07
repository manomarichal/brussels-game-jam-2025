using System;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController),typeof(Health))]
public class InputLogic : MonoBehaviour
{
    #region Variables
    private InputSystem_Actions _playerControls;


    private InputAction _move, _look, _jump, _leftClick, _rightClick;



    [Header("Movement Settings"), SerializeField]
    private CharacterController _characterController;
    [SerializeField] private float _characterSpeed = 5f;
    [SerializeField] private float _characterJumpHeight = 5f;


    [Header("Camera Settings")]

    [SerializeField] private Camera _camera;

    private float _angle = 0;



    [SerializeField] private float _sensitivity = 1.2f;
    [SerializeField] private Vector2 _visionClamping = new Vector2(-60.0f,30.0f);

    [Header("Other")]

    [SerializeField] private Health _health;
    [SerializeField] private IEquipment _equipment;

    private float _throwTimer = 0;
    [SerializeField] private Vector2 _throwTiming = new Vector2(1f,5f);
    [SerializeField] private float _throwStrength = 10f;


    #endregion

    #region Startup/Shutdown

    private void Awake()
    {
        _playerControls = new InputSystem_Actions();
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void OnEnable()
    {

        _move = _playerControls.Player.Move;
        _move.Enable();

        _look = _playerControls.Player.Look;
        _look.Enable();


        _jump = _playerControls.Player.Jump;
        _jump.Enable();
        _jump.performed += Jump;

        _leftClick = _playerControls.Player.Attack;
        _leftClick.Enable();
        _leftClick.performed += LeftClick;


    }

    private void OnDisable()
    {
        _move.Disable();

        _look.Disable();

        _jump.Disable();
        _jump.performed -= Jump;

        _leftClick = _playerControls.Player.Attack;
        _leftClick.Disable();


    }
    #endregion

    #region generic player controls


    private void FixedUpdate()
    {
        Vector2 Movement = _move.ReadValue<Vector2>();
        Vector2 Look = _look.ReadValue<Vector2>();


        _characterController.SimpleMove(_characterSpeed * ((transform.forward*Movement.y + transform.right*Movement.x).normalized));



        transform.Rotate(Vector3.up,Look.x * _sensitivity);


        _angle -= Look.y* _sensitivity;


        _angle = Mathf.Clamp(_angle, _visionClamping.x, _visionClamping.y);
        _camera.transform.localRotation = Quaternion.Euler(_angle, 0.0f, 0.0f);



        if (_leftClick.IsPressed() && _equipment!= null)
        {
            _throwTimer += Time.deltaTime;
            if (_throwTimer > _throwTiming.x)
            {
                // put UI logic for throwing here
            }
        }


    }


    private void Jump(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Equipment related

    private void LeftClick(InputAction.CallbackContext context)
    {
        if (_equipment == null)
            return;

        if (_throwTimer > _throwTiming.x)
        {
            ThrowEquippable();
        }
        else
        {
            UseEquippable();
        }

        _throwTimer = 0;


    }

    private void UseEquippable()
    {
        _equipment.UseItem();
    }

    private void ThrowEquippable()
    {
        _throwTimer = Mathf.Clamp(_throwTimer, _throwTiming.x, _throwTiming.y);


        _equipment.Throw(_camera.transform.position, _camera.transform.forward * _throwTimer * _throwStrength);
    }

    private void RightClick(InputAction.CallbackContext context)
    {
        if (_equipment == null)
            PickUp();
        else
        {
            _equipment.DropItem();
            _equipment = null;
        }
    }

    private void PickUp()
    {
        throw new NotImplementedException();
    }


    #endregion
}

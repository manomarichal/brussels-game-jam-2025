using System;
using System.Collections.Generic;
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


    [SerializeField] private float _gravity = -9.81f;
    private float _verticalMovement = 0;

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

    [SerializeField] private ObjectContainer _pickupRange;


    #endregion

    #region Startup/Shutdown

    private void Awake()
    {
        _playerControls = new InputSystem_Actions();
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        _health.OnHealthChanged.AddListener(HealthChange);
    }

    private void HealthChange(int newHealth,int dmgValue)
    {
        if(dmgValue > 0)
        {
            JuiceManager.Instance.TriggerShake(1f,dmgValue/20f);
        }


        if(newHealth == 0)
        {
            this.enabled = false;
        }
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
        _leftClick.canceled += LeftClick;

        _rightClick = _playerControls.Player.Rightclick;
        _rightClick.Enable();
        _rightClick.performed += RightClick;


    }

    private void OnDisable()
    {
        _move.Disable();

        _look.Disable();

        _jump.Disable();
        _jump.performed -= Jump;

        _leftClick.Disable();
        _leftClick.performed -= LeftClick;

        _rightClick.Disable();
        _rightClick.performed -= RightClick;


    }
    #endregion

    #region generic player controls


    private void FixedUpdate()
    {


        if (_characterController.isGrounded && _verticalMovement<0)
            _verticalMovement = 0f;
        else { _verticalMovement += _gravity * Time.deltaTime; }

        Vector2 Movement = _move.ReadValue<Vector2>();
        Vector2 Look = _look.ReadValue<Vector2>();


        _characterController.Move((_characterSpeed * ((transform.forward*Movement.y + transform.right*Movement.x).normalized) + _verticalMovement*Vector3.up)*Time.deltaTime);



        transform.Rotate(Vector3.up,Look.x * _sensitivity);


        _angle -= Look.y* _sensitivity;


        _angle = Mathf.Clamp(_angle, _visionClamping.x, _visionClamping.y);
        _camera.transform.localRotation = Quaternion.Euler(_angle, 0.0f, 0.0f);



        if (_leftClick.IsPressed() && _equipment!= null)
        {
            _throwTimer += Time.deltaTime;
           
        }


    }


    private void Jump(InputAction.CallbackContext context)
    {
        if(_characterController.isGrounded && _equipment == null)
        {
            _verticalMovement += _characterJumpHeight;
            Debug.Log("Jump");

        }
    }

    #endregion

    #region Equipment related

    private void LeftClick(InputAction.CallbackContext context)
    {
        Debug.Log("Left Click");


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

    public float GetThrowProgression()
    {
        return (_throwTimer- _throwTiming.x)/(_throwTiming.y-_throwTiming.x);
    }

    private void ThrowEquippable()
    {

        _throwTimer = Mathf.Clamp(_throwTimer, _throwTiming.x, _throwTiming.y);


        _equipment.Throw(_camera.transform.position, _camera.transform.forward * _throwTimer * _throwStrength);
        _equipment = null;

    }

    private void RightClick(InputAction.CallbackContext context)
    {
        Debug.Log("Right Click");


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
        Debug.Log("Attempting pickup");
        IEquipment savedEquipment = null;
        foreach (GameObject possiblePickup in _pickupRange.InTheZone)
        {
            IEquipment equipment = possiblePickup.GetComponent<IEquipment>();
            if(equipment != null && savedEquipment == null)
            {
                savedEquipment = equipment;
            }
        }

        if (savedEquipment != null)
        {
            _equipment = savedEquipment;
            _equipment.EquipItem(_health);
            Debug.Log("Pickup succesfull!");

        }
    }

    

    #endregion
}

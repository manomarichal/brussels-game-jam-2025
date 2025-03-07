using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputLogic : MonoBehaviour
{
    [SerializeField] private InputSystem_Actions _playerControls;
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private float _characterSpeed = 5f;
    [SerializeField] private float _characterJumpHeight = 5f;

    [SerializeField] private Camera _camera;


    [SerializeField] private float _sensitivity = 1.2f;
    [SerializeField] private Vector2 _visionClamping = new Vector2(-60.0f,30.0f);

    private float _angle = 0;

    private InputAction _move;
    private InputAction _look;
    private InputAction _jump;

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

    }

    private void OnDisable()
    {
        _move.Disable();

        _look.Disable();

        _jump.Disable();
        _jump.performed -= Jump;

    }


    private void FixedUpdate()
    {
        Vector2 Movement = _move.ReadValue<Vector2>();
        Vector2 Look = _look.ReadValue<Vector2>();


        _characterController.Move(_characterSpeed * Time.deltaTime * ((transform.forward*Movement.y + transform.right*Movement.x).normalized));



        transform.Rotate(Vector3.up,Look.x * _sensitivity);


        _angle -= Look.y* _sensitivity;


        _angle = Mathf.Clamp(_angle, _visionClamping.x, _visionClamping.y);
        _camera.transform.localRotation = Quaternion.Euler(_angle, 0.0f, 0.0f);

    }


    private void Jump(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }
}

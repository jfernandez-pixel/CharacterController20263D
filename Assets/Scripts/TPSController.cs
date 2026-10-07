using UnityEngine;
using UnityEngine.InputSystem;

public class TPSController : MonoBehaviour
{
    //Variables movimiento
    private CharacterController _characterController;
    private InputAction _moveAction;
    private InputAction _aimAction;
    private Vector2 _moveInput;
    private InputAction _jumpAction;
    private InputAction _lookAction;
    private Vector2 _lookInput;
    [SerializeField]private float _movementSpeed = 10;
    [SerializeField]private float _jumpHeight = 2;

    //Variables para la gravedad
    private float _gravitiy;
    [SerializeField]private Vector3 _playerGravity;
    [SerializeField]private Transform _sensorTransforms;
    [SerializeField]private float _sensorRadius;
    [SerializeField]private LayerMask _groundLayer;

    //Variable camara
    private Transform _cameraTransform;
    [SerializeField]private Transform _lookAtCamera;
    private float _xRotation;

    //SVariable snsibilidad raton
    [SerializeField]private float _cameraSensitivity = 10;

    
    void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _moveAction = InputSystem.actions["Move"];
        _jumpAction = InputSystem.actions["Jump"];
        _aimAction = InputSystem.actions["Aim"];
        _lookAction = InputSystem.actions["Look"];

        _cameraTransform = Camera.main.transform;
    }
   
   
   
    void Start()
    {
        _gravitiy = Physics.gravity.y;
    }

    void Update()
    {
        _moveInput = _moveAction.ReadValue<Vector2>();
        _lookInput = _lookAction.ReadValue<Vector2>();

        Gravitiy();

        if(_jumpAction.WasPressedThisFrame() && IsGrounded()) // Pone la accion de saltar 
        {
            Jump();
        }

        TPSMovement();
    
    }
    
    void TPSMovement()
    {
        Vector3 direction = new Vector3(_moveInput.x, 0, _moveInput.y);

        float MouseX = _lookInput.x * _cameraSensitivity * Time.deltaTime;
        float MouseY = _lookInput.y + _cameraSensitivity * Time.deltaTime;

        _xRotation -= MouseY;
        _xRotation = Mathf.Clamp(_xRotation, -89, 89);

        transform.Rotate(Vector3.up, MouseX);
        _lookAtCamera.localRotation = Quaternion.Euler(_xRotation, 0, 0);

        if(direction != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _cameraTransform.eulerAngles.y;
            Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

            _characterController.Move(moveDirection * _movementSpeed * Time.deltaTime);
        }
    }

    void Gravitiy() //Funcion para que detecte la gravedad si el personaje no esta tocando el suelo y no se sume
    {
        if(!IsGrounded())
        {
            _playerGravity.y += _gravitiy * Time.deltaTime;
        }
        else if (IsGrounded() && _playerGravity.y < 0)
        {
            _playerGravity.y = _gravitiy;
        }
            _characterController.Move(_playerGravity * Time.deltaTime);
    }

    void Jump()//Funcion de salto
    {
        _playerGravity.y = Mathf.Sqrt(_jumpHeight * -2 * _gravitiy);

    }

    bool IsGrounded() // Funcion para detectar el suelo
    {
        return Physics.CheckSphere(_sensorTransforms.position, _sensorRadius, _groundLayer);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_sensorTransforms.position, _sensorRadius);
    }
}

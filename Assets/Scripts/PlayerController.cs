using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private CharacterController _characterController;
    private InputAction _moveAction;
    private Vector2 _moveInput;
    [SerializeField] private float _movementSpeed = 10;
    private float _turnSmoothVelocity;
    [SerializeField] private float _smoothTime = 1;
    private float _gravitiy;
    [SerializeField]private Vector3 _playerGravity;
    [SerializeField]private Transform _sensorTransforms;
    [SerializeField]private float _sensorRadius;
    [SerializeField]private LayerMask _groundLayer;

    void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _moveAction = InputSystem.actions["Move"];
    }

    void Start()
    {
        _gravitiy = Physics.gravity.y;
    }

    
    void Update()
    {
       _moveInput = _moveAction.ReadValue<Vector2>();
        
        Gravitiy();

        Movement();

    }

    void Movement()
    {
        Vector3 moveDirection = new Vector3(_moveInput.x, 0, _moveInput.y); //Ejecuta el codigo cuando pulsas alguna tecla de movimiento

        if(moveDirection != Vector3.zero)
        {
        float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg; //Devuelve en angulos nomrales para que gire la camara
        float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _smoothTime); // suaviza el movimiento de la camara

        transform.rotation = Quaternion.Euler(0, targetAngle, 0);

        _characterController.Move(moveDirection * _movementSpeed * Time.deltaTime);
        }
    }

    void Gravitiy()
    {
        if(!_characterController.isGrounded)
        {
        _playerGravity.y += _gravitiy + Time.deltaTime;
        }
        _characterController.Move(_playerGravity * Time.deltaTime);
    }

    void isGrounded()
    {

    }
}

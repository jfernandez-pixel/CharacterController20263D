using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    //Variables movimiento
    private CharacterController _characterController;
    private InputAction _moveAction;
    private InputAction _aimAction;
    private Vector2 _moveInput;
    private InputAction _jumpAction;
    [SerializeField]private float _movementSpeed = 10;
    [SerializeField]private float _jumpHeight = 2;


    //Variables grio camara
    private float _turnSmoothVelocity;
    [SerializeField] private float _smoothTime = 1;

    //Variables para la gravedad
    private float _gravitiy;
    [SerializeField]private Vector3 _playerGravity;
    [SerializeField]private Transform _sensorTransforms;
    [SerializeField]private float _sensorRadius;
    [SerializeField]private LayerMask _groundLayer;

    //Variable camara 3ra persona
    private Transform _cameraTransform;


    void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _moveAction = InputSystem.actions["Move"];
        _jumpAction = InputSystem.actions["Jump"];
        _aimAction = InputSystem.actions["Aim"];

        _cameraTransform = Camera.main.transform;
    }

    void Start()
    {
        _gravitiy = Physics.gravity.y;
    }

    
    void Update()
    {
       _moveInput = _moveAction.ReadValue<Vector2>();
        
        Gravitiy();

        if(_jumpAction.WasPressedThisFrame() && IsGrounded()) // Pone la accion de saltar 
        {
            Jump();
        }
        
        if (_aimAction.IsPressed()) //Movimiento de apuntado tipo shooter Fortnite
        {
            AimMovement();
        }
        else
        {
            TPMovement();
        }
        //AimMovement();//Llama a la funcion de movimiento

    }

    //Diferentes funciones de movimiento
    void TopDownMovement() // Funcion de movimiento topdown como hades, diablo
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

    void TPMovement()//Funcion movimiento en 3ra persona 
    {
        Vector3 direction = new Vector3(_moveInput.x, 0, _moveInput.y); //Ejecuta el codigo cuando pulsas alguna tecla de movimiento

        if(direction != Vector3.zero)
        {
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _cameraTransform.eulerAngles.y; 
        float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _smoothTime); 

        transform.rotation = Quaternion.Euler(0, targetAngle, 0);

        Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

        _characterController.Move(moveDirection * _movementSpeed * Time.deltaTime);
        } 
    }

    void AimMovement() //Movimiento de shooter 3ra persona tipo Fortnite
    {
        Vector3 direction = new Vector3(_moveInput.x, 0, _moveInput.y); //Ejecuta el codigo cuando pulsas alguna tecla de movimiento

        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _cameraTransform.eulerAngles.y; 
        float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _cameraTransform.eulerAngles.y, ref _turnSmoothVelocity, _smoothTime); 

        transform.rotation = Quaternion.Euler(0, targetAngle, 0);

        if(direction != Vector3.zero)
        {
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

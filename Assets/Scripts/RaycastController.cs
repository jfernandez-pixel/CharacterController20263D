using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

public class RaycastController : MonoBehaviour
{
    //Variables de movimiento
    //private CharacterController _characterController;
    private NavMeshAgent _agent;
    [SerializeField]private float _movementSpeed = 10; 
    private InputAction _clickAction;
    private InputAction _moveAction;
    private Vector2 _moveInput;
    private InputAction _lookInput;
    private Vector2 _mousePosition;


    void Awake()
    {
        _clickAction = InputSystem.actions["Attack"];
        _moveAction = InputSystem.actions["Move"];
        _lookInput = InputSystem.actions["MousePosition"];
        //_characterController = GetComponent<CharacterController>();
        _agent = GetComponent<NavMeshAgent>();
    }
    
    void Start()
    {
        
    }

        void Update()
    {
        _moveInput = _moveAction.ReadValue<Vector2>();

        //TopDownMovement();
       //Debug.DrawRay(transform.position, transform.forward * 10, Color.red);
        if(_clickAction.WasPressedThisFrame())
        {
            //ShotRaycast();
            MoveToClickPoint();
        }
    }

    void ShotRaycast()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, 10))
        {
            Debug.Log(hit.transform.name);
            Debug.Log(hit.transform.gameObject.layer);
            Debug.Log(hit.transform.tag);
            Destroy(hit.transform.gameObject);
        }
    }

    /*void TopDownMovement() // Funcion de movimiento topdown como hades, diablo
    {
        Vector3 moveDirection = new Vector3(_moveInput.x, 0, _moveInput.y);
        _mousePosition = _lookInput.ReadValue<Vector2>();

        Ray ray = Camera.main.ScreenPointToRay(_mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Vector3 playerForward = hit.point - transform.position;
            playerForward.y = 0;
            transform.forward = playerForward;
        }

        if(moveDirection != Vector3.zero)
        {
        _characterController.Move(moveDirection * _movementSpeed * Time.deltaTime);
        }
    }*/

    void MoveToClickPoint()
    {
        _mousePosition = _lookInput.ReadValue<Vector2>();
        Ray ray = Camera.main.ScreenPointToRay(_mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            _agent.SetDestination(hit.point);
        }
    }
}

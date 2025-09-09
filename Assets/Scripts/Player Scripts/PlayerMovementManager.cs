using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerMovementManager : MonoBehaviour
{
    private void Awake()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        _inputTranslator.OnLocomotionEvent += HandleLocomotion;
        _inputTranslator.OnJumpEvent += HandleJump;
    }
    private void OnDestroy()
    {
        _inputTranslator.OnLocomotionEvent -= HandleLocomotion;
        _inputTranslator.OnJumpEvent -= HandleJump;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }
    private void LateUpdate()
    {
        if(_playerLocomotion != Vector2.zero) RotatePlayer();
    }

    public void HandleLocomotion(Vector2 locomotion)
    {
        _playerLocomotion = locomotion;
    }
    public void HandleJump()
    {
        _playerRigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }

    private void MovePlayer()
    {
        Vector3 forwardVector = _cameraTransform.forward.normalized;
        Vector3 rightVector = _cameraTransform.right.normalized;
        Vector3 targetVel = (_playerLocomotion.y * _forwardSpeedMultiplier * _playerSpeed * forwardVector)
                              + (_playerLocomotion.x * _strafeSpeedMultiplier *_playerSpeed * rightVector)
                              + (Vector3.up * _playerRigidbody.linearVelocity.y);
        _playerRigidbody.AddForce(targetVel - _playerRigidbody.linearVelocity, ForceMode.VelocityChange);
    }
    private void RotatePlayer()
    {
        Vector3 forwardVector = _cameraTransform.forward.normalized;
        Vector3 rightVector = _cameraTransform.right.normalized;
        Vector3 targetDirection = ((_playerLocomotion.y * forwardVector) + (_playerLocomotion.x * rightVector)).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion newRotation = Quaternion.Slerp(_playerOrientation.rotation, targetRotation, Time.deltaTime * _rotationTime);
        
        _playerOrientation.SetPositionAndRotation(_playerOrientation.position, newRotation);
    }

    private Vector2 _playerLocomotion = Vector2.zero;
    [SerializeField] private InputTranslator _inputTranslator;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private Transform _playerOrientation;
    private Rigidbody _playerRigidbody;
    private float _strafeSpeedMultiplier = 1;
    private float _forwardSpeedMultiplier = 1;
    [SerializeField] float _playerSpeed = 100;
    [SerializeField] float _jumpForce = 100;
    [SerializeField] float _rotationTime = 10;
}

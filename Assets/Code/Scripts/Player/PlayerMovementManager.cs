using System.Runtime.CompilerServices;
using UnityEngine;
using static TempoManager;

public class PlayerMovementManager : MonoBehaviour
{
    private void Awake()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
        _tempoManager = GameObject.Find("TempoManager").GetComponent<TempoManager>();
    }
    private void Start()
    {
        _inputTranslator.OnMovementEvent += HandleMovement;
        _inputTranslator.OnMousePrimaryInteractionEvent += HandleMousePrimaryInteraction;
    }
    private void OnDestroy()
    {
        _inputTranslator.OnMovementEvent -= HandleMovement;
        _inputTranslator.OnMousePrimaryInteractionEvent -= HandleMousePrimaryInteraction;
    }

    private void FixedUpdate()
    {
        MovePlayer();
        if(!IsGrounded()) _playerRigidbody.AddForce(-9.8f * Vector3.up , ForceMode.Impulse);
    }

    public bool IsGrounded()
    {
        return Physics.BoxCast(transform.position, _groundCheckBoxDimensions, Vector3.down, transform.rotation, _groundCheckBoxHeight);
    }
    private void MovePlayer()
    {
        Vector3 forwardVector = _cameraTransform.forward.normalized;
        Vector3 rightVector = _cameraTransform.right.normalized;
        Vector3 targetVel = (_playerLocomotion.y * _forwardSpeedMultiplier * _playerSpeed * forwardVector)
                              + (_playerLocomotion.x * _strafeSpeedMultiplier * _playerSpeed * rightVector)
                              + (Vector3.up * _playerRigidbody.linearVelocity.y);
        _playerRigidbody.AddForce(targetVel - _playerRigidbody.linearVelocity, ForceMode.VelocityChange);
    }

    public void HandleMovement(Vector2 locomotion)
    {
        _playerLocomotion = locomotion;
    }
    public void HandleMousePrimaryInteraction()
    {
        switch (_tempoManager.CheckHitQuality())
        {
            case HIT_QUALITY.EXCELLENT:
                Debug.Log("EXCELLENT");
                break;
            case HIT_QUALITY.GOOD:
                Debug.Log("GOOD");
                break;
            case HIT_QUALITY.BAD:
                Debug.Log("BAD");
                break;
            default:
                Debug.Log("MISS");
                break;
        }
        ;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + (Vector3.down * _groundCheckBoxHeight), _groundCheckBoxDimensions);
    }

    private Vector2 _playerLocomotion = Vector2.zero;
    [SerializeField] private InputTranslator _inputTranslator;
    [SerializeField] private Transform _cameraTransform;
    private TempoManager _tempoManager;
    private Rigidbody _playerRigidbody;
    private float _strafeSpeedMultiplier = 1;
    private float _forwardSpeedMultiplier = 1;
    [SerializeField] float _playerSpeed = 100;
    [SerializeField] private Vector3 _groundCheckBoxDimensions;
    [SerializeField] private float _groundCheckBoxHeight;

}

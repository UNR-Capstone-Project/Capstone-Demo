using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using static TempoManagerV2;
using UnityEngine.Windows;

public class Player : MonoBehaviour
{
    //Player Movement
    private Vector2 _playerLocomotion = Vector2.zero;
    [SerializeField] private InputTranslator _inputTranslator;
    
    [SerializeField] private Transform _cameraTransform;
    private Transform _mainCameraRef;
    private TempoManagerV2 _tempoManager;
    private Rigidbody _playerRigidbody;
    private float _strafeSpeedMultiplier = 1;
    private float _forwardSpeedMultiplier = 1;
    [SerializeField] float _playerSpeed = 100;
    [SerializeField] private Vector3 _groundCheckBoxDimensions;
    [SerializeField] private float _groundCheckBoxHeight;

    //Player Stats
    public float playerHealth = 10f;
    public float attackWait = 1f;

    //Player Components
    public GameObject playerSprite;
    public GameObject fireballPrefab;
    public GameObject musicMiniGamePrefab;
    private Animator playerAnimator;
    private SpriteRenderer playerSpriteRenderer;

    //Player Attacking
    private bool isAttackCooldown = false;
    public Color flashColor = Color.red;
    private Color originalColor;
    private float flashDuration = 0.2f;

    private Quaternion playerRotation;

    //Minigame Variables
    private bool miniGameOpened = false;

    private void Awake()
    {
        playerAnimator = playerSprite.GetComponent<Animator>();
        playerSpriteRenderer = playerSprite.GetComponent<SpriteRenderer>();
        originalColor = playerSpriteRenderer.material.GetColor("_TintColor");

        /*
        leftClickAction = new InputAction(type: InputActionType.Button, binding: "<Mouse>/leftButton");
        leftClickAction.performed += mouseLeftClick;
        leftClickAction.Enable();

        rightClickAction = new InputAction(type: InputActionType.Button, binding: "<Mouse>/rightButton");
        rightClickAction.performed += mouseRightClick;
        rightClickAction.Enable();
        */

        //playerRotation = Quaternion.Euler(0f, 45f, 0f);
        _playerRigidbody = GetComponent<Rigidbody>();
        _tempoManager = GameObject.Find("TempoManager").GetComponent<TempoManagerV2>();
        _mainCameraRef = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }
    private void Start()
    {
        _inputTranslator.OnMovementEvent += HandleMovement;
        _inputTranslator.OnMousePrimaryInteractionEvent += HandleMousePrimaryInteraction;

        //Setting the y-axis rotation of PlayerCamera so it is aligned with the MainCamera orientation on the y-axis
        _cameraTransform.SetLocalPositionAndRotation(_cameraTransform.localPosition, 
                                                     Quaternion.Euler(0 ,_mainCameraRef.localRotation.eulerAngles.y, 0));
    }
    private void OnDestroy()
    {
        _inputTranslator.OnMovementEvent -= HandleMovement;
        _inputTranslator.OnMousePrimaryInteractionEvent -= HandleMousePrimaryInteraction;
    }

    private void FixedUpdate()
    {
        MovePlayer();
        if (!IsGrounded()) _playerRigidbody.AddForce(-9.8f * Vector3.up, ForceMode.Impulse);
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

        if (Mathf.Abs(locomotion.x) > 0)
        {
            Vector3 currentScale = playerSprite.transform.localScale;
            currentScale.x = Mathf.Sign(locomotion.x) * Mathf.Abs(currentScale.x);
            playerSprite.transform.localScale = currentScale;
        }

        if (locomotion.x != 0 || locomotion.y != 0) //Player is running
        {
            playerAnimator.SetBool("isRunning", true);
        }
        else
        {
            playerAnimator.SetBool("isRunning", false);
        }
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
        };
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + (Vector3.down * _groundCheckBoxHeight), _groundCheckBoxDimensions);
    }

    private void mouseLeftClick(InputAction.CallbackContext context) //Projectile Attack
    {
        if (!isAttackCooldown)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;
            int groundMask = LayerMask.GetMask("Ground");

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundMask))
            {
                Vector3 hitPoint = hit.point;
                Vector3 targetVec = hitPoint - transform.position;
                Vector3 projectileDirection = targetVec.normalized;

                if (projectileDirection.y < 0f) { projectileDirection.y = 0f; } //Clamp to angles above 0 degrees horizontally
                projectileDirection.Normalize();

                Vector3 spawnPos = transform.position + projectileDirection * 0.5f;
                GameObject fireballInstance = Instantiate(fireballPrefab, spawnPos, Quaternion.identity);
                fireballInstance.GetComponent<ProjectileAttack>().setProjectileDirection(projectileDirection);

                isAttackCooldown = true;
                StartCoroutine(AttackCooldown(attackWait));
            }
        }
    }
    public void closeMiniGame() { miniGameOpened = false; }

    private void mouseRightClick(InputAction.CallbackContext context) //Special Attack
    {
        //ISSUE: Determine type of weapon being used, and open the corresponding mini-game.
        if (!miniGameOpened)
        {
            GameObject musicMiniGameInstance = Instantiate(musicMiniGamePrefab);
            musicMiniGameInstance.name = musicMiniGamePrefab.name;
            miniGameOpened = true;
        }
    }

    IEnumerator AttackCooldown(float cooldownTime)
    {
        yield return new WaitForSeconds(cooldownTime);
        isAttackCooldown = false;
    }

    public void takeDamage(float damageAmount)
    {
        playerHealth -= damageAmount;
        StopCoroutine(flashPlayer());
        StartCoroutine(flashPlayer());
    }

    IEnumerator flashPlayer()
    {
        playerSpriteRenderer.material.SetColor("_TintColor", flashColor);
        yield return new WaitForSeconds(flashDuration);
        playerSpriteRenderer.material.SetColor("_TintColor", originalColor);
    }
}

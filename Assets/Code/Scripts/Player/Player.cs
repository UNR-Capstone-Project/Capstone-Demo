using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using static TempoManagerV2;
using UnityEngine.Windows;

public class Player : MonoBehaviour
{
    //Player Movement
    private Vector2 playerLocomotion = Vector2.zero;
    [SerializeField] private InputTranslator inputTranslator;
    
    private Transform mainCameraRef;
    private TempoManagerV2 tempoManager;
    private Rigidbody playerRigidbody;
    private float strafeSpeedMultiplier = 1;
    private float forwardSpeedMultiplier = 1;
    [SerializeField] float playerSpeed = 100;
    [SerializeField] private Vector3 groundCheckBoxDimensions;
    [SerializeField] private float groundCheckBoxHeight;

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
        playerRigidbody = GetComponent<Rigidbody>();
        tempoManager = GameObject.Find("TempoManager").GetComponent<TempoManagerV2>();
        mainCameraRef = Camera.main.transform;
    }
    private void Start()
    {
        inputTranslator.OnMovementEvent += HandleMovement;
        inputTranslator.OnMousePrimaryInteractionEvent += HandleMousePrimaryInteraction;
    }
    private void OnDestroy()
    {
        inputTranslator.OnMovementEvent -= HandleMovement;
        inputTranslator.OnMousePrimaryInteractionEvent -= HandleMousePrimaryInteraction;
    }

    private void FixedUpdate()
    {
        MovePlayer();

        if (!IsGrounded())
        {
            playerRigidbody.AddForce(-9.8f * Vector3.up, ForceMode.Impulse);
        }
    }

    public bool IsGrounded()
    {
        return Physics.BoxCast(transform.position, groundCheckBoxDimensions, Vector3.down, transform.rotation, groundCheckBoxHeight);
    }
    private void MovePlayer()
    {
        Vector3 forwardVector = mainCameraRef.forward;
        forwardVector.y = 0f;
        forwardVector = forwardVector.normalized;

        Vector3 rightVector = mainCameraRef.right;
        rightVector.y = 0f;
        rightVector = rightVector.normalized;
        
        Vector3 targetVel = (playerLocomotion.y * forwardSpeedMultiplier * playerSpeed * forwardVector)
                          + (playerLocomotion.x * strafeSpeedMultiplier * playerSpeed * rightVector)
                          + (Vector3.up * playerRigidbody.linearVelocity.y);

        playerRigidbody.AddForce(targetVel - playerRigidbody.linearVelocity, ForceMode.VelocityChange);
    }

    public void HandleMovement(Vector2 locomotion)
    {
        playerLocomotion = locomotion;

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
        switch (tempoManager.CheckHitQuality())
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

        ProjectileAttack();
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + (Vector3.down * groundCheckBoxHeight), groundCheckBoxDimensions);
    }

    private void ProjectileAttack()
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

    private void OpenMinigame()
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

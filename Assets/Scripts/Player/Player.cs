using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float playerSpeed = 5f;
    public float gravityStrength = 9.8f;
    public float playerHealth = 10f;

    public GameObject playerSprite;
    public GameObject fireballPrefab;

    private CharacterController characterController;
    private Animator playerAnimator;

    private InputAction moveAction;
    private InputAction leftClickAction;
    private float vSpeed;

    private Quaternion playerRotation;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerAnimator = playerSprite.GetComponent<Animator>();

        leftClickAction = new InputAction(type: InputActionType.Button, binding: "<Mouse>/leftButton");
        leftClickAction.performed += mouseLeftClick;
        leftClickAction.Enable();

        moveAction = new InputAction(type: InputActionType.Value);
        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Left", "<Keyboard>/a")
            .With("Down", "<Keyboard>/s")
            .With("Right", "<Keyboard>/d");

        moveAction.Enable();

        playerRotation = Quaternion.Euler(0f, 45f, 0f);
    }

    void Update()
    {
        if (!characterController.isGrounded)
        {
            vSpeed -= gravityStrength * Time.deltaTime;
        }
        else if (vSpeed < 0) { vSpeed = 0f; } //Clamp

        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 horizontal = new Vector3(input.x, 0, input.y);
        Vector3 vertical = new Vector3(0f, vSpeed, 0f);

        Vector3 rotatedHorizontal = playerRotation * horizontal;
        Vector3 moveDir = rotatedHorizontal + vertical;

        characterController.Move(moveDir * playerSpeed * Time.deltaTime);

        if (Mathf.Abs(input.x) > 0)
        {
            Vector3 currentScale = playerSprite.transform.localScale;
            currentScale.x = Mathf.Sign(input.x) * Mathf.Abs(currentScale.x);
            playerSprite.transform.localScale = currentScale;
        }

        if (input.x != 0 || input.y != 0) //Player is running
        {
            playerAnimator.SetBool("isRunning", true);
        }
        else
        {
            playerAnimator.SetBool("isRunning", false);
        }
    }

    private void mouseLeftClick(InputAction.CallbackContext context)
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
        }
    }

    public void takeDamage(float damageAmount)
    {
        playerHealth -= damageAmount;
    }
}

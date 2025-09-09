using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float playerSpeed = 5f;
    public float gravityStrength = 9.8f;
    public GameObject playerSprite;

    private CharacterController characterController;
    private InputAction moveAction;
    private float vSpeed;
    private Quaternion playerRotation;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();

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
            currentScale.x = -Mathf.Sign(input.x) * Mathf.Abs(currentScale.x);
            playerSprite.transform.localScale = currentScale;
        }
    }
}

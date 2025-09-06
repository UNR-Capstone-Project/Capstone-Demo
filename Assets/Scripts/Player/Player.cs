using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float playerSpeed = 5f;

    private CharacterController characterController;
    private InputAction moveAction;
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
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 moveDir = new Vector3(input.x, 0, input.y);
        Vector3 rotatedMoveDir = playerRotation * moveDir;
        characterController.Move(playerSpeed * Time.deltaTime * rotatedMoveDir);
    }
}

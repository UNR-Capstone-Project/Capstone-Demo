using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Player Input Translator")]
public class InputTranslator : ScriptableObject, PlayerInput.IGameplayActions
{
    private PlayerInput _playerInputs;
    public event Action<Vector2> OnLocomotionEvent;
    public event Action OnJumpEvent;

    private void Awake()
    {
        _playerInputs = new PlayerInput();
        _playerInputs.Gameplay.SetCallbacks(this);
    }
    private void OnEnable()
    {
        if(_playerInputs == null)
        {
            _playerInputs = new PlayerInput();
            _playerInputs.Gameplay.SetCallbacks(this);
        }
        _playerInputs.Gameplay.Enable();
    }
    private void OnDisable()
    {
        _playerInputs.Gameplay.Disable();
    }
    private void OnDestroy()
    {
        _playerInputs.Gameplay.RemoveCallbacks(this);
        _playerInputs = null;
    }

    public void OnJump(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (context.started) OnJumpEvent?.Invoke();
    }

    public void OnLocomotion(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        OnLocomotionEvent?.Invoke(context.ReadValue<Vector2>());
    }

}

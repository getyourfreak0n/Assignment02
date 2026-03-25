using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    NewInputActions inputActions;
    
    public Action jumpAction;
    public Vector2 moveVector;

    void Awake()
    {
        inputActions = new NewInputActions();
    }

    void OnEnable()
    {
        inputActions.Enable();

        inputActions.Player.Move.performed += onMove;
        inputActions.Player.Move.canceled += onMoveCanceled;
        inputActions.Player.Jump.performed += onJump;

    }
    void OnDisable()
    {
        inputActions.Disable();
        
        inputActions.Player.Move.performed -= onMove;
        inputActions.Player.Move.canceled -= onMoveCanceled;
        inputActions.Player.Jump.performed -= onJump;
    }
    
    void onMove(InputAction.CallbackContext ctx)
    {
        moveVector = ctx.ReadValue<Vector2>();
    }

    void onMoveCanceled(InputAction.CallbackContext ctx)
    { 
        moveVector = Vector2.zero;
    }

    void onJump(InputAction.CallbackContext ctx)
    {
        jumpAction?.Invoke();
    }
    
    
    
}

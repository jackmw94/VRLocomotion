using UnityEngine;

#if ENABLE_INPUT_SYSTEM
public class InputSystemLocomotionController : VRLocomotionController
{
    [SerializeField] private InputAction inputAction;

    private bool isDown = false;
    private bool isHeld = false;
    
    public override bool IsDown => isDown;
    public override bool IsHeld => isHeld;

    private void OnEnable()
    {
        inputAction.Enable();
        
        inputAction.started += OnDown;
        inputAction.performed += OnHeld;
        inputAction.canceled += OnReleased;
    }

    private void OnDisable()
    {
        inputAction.Disable();
        
        inputAction.started -= OnDown;
        inputAction.performed -= OnHeld;
        inputAction.canceled -= OnReleased;
    }

    private void OnDown(InputAction.CallbackContext context)
    {
        isDown = true;
        isHeld = true;
    }

    private void OnHeld(InputAction.CallbackContext context)
    {
        if (context.control.IsPressed())
        {
            isDown = false;
            isHeld = true;
        }
    }

    private void OnReleased(InputAction.CallbackContext context)
    {
        isDown = false;
        isHeld = false;
    }
}
#endif
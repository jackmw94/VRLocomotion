using UnityEngine;

#if ENABLE_LEGACY_INPUT_MANAGER
public class LegacyInputLocomotionController : VRLocomotionController
{
    [SerializeField] private string buttonName;
    
    public override bool IsDown => Input.GetButtonDown(buttonName);
    public override bool IsHeld => Input.GetButton(buttonName);
}
#endif
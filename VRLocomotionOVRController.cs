using UnityEngine;

public class VRLocomotionOVRController : VRLocomotionController
{
    [SerializeField] private OVRInput.Controller controller;
    [SerializeField] private OVRInput.Button button;

    public override bool IsDown => OVRInput.GetDown(button, controller);
    public override bool IsHeld => OVRInput.Get(button, controller);
}
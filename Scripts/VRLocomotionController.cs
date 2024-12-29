using UnityEngine;

public abstract class VRLocomotionController : MonoBehaviour
{
    public abstract bool IsDown { get; }
    public abstract bool IsHeld { get; }
}
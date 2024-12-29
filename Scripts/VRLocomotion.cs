using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VRLocomotion : MonoBehaviour
{
    [SerializeField] private Transform headTransform;
    [SerializeField] private VRLocomotionController[] controllers;
    [Space]
    [SerializeField] private bool allowScaling = false;
    [SerializeField] private float lerpSpeed = 30f;

    private Dictionary<VRLocomotionController, Transform> markers = new();
    
    private Transform playerProxy;
    private Transform controllerProxy;

    private bool wasUsingTwoHands = false;

    private void Awake()
    {
        playerProxy = new GameObject("[Generated] Player Proxy") { hideFlags = HideFlags.DontSave }.transform;
        controllerProxy = new GameObject("[Generated] Main Controller Proxy") { hideFlags = HideFlags.DontSave }.transform;
    }

    private void Update()
    {
        foreach (VRLocomotionController controller in controllers)
        {
            if (controller.IsDown)
            {
                SetMarker(controller);
            }
        }

        VRLocomotionController[] heldControllers = controllers.Where(p => p.IsHeld).ToArray();
        Debug.Assert(heldControllers.Length <= 2);
        bool isUsingOneHand = heldControllers.Length == 1;
        bool isUsingTwoHands = heldControllers.Length == 2;

        if (isUsingTwoHands)
        {
            TwoHandedLocomotion(heldControllers[0], heldControllers[1]);
        }

        if (isUsingOneHand)
        {
            if (wasUsingTwoHands)
            {
                // Force reset marker when one hand released and other still holding
                SetMarker(heldControllers[0]);
            }
            
            OneHandedLocomotion(heldControllers[0]);
        }
        
        wasUsingTwoHands = isUsingTwoHands;
    }

    private void TwoHandedLocomotion(VRLocomotionController controller1, VRLocomotionController controller2)
    {
        if (allowScaling)
        {
            float scaleChange = GetScaleChange(controller1, controller2);

            controllerProxy.localScale = Vector3.one;
            controllerProxy.position = headTransform.position;

            transform.parent = controllerProxy;
            controllerProxy.localScale = scaleChange * Vector3.one;
            transform.SetParent(null);
        }


        Pose targetPose = GetAverageTargetPose(controller1, controller2);
        transform.position = Vector3.Lerp(transform.position, targetPose.position, lerpSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetPose.rotation, lerpSpeed * Time.deltaTime);
    }

    private void OneHandedLocomotion(VRLocomotionController controller)
    {
        Pose targetPose = GetTargetPose(controller);

        transform.position = Vector3.Lerp(transform.position, targetPose.position, lerpSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetPose.rotation, lerpSpeed * Time.deltaTime);
    }

    private float GetScaleChange(VRLocomotionController controller1, VRLocomotionController controller2)
    {
        Transform controller1Marker = GetMarker(controller1);
        Transform controller2Marker = GetMarker(controller2);
        
        float markerDistance = (controller1Marker.position - controller2Marker.position).magnitude;
        float controllerDistance = (controller1.transform.position - controller2.transform.position).magnitude;

        float scaleChange = (markerDistance / controllerDistance);
        return scaleChange;
    }

    private Pose GetAverageTargetPose(VRLocomotionController controller1, VRLocomotionController controller2)
    {
        Pose pose1 = GetTargetPose(controller1);
        Pose pose2 = GetTargetPose(controller2);

        Vector3 targetPosition = Vector3.Lerp(pose1.position, pose2.position, 0.5f);
        Quaternion targetRotation = Quaternion.Lerp(pose1.rotation, pose2.rotation, 0.5f);

        return new Pose(targetPosition, targetRotation);
    }

    private Pose GetTargetPose(VRLocomotionController controller)
    {
        MatchTransforms(playerProxy, transform);
        MatchTransforms(controllerProxy, controller.transform);
        
        Transform marker = GetMarker(controller);

        playerProxy.parent = controllerProxy;
        MatchTransforms(controllerProxy, marker);
        playerProxy.SetParent(null);

        return new Pose(playerProxy.position, playerProxy.rotation);
    }

    private void SetMarker(VRLocomotionController controller)
    {
        Transform marker = GetMarker(controller);
        MatchTransforms(marker, controller.transform);
    }
    
    private void MatchTransforms(Transform move, Transform match)
    {
        move.position = match.position;
        move.rotation = match.rotation;
    }

    private Transform GetMarker(VRLocomotionController controller)
    {
        if (!markers.TryGetValue(controller, out Transform marker))
        {
            marker = new GameObject($"[Generated] Hold Marker ({controller.name})") { hideFlags = HideFlags.DontSave }.transform;
            markers.Add(controller, marker);
        }
        
        return marker;
    }
}
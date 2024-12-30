# VRLocomotion
A very simple method of getting around in VR via "swimming". Grab and pull with controllers + (optional) scaling when controllers are squeeze or pulled apart.

## Installation
In the top left of the `Packages` window, navigate to `Add Package -> Add package from git URL` and paste `https://github.com/jackmw94/VRLocomotion.git`.

## Setup
Add the VRLocomotion component to the camera rig's root object. On each controller transform add one of the VRLocomotionController subclasses depending on the type of input your project is using:
* If you have the oculus SDK in your project then you'll have access to the OVRLocomotionController components which can receive input through OVRInput system
* If you're using the legacy input system then use the LegacyInputLocomotionController which references a button name as defined in the input manager in player settings
* If you're using the new input system then use the InputSystemLocomotionController which uses a controller binding to receive input
You can also extend this class should these components not fit your needs, the component provides is properties for OnDown and OnHeld, and its attached controller-tracked transform.

Finally, make sure the VRLocomotion component references the controller components and hit play.

VRLocomotion has two parameters:
* 'Lerp speed' controls how quickly the transform reaches its target pose, smoothing out real movement at the expense of lag. I believe ~20 is a nice happy medium
* 'Allow scale' determines whether grabbing with both controllers can affect player scale, growing when you grab and move your hands together, shrinking when you grab and move your hands apart.

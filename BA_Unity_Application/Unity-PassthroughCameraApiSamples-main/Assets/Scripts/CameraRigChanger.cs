using UnityEngine;
using Oculus.Platform; // Required for using Oculus Input

public class CameraRigChanger : MonoBehaviour {
    [Header("Camera Rigs")]
    [Tooltip("The OVRCameraRig used for the setup mode.")]
    public GameObject setupCameraRig;
    [Tooltip("The OVRCameraRig used for the cooking mode.")]
    public GameObject cookingCameraRig;

    public Transform setupCameraRigTrackingSpace;
    public Transform cookingCameraRigTrackingSpace;
    public Transform agent;

    // Flag to track the currently active mode.
    private bool isSetupModeActive = true;

    // Controller type used to cast the ray (default: right controller)
    [SerializeField]
    private OVRInput.Controller controllerType = OVRInput.Controller.LTouch; // Default is right controller

    [SerializeField]
    [Header("Input Settings")]
    [Tooltip("The controller button used to switch camera rigs.")]
    private OVRInput.RawButton switchButton = OVRInput.RawButton.X; // A button on Oculus controller (e.g., A on Right Touch or X on Left Touch)

    //[SerializeField]
    //private OVRInput.RawButton grabButton = OVRInput.RawButton.A;

    // Option: Which controller's input to accept.
    [Tooltip("The controller to use for switching camera rigs (e.g., RHand, LHand, or All for both).")]
    private OVRInput.Controller inputController = OVRInput.Controller.All;

    void Start() {
        // Ensure the initial state is set correctly.
        if (setupCameraRig != null) {
            setupCameraRig.SetActive(true);
        }
        if (cookingCameraRig != null) {
            cookingCameraRig.SetActive(false);
        }
        isSetupModeActive = true;

        if (setupCameraRig == null || cookingCameraRig == null) {
            Debug.LogError("CameraRigChanger: setupCameraRig or cookingCameraRig has not been assigned.", this);
            enabled = false; // Disable the script.
        }
    }

    void Update() {
        // Detect when the specified button is pressed down.
        if (OVRInput.GetDown(switchButton, controllerType)) {
            Debug.Log(switchButton);
            ToggleCameraRigs();
        }
    }

    /// <summary>
    /// Toggles the currently active CameraRig.
    /// </summary>
    public void ToggleCameraRigs() {
        if (isSetupModeActive) {
            if (setupCameraRig == null || cookingCameraRig == null)
            {
                Debug.LogError("CameraRigChanger: Cannot switch because CameraRigs are not set up correctly.", this);
                return;
            }

            if (isSetupModeActive)
            {
                // Switch from Setup Mode -> Cooking Mode
                Debug.Log("CameraRigChanger: Switching to Cooking Mode.");
                setupCameraRig.SetActive(false);
                cookingCameraRig.SetActive(true);

                cookingCameraRig.transform.position = setupCameraRig.transform.position;
                cookingCameraRigTrackingSpace.localPosition = setupCameraRigTrackingSpace.localPosition;
                //agent.position = cookingCameraRigTrackingSpace.position;
                ResetAgentVelocity();
            }
            else
            {
                // Switch from Cooking Mode -> Setup Mode
                Debug.Log("CameraRigChanger: Switching to Setup Mode.");
                setupCameraRig.SetActive(true);
                cookingCameraRig.SetActive(false);
            }
            // Invert the flag.
            isSetupModeActive = !isSetupModeActive;
        }
    }
        /// <summary>
        /// Resets the velocity of the agent's Rigidbody to zero.
        /// </summary>
    private void ResetAgentVelocity()
    {
        if (agent == null) return;

        Rigidbody agentRigidbody = agent.GetComponent<Rigidbody>();
        if (agentRigidbody != null)
        {
            agentRigidbody.useGravity = true;
        }
    }

}

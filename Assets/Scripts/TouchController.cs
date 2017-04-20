using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour {
    public GameObject attachPoint;
    public string hardware;

    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
    private SteamVR_TrackedObject trackedObj;
    public GameObject pickup;
    public bool dragging;
    public bool triggerDown;
    public bool deferredTriggerExit;


    // Use this for initialization
    void Start() {
        deferredTriggerExit = false;
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        DetectVRHardware();
    }

    public void DetectVRHardware() {
        string model = UnityEngine.VR.VRDevice.model != null ? UnityEngine.VR.VRDevice.model : "";
        if (model.IndexOf("Rift") >= 0) {
            hardware = "oculus_touch";
        } else {
            hardware = "htc_vive";
        }
    }

    // Update is called once per frame
    void Update() {
        if (controller == null) {
            Debug.Log("[" + trackedObj.index + "] Controller not initialized");
            return;
        }

        // Handle Trigger Down
        if (!dragging && controller.GetPressDown(triggerButton)) {
            //Debug.Log("[" + trackedObj.index + "] trigger pressed at " + this.transform.position.ToString());
            if (pickup != null && pickup.GetComponent<TouchableController>()) {
                pickup.GetComponent<TouchableController>().onTriggerDown.Invoke(this.transform, (int)trackedObj.index);
                triggerDown = true;
            }
        }

        if (controller.GetPressUp(triggerButton)) {
            if (pickup != null && pickup.GetComponent<TouchableController>() && pickup.GetComponent<TouchableController>().onTriggerUp != null) {
                pickup.GetComponent<TouchableController>().onTriggerUp.Invoke(this.transform, (int)trackedObj.index);
                triggerDown = false;
                if (deferredTriggerExit) {
                    pickup = null; // this happens when holding the trigger while moving away from a collider, then releasing outside the collider.
                    deferredTriggerExit = false;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider collider) {
        if (!dragging && !triggerDown) {
            pickup = collider.gameObject;
        } 
    }

    private void OnTriggerExit(Collider collider) {
        if (triggerDown) deferredTriggerExit = true;
    }
}

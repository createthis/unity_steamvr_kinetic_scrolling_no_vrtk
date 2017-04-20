using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KineticScroller : MonoBehaviour {
    public FixedJoint fixedJoint;

    private Rigidbody rigidBody;

    public void OnTriggerDown(Transform controller, int controllerIndex) {
        GameObject attachPoint = controller.GetComponent<TouchController>().attachPoint;

        Rigidbody controllerRigidbody = attachPoint.GetComponent<Rigidbody>();
        if (!controllerRigidbody) {
            controllerRigidbody = attachPoint.AddComponent<Rigidbody>();
            controllerRigidbody.isKinematic = true;
            controllerRigidbody.useGravity = true;
        }
        fixedJoint = gameObject.AddComponent<FixedJoint>();
        fixedJoint.connectedBody = controllerRigidbody;
        fixedJoint.breakForce = Mathf.Infinity;
        fixedJoint.breakTorque = Mathf.Infinity;
    }

    public void OnTriggerUpdate(Transform controller, int controllerIndex) {

    }

    public void OnTriggerUp(Transform controller, int controllerIndex) {
        if (!fixedJoint) return;
        fixedJoint.connectedBody = null;
        Destroy(fixedJoint);
        SteamVR_Controller.Device device = SteamVR_Controller.Input((int)controllerIndex);
        rigidBody.velocity = device.velocity;
        rigidBody.angularVelocity = device.angularVelocity;
    }

    // Use this for initialization
    void Start() {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;
    }

    // Update is called once per frame
    void Update() {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragController : MonoBehaviour {

    public Camera worldCamera;

    public LayerMask dragLayer;

    public float damping = 1.0f;

    public float frequency = 5.0f;

    private TargetJoint2D targetJoint;

    private void Update() {
        if (!GameManager.started)
            return;

        var worldPos = worldCamera.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0)) {
            var collider = Physics2D.OverlapPoint(worldPos, dragLayer);

            if (collider == null)
                return;

            var body = collider.attachedRigidbody;

            if (body == null)
                return;

            targetJoint = body.gameObject.AddComponent<TargetJoint2D>();
            targetJoint.dampingRatio = damping;
            targetJoint.frequency = frequency;

            targetJoint.anchor = targetJoint.transform.InverseTransformPoint(worldPos);
        }

        if (Input.GetMouseButtonUp(0)) {
            Destroy(targetJoint);
            targetJoint = null;
        }

        if (targetJoint != null) {
            targetJoint.target = worldPos;
        }
    }

    public void Clean() {
        Destroy(targetJoint);
        targetJoint = null;
    }
}

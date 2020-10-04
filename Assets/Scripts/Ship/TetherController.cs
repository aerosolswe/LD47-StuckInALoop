using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherController : MonoBehaviour {

    private List<HingeJoint2D> tethers = new List<HingeJoint2D>();

    public LineRenderer lr;

    public HingeJoint2D tetherPart;

    public HingeJoint2D startJoint;
    public HingeJoint2D endJoint; // astronaut

    public void Generate() {
        for (int i = 0; i < tethers.Count; i++) {
            Destroy(tethers[i]);
        }
        tethers.Clear();

        float dst = Vector3.Distance(startJoint.transform.position, endJoint.transform.position);

        int amount = Mathf.RoundToInt(dst / tetherPart.transform.localScale.y);
        
        for (int i = 0; i < amount; i++) {
            HingeJoint2D hj = (HingeJoint2D)Instantiate(tetherPart, this.transform);
            hj.useLimits = true;

            if (i == 0) {
                hj.connectedBody = startJoint.attachedRigidbody;
                
                Vector3 pos = hj.transform.localPosition;
                pos.y -= tetherPart.transform.localScale.y;
                hj.transform.localPosition = pos;

            } else {
                Vector3 pos = tethers[i - 1].transform.localPosition;
                pos.y -= tetherPart.transform.localScale.y;
                hj.transform.localPosition = pos;
                
                hj.connectedBody = tethers[i - 1].attachedRigidbody;

                if (i == (amount - 1)) {
                    endJoint.connectedBody = hj.attachedRigidbody;
                }
            }

            tethers.Add(hj);
        }
        
    }

    public void Clean() {
        for (int i = 0; i < tethers.Count; i++) {
            Destroy(tethers[i]);
        }
        tethers.Clear();

        lr.positionCount = 0;
    }

    private void Update() {
        if (tethers.Count > 0) {
            Vector3[] positions = new Vector3[tethers.Count];

            for (int i = 0; i < tethers.Count; i++) {
                positions[i] = tethers[i].transform.position;
            }

            lr.positionCount = tethers.Count;
            lr.SetPositions(positions);
            
        }
    }
}

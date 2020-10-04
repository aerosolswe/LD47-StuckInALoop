using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!GameManager.started)
            return;

        if (collision.tag == "Meteor") {
            Meteor m = collision.GetComponent<Meteor>();
            if (m != null)
                m.Remove();
        } else if (collision.tag == "Player") {
            HealthBar.instance.Health = 0;
        }
    }
}

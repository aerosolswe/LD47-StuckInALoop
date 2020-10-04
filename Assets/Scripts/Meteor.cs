using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour {

    private MeteorsManager mm;

    private Rigidbody2D body;
    private Animator animator;

    private Vector3 direction = Vector3.up;

    public int damage = 25;

    public float force = 5f;

    public float torque = 0;

    private bool destroyed = false;

    public void Init(MeteorsManager mm) {
        this.mm = mm;

        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        torque = Random.Range(-10, 10);

        body.AddTorque(torque);
        body.AddForce(direction * force, ForceMode2D.Force);
    }

    public void SetDirection(Vector3 direction) {
        this.direction = direction;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (destroyed)
            return;

        if (collision.transform.tag == "Player" || collision.transform.tag == "Ship") {
            HealthBar.instance.TakeDamage(collision.transform.tag, damage);
            Remove();
        }
    }

    public void Remove() {
        destroyed = true;

        mm.meteors.Remove(this);

        body.velocity = Vector3.zero;
        GetComponent<Collider2D>().enabled = false;

        animator.SetTrigger("destroy");
    }

    public void Destroy() {
        Destroy(this.gameObject);
    }

}

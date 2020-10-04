using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour {

    public Transform flameTransform;

    public TetherController tc;

    private Rigidbody2D body;
    private AudioSource source;

    public float currentForce = 0;
    public float maxForce = 750;

    public float maxTime = 15f;
    private float time = 0;

    private float randomDelay = 2;
    private float currentDelay = 0;

    public float randomModifier = 1;

    private void Start() {
        body = GetComponent<Rigidbody2D>();
        source = GetComponent<AudioSource>();
    }

    private void Update() {
        if (!GameManager.started)
            return;

        UpdateFrameScale(currentForce);

        currentDelay -= Time.deltaTime;

        if (currentDelay <= 0) {
            currentDelay = randomDelay;

            randomModifier = Random.Range(0.1f, 1.4f);
        }

        time += Time.deltaTime;

        if (time > maxTime) {
            time = maxTime;
        }

        float t = time / maxTime;

        currentForce = Mathf.Lerp(0, maxForce, t) * randomModifier;
    }

    public float startForce = 50;

    public IEnumerator startRoutine() {
        while (transform.localPosition.y < 0) {
            yield return new WaitForFixedUpdate();
            body.AddForce(transform.up * startForce, ForceMode2D.Force);
            UpdateFrameScale(startForce * 100);
        }
    }

    private void FixedUpdate() {
        if (!GameManager.started)
            return;

        body.AddForce(transform.up * currentForce, ForceMode2D.Force);
    }

    public void Clean(Vector3 resetPos) {
        currentForce = 0;
        currentDelay = 0;
        time = 0;
        body.velocity = Vector3.zero;
        body.angularVelocity = 0;
        body.transform.localRotation = Quaternion.identity;
        body.MovePosition(resetPos);
    }

    private void UpdateFrameScale(float force) {
        Vector3 scale = flameTransform.localScale;
        float t = force / maxForce;
        scale.y = Mathf.Lerp(0, scale.x, t);
        flameTransform.localScale = scale;

        float vol = Mathf.Lerp(0, 0.8f, t);
        vol = Mathf.Clamp(vol, 0, 0.8f);
        source.volume = vol;
    }
}

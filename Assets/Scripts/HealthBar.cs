using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour {

    public static HealthBar instance;

    public AudioClip[] grunts;
    public AudioClip scream;
    public AudioClip pow;

    public float fullHealthScale = 4.864188f;

    public Transform parent;
    public Transform foreground;

    private int health = 100;

    private void Start() {
        instance = this;
        Health = 100;
    }

    public void Show(bool enable) {
        parent.gameObject.SetActive(enable);
    }

    public void TakeDamage(string src, int amount) {
        if (Health <= 0) {
            return;
        }

        if (src == "Player") {
            AudioObject.PlaySound(grunts[Random.Range(0, grunts.Length)], 0.6f, Random.Range(1, 1.1f));
        } else if (src == "Ship") {
            AudioObject.PlaySound(pow, 0.6f, Random.Range(1, 1.1f));
        }

        Health -= amount;
    }

    public int Health {
        get {
            return health;
        }

        set {
            health = value;

            if (health <= 0 && GameManager.started) {
                health = 0;

                AudioObject.PlaySound(scream, 0.6f, Random.Range(1, 1.1f));
                GameManager.instance.EndGame();
            }

            float sN = (float)health / (float)100;
            float yScale = Mathf.Lerp(0, fullHealthScale, sN);
            Vector3 scale = foreground.localScale;
            scale.y = yScale;
            foreground.localScale = scale;

        }
    }
}

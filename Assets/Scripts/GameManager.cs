using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public static bool started = false;

    public MeteorsManager meteorsManager;
    public DragController dc;

    public GameObject playerPrefab;
    public ShipController scPrefab;

    public Transform shipSpawn;
    public Transform playerSpawn;

    private GameObject playerObject;
    private ShipController shipObject;

    public TextMeshPro startText;
    public TextMeshPro timerText;
    public TextMeshPro endText;

    public int secondsWrangled = 0;
    
    private void Start() {
        instance = this;

        startText.gameObject.SetActive(true);
        StartCoroutine(startRoutine());
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.M)) {
            if (AudioListener.volume > 0) {
                AudioListener.volume = 0;
            } else {
                AudioListener.volume = 1;
            }
        }
    }

    IEnumerator startRoutine() {
        bool waiting = false;

        while (!waiting) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                waiting = true;
            }

            yield return null;
        }

        StartCoroutine(StartGame());
    }

    IEnumerator timerRoutine() {
        UpdateText();

        while (started) {
            yield return new WaitForSeconds(1);
            secondsWrangled += 1;
            UpdateText();
        }

    }

    void UpdateText() {
        timerText.text = secondsWrangled + " seconds";
    }

    public IEnumerator StartGame() {
        secondsWrangled = 0;

        endText.gameObject.SetActive(false);
        startText.gameObject.SetActive(false);

        playerObject = Instantiate(playerPrefab);
        playerObject.transform.position = playerSpawn.position;
        HingeJoint2D playerBody = playerObject.GetComponentInChildren<HingeJoint2D>();

        shipObject = (ShipController)Instantiate(scPrefab);
        shipObject.transform.position = shipSpawn.position;
        shipObject.tc.endJoint = playerBody;
        
        shipObject.tc.Generate();

        yield return StartCoroutine(shipObject.startRoutine());

        HealthBar.instance.Health = 100;
        HealthBar.instance.Show(true);

        started = true;
        StartCoroutine(timerRoutine());
        timerText.gameObject.SetActive(true);

        meteorsManager.StartSpawnWaves();
    }

    public void EndGame() {
        started = false;

        timerText.gameObject.SetActive(false);
        endText.gameObject.SetActive(true);
        endText.text = "You wrangled for " + secondsWrangled + " seconds. Press space to play again";

        HealthBar.instance.Show(false);

        meteorsManager.StopSpawnWaves();
        dc.Clean();
        shipObject.tc.Clean();

        Invoke("DelayedEnd", 5f);
    }

    public void DelayedEnd() {
        Destroy(playerObject);

        Destroy(shipObject.gameObject);

        StartCoroutine(startRoutine());
    }
}

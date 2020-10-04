using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorsManager : MonoBehaviour {

    public Transform[] spawnPoints;
    public Meteor[] meteorPrefabs;
    public Transform meteorsParent;

    public List<Meteor> meteors = new List<Meteor>();

    public WaitForSeconds spawnDelay = new WaitForSeconds(5);

    private Coroutine coroutine;

    public void StartSpawnWaves() {
        coroutine = StartCoroutine(wavesRoutine());
    }

    public void StopSpawnWaves() {
        StopCoroutine(coroutine);

        for (int i = 0; i < meteors.Count; i++) {
            meteors[i].Remove();
        }
    }

    IEnumerator wavesRoutine() {
        while (GameManager.started) {
            yield return spawnDelay;

            SpawnMeteor();
        }
    }

    public void SpawnMeteor() {
        Meteor m = (Meteor)Instantiate(meteorPrefabs[Random.Range(0, meteorPrefabs.Length)], meteorsParent);
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        m.transform.position = spawnPoint.position;
        m.SetDirection(spawnPoint.right);
        m.Init(this);

        meteors.Add(m);
    }
}

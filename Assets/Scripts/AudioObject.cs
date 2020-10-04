using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioObject : MonoBehaviour {

    public static void PlaySound(AudioClip clip, float volume, float pitch) {
        GameObject go = new GameObject("AudioObject: " + clip.name);
        go.SetActive(false);

        AudioSource source = go.AddComponent<AudioSource>();
        source.playOnAwake = false;
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;

        AudioObject ao = go.AddComponent<AudioObject>();
        go.SetActive(true);
        ao.Play();
    }

    public void Play() {
        StartCoroutine(playSound());
    }

    IEnumerator playSound() {
        AudioSource source = GetComponent<AudioSource>();

        if (source == null || source.clip == null) {
            Destroy(this.gameObject);
            yield break;
        }

        source.Play();

        yield return new WaitForSeconds(source.clip.length);

        Destroy(this.gameObject);
    }
}

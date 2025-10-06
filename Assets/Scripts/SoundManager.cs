using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    [SerializeField] private AudioClipRefsSO audioClipRefsSO;

    public void OnThrowBall() {
        PlaySound(audioClipRefsSO.onThrowBall, 0.3f);
    }

    public void OnJump() {
        PlaySound(audioClipRefsSO.onJump, 0.7f);
    }

    public void OnCollect() {
        PlaySound(audioClipRefsSO.onCollect, 0.7f);
    }

    public void OnCheckPoint() {
        PlaySound(audioClipRefsSO.onCheckPoint, 0.5f);
    }

    public void OnDead() {
        PlaySound(audioClipRefsSO.onDead, 0.15f);
    }

    public void OnBeetleMove() {
        PlaySound(audioClipRefsSO.onBeetleMove, 1f);
    }

    public void OnFlash() {
        PlaySound(audioClipRefsSO.onFlash, 0.5f);
    }

    public void PlayFootstepSound() {
        PlaySound(audioClipRefsSO.onWalk);
    }

    private void PlaySound(AudioClip[] audioClips, float volume = 1f) {
        PlaySound(audioClips[Random.Range(0, audioClips.Length)], volume);
    }

    private void PlaySound(AudioClip audioClip, float volume = 1f) {
        if (audioClip == null) return;

        Vector3 position = Camera.main.transform.position;
        GameObject soundGameObject = new GameObject("OneShotAudio");
        soundGameObject.transform.position = position;

        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.spatialBlend = 0;
        audioSource.panStereo = 0;
        audioSource.Play();

        Destroy(soundGameObject, audioClip.length);
    }
}

using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class TemporarySoundPlayer : MonoBehaviour
{
    private AudioSource audioSource;

    public string clipName {  get { return audioSource.clip.name; } }

    public void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play(AudioMixerGroup audioMixer, float delay, bool isLoop)
    {
        audioSource.outputAudioMixerGroup = audioMixer;
        audioSource.loop = isLoop;
        audioSource.Play();

        if (!isLoop)    // 루프 오디오는 파괴하지 않음
            StartCoroutine(DestroyWhenFinish(audioSource.clip.length));
    }

    public void InitSound(AudioClip clip)
    {
        audioSource.clip = clip;
    }

    private IEnumerator DestroyWhenFinish(float clipLength)    // 재생 후 파괴
    {
        yield return new WaitForSeconds(clipLength);

        Destroy(gameObject);
    }
}

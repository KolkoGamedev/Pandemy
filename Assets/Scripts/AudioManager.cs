using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip menuSoundtrack = null;
    [SerializeField] private FogManager fogManager = null;
    [SerializeField] private List<AudioClip> listOfTypingSounds = null;
    [SerializeField] private AudioSource typewriterSource = null;
    private AudioSource _as;

    private void Awake()
    {
        _as = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        TypeWriter.OnLetterWritten += PlayTypingSound;
        fogManager.OnFogFadeIn += StartPlayingSoundtrack;
    }

    private void OnDisable()
    {
        TypeWriter.OnLetterWritten -= PlayTypingSound;
    }

    private void StartPlayingSoundtrack()
    {
        _as.PlayOneShot(menuSoundtrack);
    }

    private void PlayTypingSound()
    {
        typewriterSource.PlayOneShot(listOfTypingSounds[Random.Range(0, listOfTypingSounds.Count)]);
    }
}

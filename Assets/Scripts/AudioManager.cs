using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private AudioClip menuSoundtrack = null;
    [SerializeField] private FogManager fogManager = null;
    [SerializeField] private List<AudioClip> listOfTypingSounds = null;
    //[SerializeField] private AudioSource typewriterSource = null;
    private AudioSource _as;

    private void Awake()
    {
        _as = GetComponent<AudioSource>();
        DontDestroyOnLoad(this.gameObject);
        Instance = this;
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
        _as.PlayOneShot(listOfTypingSounds[Random.Range(0, listOfTypingSounds.Count)]);
    }

    public void PlaySound(AudioClip clip)
    {
        _as.PlayOneShot(clip);
    }
}

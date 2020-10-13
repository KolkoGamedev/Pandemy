using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogSystem : MonoBehaviour
{
    [SerializeField] private TMP_Text narratorTextField = null;

    private FogManager _fogManager;
    private TypeWriter _tw;
    private void Awake()
    {
        _tw = FindObjectOfType<TypeWriter>();
    }

    private void Start()
    {
        _fogManager = FindObjectOfType < FogManager > ();
        _fogManager.OnFogFadeIn += StartGame;
    }

    public void NarratorSpeech(string sentence, float delay)
    {
        _tw.TypewriteSentence(sentence, narratorTextField, delay);
    }

    private void StartGame()
    {
        NarratorSpeech("You wake up from long rest", 1f);
    }
    
}

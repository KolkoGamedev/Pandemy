﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;

public class IntroManager : MonoBehaviour
{
    [SerializeField] private FogManager fogManager = null;
    [SerializeField] private TMP_Text welcomeText = null;
    [SerializeField] private List<Sentence> introDialogue = null;
    [SerializeField] private TMP_Text gameTitleText = null;
    [SerializeField] private GameObject menuHolder = null;
    private TypeWriter _typeWriter;
    
    private void Awake()
    {
        fogManager.OnFogFadeIn += StartSequence;
    }

    private void Start()
    {
        _typeWriter = TypeWriter.Instance;
    }

    private void StartSequence()
    {
        StartCoroutine(Sequence());
    }

    private IEnumerator Sequence()
    {
        for (int i = 0; i < introDialogue.Count; i++)
        {
            yield return new WaitUntil(() => _typeWriter.canWrite);
            _typeWriter.TypewriteSentence(introDialogue[i].text, welcomeText, introDialogue[i].delay);
        }
        yield return new WaitForSeconds(3);
        StartCoroutine(FadeOutText());
    }
    
    private IEnumerator FadeOutText()
    {
        float elapsedTime = 1f;
        float desiredTime = 0f;
        Color textColor = welcomeText.color;
        while (elapsedTime > desiredTime)
        {
            elapsedTime -= Time.deltaTime;
            welcomeText.color = new Color(textColor.r, textColor.b, textColor.b, elapsedTime);
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        ShowMenu();
    }

    private IEnumerator FadeInText(TMP_Text text)
    {
        float elapsedTime = 0f;
        float desiredTime = 1f;
        Color textColor = text.color;
        while (elapsedTime < desiredTime)
        {
            elapsedTime += Time.deltaTime;
            text.color = new Color(textColor.r, textColor.b, textColor.b, elapsedTime);
            yield return null;
        }
        yield return new WaitForSeconds(4f);
        menuHolder.gameObject.SetActive(true);
    }

    private void ShowMenu()
    {
        StartCoroutine(FadeInText(gameTitleText));
    }
}

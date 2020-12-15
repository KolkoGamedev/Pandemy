using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;

public class IntroManager : MonoBehaviour
{
    
    [SerializeField] private FogManager fogManager = null;
    [SerializeField] private TMP_Text welcomeText = null;
    [SerializeField] private List<Sentence> introDialogue = null;        // List of texts
    [SerializeField] private TMP_Text gameTitleText = null;
    [SerializeField] private GameObject menuHolder = null;
    [SerializeField] private float writeSpeed = .1f;
    private TypeWriter _typeWriter;
    private bool _isSkipped;
    private void Awake()
    {
        fogManager.OnFogFadeIn += StartSequence;
    }

    private void OnDisable()
    {
        fogManager.OnFogFadeIn -= StartSequence;
        
    }

    private void Start()
    {
        _typeWriter = TypeWriter.Instance;
    }

    private void StartSequence()
    {
        StartCoroutine(Sequence());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SkipIntro();
        }
        
    }

    private IEnumerator Sequence()                                     // Coroutine that shows every text line with delay from inspector
    {
        for (int i = 0; i < introDialogue.Count; i++)
        {
            if (_isSkipped)
                break;
                
            yield return new WaitUntil(() => _typeWriter.canWrite || _isSkipped);
            _typeWriter.TypewriteSentence(new DialogScene(introDialogue[i].text, welcomeText, introDialogue[i].delay), writeSpeed);
        }
        yield return new WaitForSeconds(3);                            // Wait 3 seconds after last text to fade out
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
        yield return new WaitForSeconds(1f);                        // Wait 1 second after fadeout to start showing menu
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

        if (!_isSkipped)
        {
            yield return new WaitForSeconds(4f);
            menuHolder.gameObject.SetActive(true);
        }
        else
            menuHolder.gameObject.SetActive(true);
    }

    private void ShowMenu()
    {
        StartCoroutine(FadeInText(gameTitleText));
    }

    private void SkipIntro()
    {
        _isSkipped = true;
        StopAllCoroutines();
        StartCoroutine(FadeOutText());
    }
}

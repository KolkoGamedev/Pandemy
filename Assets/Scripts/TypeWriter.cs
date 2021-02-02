using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

[Serializable]
public struct Sentence
{
    public string text;
    public float delay;
}
public class TypeWriter : MonoBehaviour
{
    [SerializeField] private float writeSpeed = 0.1f;
    public static event Action OnLetterWritten = delegate {  };
    public static TypeWriter Instance;
    [SerializeField] private TMP_Text dialogTextField = null;
    public bool canWrite = true;
    public float pauseTimeAtDot = 3f;
    public float longPause = 3f;
    private bool isDialogSkipped = false;
    private string currentSentence = "";
    private string goalSentence = "";
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        DialogSystem.OnDialogSkipped += DialogSystemOnDialogSkipped;
    }

    private void DialogSystemOnDialogSkipped(DialogScene dialogScene)
    {
        if (!dialogScene.wasSkipped && !dialogScene.nextSceneId.StartsWith("x") && !dialogScene.sceneId.StartsWith("x") && !dialogScene.sceneId.StartsWith("m"))
        {
            
            StopAllCoroutines();
            PostWrittenEvents.Instance.StopAllCoroutines();

            if (dialogScene.textAuthor == "")
            {
                dialogScene.text = dialogScene.text.Replace('^', ' ');
                dialogScene.text = dialogScene.text.Replace('&', ' ');
                dialogScene.currentTextField.text = dialogScene.text;
            }
            else
            {
                goalSentence = goalSentence.Replace('^', ' ');
                dialogScene.text = goalSentence.Replace('&', ' ');
                string completedSentence = goalSentence.Substring(currentSentence.Length, goalSentence.Length-currentSentence.Length);
                dialogScene.currentTextField.text += completedSentence;
                dialogScene.currentTextField.text += dialogScene.msgAfterScene;
            }
            dialogScene.OnSceneFinished?.Invoke();
            dialogScene.wasSkipped = true;
            StartCoroutine(WaitForTimeAfterSkip(dialogScene));
        }
    }

    public IEnumerator WaitForTimeAfterSkip(DialogScene dialogScene)
    {
        yield return new WaitForSeconds(dialogScene.afterTextDelay);
        canWrite = true;
    }
    public void TypewriteSentence(DialogScene currentScene)
    {
        StartCoroutine(Typewrite(currentScene));
    }
    public void TypewriteSentence(DialogScene currentScene, string author)
    {
        StartCoroutine(Typewrite(currentScene, author));
    }
    public void TypewriteSentence(DialogScene currentScene, float writeSpeed)
    {
        StartCoroutine(Typewrite(currentScene, writeSpeed));
        
    }
    // Narrator
    private IEnumerator Typewrite(DialogScene currentScene)
    {
        string currentWord = "";
        int index = 0;
        
        while (currentWord.Length < currentScene.text.Length)
        {
            canWrite = false;
            
            if (currentScene.overrideDialog)
                dialogTextField.text = "";
            
            if(currentScene.text[index] != '^' && currentScene.text[index] != '&')
                currentWord += currentScene.text[index];
            else if(currentScene.text[index] == '^')
            {
                currentWord += ' ';
                yield return new WaitForSeconds(longPause);
            }
            else if (currentScene.text[index] == '&')
            {
                currentWord += ' ';
                AudioManager.Instance.PlaySound(currentScene.soundEvent);
            }
            currentScene.currentTextField.text = currentWord;
            OnLetterWritten?.Invoke();

            if (index > 1 && currentScene.text[index] == ' ' && currentScene.text[index-1] == '.')
                yield return new WaitForSeconds(writeSpeed * pauseTimeAtDot);
            else
                yield return new WaitForSeconds(writeSpeed);
            index++;
        }
        yield return new WaitForSeconds(currentScene.afterTextDelay);
        canWrite = true;
        currentScene.OnSceneFinished?.Invoke();
    }
    //Intro
    private IEnumerator Typewrite(DialogScene currentScene, float writeSpeed)
    {
        string currentWord = "";
        int index = 0;
        
        while (currentWord.Length < currentScene.text.Length)
        {
            canWrite = false;
            currentWord += currentScene.text[index];
            currentScene.currentTextField.text = currentWord;
            OnLetterWritten?.Invoke();
            
            if (index > 1 && currentScene.text[index] == ' ' && currentScene.text[index-1] == '.')
                yield return new WaitForSeconds(writeSpeed * pauseTimeAtDot);
            else
                yield return new WaitForSeconds(writeSpeed);
            index++;
        }
        yield return new WaitForSeconds(currentScene.afterTextDelay);
        canWrite = true;
    }
    //Dialogi
    private IEnumerator Typewrite(DialogScene currentScene, string author)
    {
        char sign = ' ';
        int index = 0;
        goalSentence = currentScene.text;
        currentSentence = "";

        if (currentScene.overrideDialog)
            currentScene.currentTextField.text = "";
        
        if(author[0] != '0')
            currentScene.currentTextField.text += "<br><br>"+author+"<br><br>";
        
        while (currentSentence.Length < currentScene.text.Length)
        {
            canWrite = false;
            
            if(currentScene.text[index] == '^')
            {
                currentSentence += ' ';
                sign = ' ';
                yield return new WaitForSeconds(longPause);
            }
            else if (currentScene.text[index] == '&')
            {
                currentSentence += ' ';
                sign = ' ';
                AudioManager.Instance.PlaySound(currentScene.soundEvent);
            }
            else
            {
                currentSentence += currentScene.text[index];
                sign = currentScene.text[index];
            }
            currentScene.currentTextField.text += sign;
            OnLetterWritten?.Invoke();
            
            if (index > 1 && currentScene.text[index] == ' ' && currentScene.text[index-1] == '.')
                yield return new WaitForSeconds(writeSpeed * pauseTimeAtDot);
            else
                yield return new WaitForSeconds(writeSpeed);
            index++;
        }

        currentScene.currentTextField.text += "<br><br>"+currentScene.msgAfterScene+"<br>";
        yield return new WaitForSeconds(currentScene.afterTextDelay);
        canWrite = true;
       
    }
}

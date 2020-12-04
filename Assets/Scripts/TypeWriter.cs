using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
    //public static event Action<TMP_Text> OnSceneFinishedWritting = delegate {  };
    public static TypeWriter Instance;
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
        DialogSystem.OnDialogSkipped += DialogSystemOnOnDialogSkipped;
    }

    private void DialogSystemOnOnDialogSkipped(DialogScene dialogScene)
    {
        if (!dialogScene.wasSkipped)
        {
            StopAllCoroutines();
            
            if (dialogScene.textAuthor == "")
            {
                dialogScene.text = dialogScene.text.Replace('^', ' ');
                dialogScene.currentTextField.text = dialogScene.text;
            }
            else
            {
                goalSentence = goalSentence.Replace('^', ' ');
                string completedSentence = goalSentence.Substring(currentSentence.Length, goalSentence.Length-currentSentence.Length);
                dialogScene.currentTextField.text += completedSentence;
            }
            dialogScene.wasSkipped = true;
            canWrite = true;
        }
    }

    public void TypewriteSentence(string sentence, TMP_Text textField, float delay)
    {
        StartCoroutine(Typewrite(sentence, textField, delay));
    }
    public void TypewriteSentence(string sentence, string author, TMP_Text textField, float delay)
    {
        StartCoroutine(Typewrite(sentence, author, textField, delay));
    }
    public void TypewriteSentence(string sentence, TMP_Text textField, float delay, float writeSpeed)
    {
        StartCoroutine(Typewrite(sentence, textField, delay, writeSpeed));
        
    }
    // Narrator
    private IEnumerator Typewrite(string sentence, TMP_Text textField, float delay)
    {
        string currentWord = "";
        int index = 0;
        
        while (currentWord.Length < sentence.Length)
        {
            canWrite = false;
            if(sentence[index] != '^')
                currentWord += sentence[index];
            else
            {
                currentWord += ' ';
                yield return new WaitForSeconds(longPause);
            }
            textField.text = currentWord;
            OnLetterWritten?.Invoke();
            
            if (index > 1 && sentence[index] == ' ' && sentence[index-1] == '.')
                yield return new WaitForSeconds(writeSpeed * pauseTimeAtDot);
            else
                yield return new WaitForSeconds(writeSpeed);
            index++;
        }
        yield return new WaitForSeconds(delay);
        canWrite = true;
        OnSceneFinishedWritting?.Invoke(textField);
    }
    //Intro
    private IEnumerator Typewrite(string sentence, TMP_Text textField, float delay, float writeSpeed)
    {
        string currentWord = "";
        int index = 0;
        
        while (currentWord.Length < sentence.Length)
        {
            canWrite = false;
            currentWord += sentence[index];
            textField.text = currentWord;
            OnLetterWritten?.Invoke();
            
            if (index > 1 && sentence[index] == ' ' && sentence[index-1] == '.')
                yield return new WaitForSeconds(writeSpeed * pauseTimeAtDot);
            else
                yield return new WaitForSeconds(writeSpeed);
            index++;
        }
        yield return new WaitForSeconds(delay);
        canWrite = true;
    }
    //Dialogi
    private IEnumerator Typewrite(string sentence,string author, TMP_Text textField, float delay)
    {
        char sign = ' ';
        int index = 0;
        goalSentence = sentence;
        currentSentence = "";
        
        textField.text += "<br><br>"+author+"<br><br>";
        while (currentSentence.Length < sentence.Length)
        {
            canWrite = false;

            if (sentence[index] != '^')
            {
                currentSentence += sentence[index];
                sign = sentence[index];
            }
            else
            {
                currentSentence += ' ';
                sign = ' ';
                yield return new WaitForSeconds(longPause);
            }
            textField.text += sign;
            OnLetterWritten?.Invoke();
            
            if (index > 1 && sentence[index] == ' ' && sentence[index-1] == '.')
                yield return new WaitForSeconds(writeSpeed * pauseTimeAtDot);
            else
                yield return new WaitForSeconds(writeSpeed);
            index++;
        }
        yield return new WaitForSeconds(delay);
        canWrite = true;
        OnSceneFinishedWritting?.Invoke(textField);
    }
}

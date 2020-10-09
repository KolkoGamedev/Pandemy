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
    public static TypeWriter Instance;
    public bool canWrite = true;
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
    }

    public void TypewriteSentence(string sentence, TMP_Text textField, float delay)
    {
        StartCoroutine(Typewrite(sentence, textField, delay));
    }

    private IEnumerator Typewrite(string sentence, TMP_Text textField, float delay)
    {
        string currentWord = "";
        int index = 0;
        while (currentWord.Length < sentence.Length)
        {
            canWrite = false;
            currentWord += sentence[index];
            textField.text = currentWord;
            OnLetterWritten?.Invoke();
            index++;
            yield return new WaitForSeconds(writeSpeed);
        }
        yield return new WaitForSeconds(delay);
        canWrite = true;
    }
}

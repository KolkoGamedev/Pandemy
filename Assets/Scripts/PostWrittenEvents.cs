using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PostWrittenEvents : MonoBehaviour
{
    private void Awake()
    {
        //TypeWriter.OnSceneFinishedWritting += FadeOutTextField;
    }

    public void FadeOutTextField(TMP_Text textField)
    {
        StartCoroutine(FadeOutText(textField));
    }
    private IEnumerator FadeOutText(TMP_Text textField)
    {
        float elapsedTime = 4f;
        float desiredTime = 0f;
        Color textColor = textField.color;
        while (elapsedTime > desiredTime)
        {
            elapsedTime -= Time.deltaTime;
            textField.color = new Color(textColor.r, textColor.b, textColor.b, elapsedTime);
            yield return null;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PostWrittenEvents : MonoBehaviour
{
    public static PostWrittenEvents Instance;
    [SerializeField] private GameObject tv = null;
    [SerializeField] private List<RectTransform> textFields = null;
    
    private void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(gameObject);
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

    public void SwitchTv()
    {
        if (tv.activeInHierarchy)
        {
            tv.SetActive(false);
            for (int i = 0; i < textFields.Count; i++)
            {
                textFields[i].localPosition = new Vector3(0, textFields[i].localPosition.y, textFields[i].localPosition.z);
            }
        }
        else
        {
            tv.SetActive(true);
            for (int i = 0; i < textFields.Count; i++)
            {
                textFields[i].localPosition = new Vector3(-156, textFields[i].localPosition.y, textFields[i].localPosition.z);
            }
        }
    }

    public void ClearSaves()
    {
        PlayerPrefs.DeleteAll();
    }
    
    public void TurnOffGame()
    {
        Application.Quit();
    }
}

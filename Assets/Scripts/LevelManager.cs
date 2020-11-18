using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    [SerializeField] private List<GameObject> texts = null;            // Textfields to hide before level swap
    private FogManager _fm;
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

    private void Start()
    {
        _fm = FindObjectOfType<FogManager>();
    }

    public void HideTextsAndStartLevel(string sceneName)
    {
        for(int i = 0; i < texts.Count; i++)
            texts[i].SetActive(false);

        StartCoroutine(_fm.FadeOut(callback =>
        {
            if(callback.Equals(true))
                LoadLevel(sceneName);
        }));
    }
    
    public void LoadLevel(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}

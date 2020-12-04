using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static event Action GameStarted = delegate {  };
    public static LevelManager Instance;
    
    [SerializeField] private GameObject menuMap = null;        
    [SerializeField] private GameObject gameMap = null;         
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

    public void SwapMap()
    {
        menuMap.SetActive(false);
        gameMap.SetActive(true);
        GameStarted?.Invoke();
        /*
        StartCoroutine(_fm.FadeOut(callback =>
        {
            if(callback.Equals(true))
                LoadLevel(sceneName);
        }));
        */
    }
}

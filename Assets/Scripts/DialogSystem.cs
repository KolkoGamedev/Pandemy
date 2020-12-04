using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    [Header("TEXT FIELDS")]
    [SerializeField] private TMP_Text narratorTextField = null;
    [SerializeField] private TMP_Text dialogTextField = null;

    [Header("CHOICE TYPES")] 
    [SerializeField] private GameObject threeChoiceSetup = null;
    [SerializeField] private GameObject twoChoiceSetup = null;
    [SerializeField] private bool is3Choice = false;
    
    [Header("DIALOG")]
    [SerializeField] private List<DialogScene> scenariusz = new List<DialogScene>();
    public static event Action<DialogScene> OnDialogSkipped = delegate {  };
    private FogManager _fogManager;
    private TypeWriter _tw;
    private string _sceneId = "0_0";
    private DialogScene currentScene;
    
    private void Awake()
    {
        _tw = FindObjectOfType<TypeWriter>();
        LevelManager.GameStarted += StartGame;
    }

    public DialogScene FindSceneById(string id)
    {
        for (int i = 0; i < scenariusz.Count; i++)
        {
            if (scenariusz[i].sceneId == id)
            {
                return scenariusz[i];
            }
        }
        return null;
    }

    private void StartGame()
    {
        if (scenariusz.Count > 0)
        {
            StartCoroutine(StartDialog());
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnDialogSkipped?.Invoke(currentScene);
        }
    }

    private IEnumerator StartDialog()
    {
        while (_sceneId != "end")
        {
            if (_sceneId[0] == 'x')
            {
                // ODWOLANIE DO SKRYPTU Z WYBORAMI
                // KORUTYNA KTORA PO UKOCZENIU ZWRACA SCENE ID
                // _sceneId = korutyna();
            }
            yield return new WaitUntil(() => _tw.canWrite);
            currentScene = FindSceneById(_sceneId);
            if (currentScene.textAuthor != "")
            {
                _tw.TypewriteSentence(currentScene.text, currentScene.textAuthor, currentScene.currentTextField, currentScene.afterTextDelay);
            }
            else
            {
                currentScene.currentTextField.color = new Color(currentScene.currentTextField.color.r, currentScene.currentTextField.color.g, currentScene.currentTextField.color.b, 1);
                _tw.TypewriteSentence(currentScene.text, currentScene.currentTextField, currentScene.afterTextDelay);
            }
            _sceneId = currentScene.nextSceneId;
        }
    }
}

[Serializable]
public class DialogScene
{
    public string sceneId;
    public TMP_Text currentTextField;
    public string textAuthor;
    [TextArea(5, 10)] public string text;
    public float afterTextDelay;
    public string nextSceneId;
    public bool wasSkipped;
    //public UnityEvent OnSceneFinished;
}

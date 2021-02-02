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
    
    [SerializeField] private DialogChoices dialogChoices = null;
    [SerializeField] private MinigamesManager _minigamesManager = null;
    [SerializeField] private PostWrittenEvents _events = null;
    [Header("DIALOG")]
    [SerializeField] private List<DialogScene> scenariusz = new List<DialogScene>();


    public static event Action<DialogScene> OnDialogSkipped = delegate {  };
    private FogManager _fogManager;
    private TypeWriter _tw;
    private string _sceneId = "0_70";
    private DialogScene currentScene;
    private bool gameIsLoaded = true;
    private void Awake()
    {
        _tw = FindObjectOfType<TypeWriter>();
        LevelManager.GameStarted += StartGame;

    }

    private void Start()
    {
        if (gameIsLoaded)
        {
            _events.SwitchTv();
            
        }
            
    }

    public DialogScene FindSceneById(string id)
    {
        /*
        if (PlayerPrefs.HasKey("Saved"))
        {
            id = PlayerPrefs.GetString("Saved");
        }
        else
        {
            PlayerPrefs.SetString("Saved", id);
        }*/
        
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
            yield return new WaitUntil(() => _tw.canWrite);
            currentScene = FindSceneById(_sceneId);
            if (_sceneId[0] == 'm')
            {
                _minigamesManager.SetupMinigame(currentScene);

                yield return new WaitUntil(() => _minigamesManager.minigameFinished);
                Debug.Log("Minigame Finished");
                currentScene = FindSceneById(currentScene.nextSceneId);
            }
            
            
            
            if (currentScene.textAuthor != "" && currentScene.text != "")
            {
                _tw.TypewriteSentence(currentScene, currentScene.textAuthor);
            }
            else if(currentScene.textAuthor == "" && currentScene.text != "")
            {
                currentScene.currentTextField.color = new Color(currentScene.currentTextField.color.r, currentScene.currentTextField.color.g, currentScene.currentTextField.color.b, 1);
                _tw.TypewriteSentence(currentScene);
            }
            if (_sceneId[0] == 'x')
            {
                dialogChoices.SetupChoices(_sceneId);

                yield return new WaitUntil(() => dialogChoices.currentChoice.choiceMade);

                currentScene.nextSceneId = dialogChoices.currentChoice.wybor.destinationId;
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
    [TextArea(5, 15)] public string text;
    public float afterTextDelay;
    public string nextSceneId;
    [HideInInspector] public bool wasSkipped;
    public bool overrideDialog;
    public string msgAfterScene;
    public UnityEvent OnSceneFinished;
    public AudioClip soundEvent;
    public GameObject minigame;

    public DialogScene(string text, TMP_Text textField, float delay)
    {
        this.text = text;
        this.currentTextField = textField;
        this.afterTextDelay = delay;
    }
}

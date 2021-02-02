using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogChoices : MonoBehaviour
{
    [Header("Wybory")]
    [SerializeField] private List<Wybory> wybory = new List<Wybory>();

    [SerializeField] private GameObject setup3 = null;
    [SerializeField] private GameObject setup2 = null;
    
    [HideInInspector]
    public Wybory currentChoice;

    
    
    public void SetupChoices(string sceneId)
    {
        currentChoice = FindWyborById(sceneId);
        currentChoice.choiceMade = false;
        currentChoice.wybor.destinationId = "";
        
        if (currentChoice.is3Choice)
        {
            setup3.SetActive(true);
            setup2.SetActive(false);
        }
        else
        {
            setup2.SetActive(true);
            setup3.SetActive(false);
        }

        for (int i = 0; i < currentChoice.wybor.choicesFields.Count; i++)
        {
            currentChoice.wybor.choicesFields[i].text = currentChoice.wybor.choices[i];
            var i1 = i;
            currentChoice.wybor.choicesFields[i].GetComponent<Button>().onClick.AddListener(() => OnButtonClick(currentChoice.wybor.destination[i1]));
        }
            
    }

    private void OnButtonClick(string destinationId)
    {
        currentChoice.choiceMade = true;
        currentChoice.wybor.destinationId = destinationId;
        if (currentChoice.is3Choice)
        {
            setup3.SetActive(false);
        }
        else
            setup2.SetActive(false);
    }
    
    public Wybory FindWyborById(string id)
    {
        for (int i = 0; i < wybory.Count; i++)
        {
            if (wybory[i].sceneId == id)
            {
                return wybory[i];
            }
        }
        return null;
    }
}

[Serializable]
public class Wybory
{
    public string sceneId;
    public Wybor wybor;
    public bool is3Choice;
    public bool choiceMade;
}

[Serializable]
public class Wybor
{
    public List<TMP_Text> choicesFields;
    public List<string> choices;
    public List<string> destination;
    public string destinationId;
}


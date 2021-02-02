using System;
using System.Collections;
using System.Collections.Generic;
using Data.Util;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Bomba : MiniGame
{
    [SerializeField] private TMP_Text TextField=null;
    string TextString = "";

    public void ButtonPressed()
    {
        if(TextString.Length < 4)
        {
            string buttonValue = EventSystem.current.currentSelectedGameObject.name;
            TextString += buttonValue;
            TextField.text = TextString;
        }
    }

    public void Reset()
    {
        TextField.text = null;
        TextString = "";
    }

    public void Arm()
    {
        if(TextString=="9562")
        {
            base.FinishGame();
        }
    }
    private void Awake()
    {

    }
}

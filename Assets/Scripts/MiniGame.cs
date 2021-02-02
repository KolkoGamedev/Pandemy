using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGame : MonoBehaviour
{
    public event Action OnMinigameFinished = delegate { };
    public virtual void FinishGame()
    {
        OnMinigameFinished.Invoke();
        gameObject.SetActive(false);
    }
}

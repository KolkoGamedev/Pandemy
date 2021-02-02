using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigamesManager : MonoBehaviour
{
    public bool minigameFinished = false;

    private MiniGame _miniGame;
    public void SetupMinigame(DialogScene scene)
    {
        minigameFinished = false;
        scene.minigame.SetActive(true);
        _miniGame = scene.minigame.GetComponent<MiniGame>();
        _miniGame.OnMinigameFinished += () => minigameFinished = true;

    }
}

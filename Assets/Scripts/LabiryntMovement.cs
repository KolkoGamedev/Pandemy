using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabiryntMovement : MiniGame
{
    [SerializeField] private Canvas gameCanvas;
    [SerializeField] private GameObject spawnPoint;
    Vector2 pos;
    bool isClicked = false;

    private void Awake()
    {
 
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name.Equals("EndPoint"))
        {
            isClicked = false;
            base.FinishGame();
        }
        else
        {
            isClicked = false;
            transform.position = spawnPoint.transform.position;
        }
    }
    void OnMouseDown()
    {
        isClicked = true;
    }
    private void Update()
    {
        if(isClicked)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(gameCanvas.transform as RectTransform, Input.mousePosition, gameCanvas.worldCamera, out pos);
            transform.position = gameCanvas.transform.TransformPoint(pos);
        }    
    }
}

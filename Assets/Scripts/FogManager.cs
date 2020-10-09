using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class FogManager : MonoBehaviour
{
    public event Action OnFogFadeIn = delegate {  };
    [SerializeField] private SpriteRenderer fogRenderer = null;
    private Material _fogMaterial;
    
   
    void Awake()
    {
        _fogMaterial = fogRenderer.material;
    }

    private void Start()
    {
        FadeInFog();
    }

    public void FadeInFog()
    {
        StartCoroutine(FadeIn());
    }

    public IEnumerator FadeIn()
    {
        float expectedTime = 3f;
        float elapsedTime = 0f;
        
        while (elapsedTime < expectedTime)
        {
            elapsedTime += Time.deltaTime;
            _fogMaterial.SetFloat("_FogAlpha", elapsedTime/expectedTime);
            yield return null;
        }
        OnFogFadeIn?.Invoke();
    }
    public IEnumerator FadeOut(Action<bool> callback)
    {
        float expectedTime = 3f;
        float elapsedTime = 0f;
        
        while (elapsedTime < expectedTime)
        {
            elapsedTime += Time.deltaTime;
            _fogMaterial.SetFloat("_FogAlpha", 1 - (elapsedTime /expectedTime));
            yield return null;
        }
        callback(true);
    }
}

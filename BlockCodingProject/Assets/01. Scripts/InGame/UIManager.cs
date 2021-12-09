using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject clickBlockPanel;

    public Slider zoomSlider;
    public Transform blockPanel;

    public GameObject wallBlockEffect;
    public RectTransform screenFade;

    private const float zoomScale = 0.5f;

    public static float currentZoomValue = 1f;

    private void Awake()
    {
        if(!Instance)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        currentZoomValue = 0.6f + zoomSlider.value * zoomScale;
        blockPanel.transform.localScale = new Vector3(currentZoomValue, currentZoomValue, currentZoomValue);

        zoomSlider.onValueChanged.AddListener(x => {
            currentZoomValue = 0.6f + zoomSlider.value * zoomScale;
            blockPanel.transform.localScale = new Vector3(currentZoomValue, currentZoomValue, currentZoomValue);
        });

    }

    public static void ClickBlock(bool value)
    {
        Instance.clickBlockPanel.SetActive(value);
    }

    public static void ResetFadeInOut(bool value, Action action)
    {
        ClickBlock(true);
        Instance.screenFade.gameObject.SetActive(true);

        if (value) // ÀÎ
        {
            DOTween.To(() => Instance.screenFade.offsetMin, value => Instance.screenFade.offsetMin = value, new Vector2(1920, 0), 1).OnComplete(() => {
                ClickBlock(false);
                action();
            });
        }
        else
        {
            Instance.screenFade.offsetMin = new Vector2(0, 0);
            Instance.screenFade.offsetMax = new Vector2(-1920, 0);
            DOTween.To(() => Instance.screenFade.offsetMax, value => Instance.screenFade.offsetMax = value, new Vector2(0, 0), 1).OnComplete(() => action());
        }
    }
}

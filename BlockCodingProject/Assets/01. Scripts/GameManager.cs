using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<LineGenerate> lineCircles = new List<LineGenerate>();
    public List<LineRenderer> enabledLines = new List<LineRenderer>();

    public RectTransform codingPanel;
    public RectTransform ingameCanvas;

    public static float canvasScaleValue;

    public static float CanvasScale { get { return  canvasScaleValue / UIManager.currentZoomValue; } }

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }

        canvasScaleValue = 1 / ingameCanvas.transform.localScale.x;
    }

    private bool toggle = false;
    private void Update()
    {
#if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.Space))
        {
            toggle = !toggle;
            MovingPanel(toggle);
        }
#endif
    }

    public void MovingPanel(bool moveToCoding)
    {
        if (moveToCoding)
        {
            DOTween.To(() => codingPanel.offsetMin, value => codingPanel.offsetMin = value, new Vector2(0, 0), 1);
        }
        else
        {
            DOTween.To(() => codingPanel.offsetMin, value => codingPanel.offsetMin = value, new Vector2(1920, 0), 1);
        }
    }

    public static Vector3 ScreenToWorldPoint()
    {
        Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return new Vector3(point.x, point.y, 0) * CanvasScale;
    }
}

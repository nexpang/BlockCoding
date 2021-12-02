using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum GameStatus
{
    INGAME,
    CODING
}


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameStatus gameStatus;

    public List<LineGenerate> lineCircles = new List<LineGenerate>();
    public LineRenderer[] enabledLines;

    public RectTransform codingPanel;
    public RectTransform ingameCanvas;

    public static float canvasScaleValue;
    private bool isMovingTab = false;

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
        if (isMovingTab) return;

        isMovingTab = true;

        if (moveToCoding)
        {
            gameStatus = GameStatus.CODING;
            UIManager.ClickBlock(true);
            DOTween.To(() => codingPanel.offsetMin, value => codingPanel.offsetMin = value, new Vector2(0, 0), 1).OnComplete(() =>
            {
                foreach (LineRenderer item in enabledLines)
                {
                    item.gameObject.SetActive(true);
                }
                enabledLines = null;
                isMovingTab = false;
                UIManager.ClickBlock(false);
            });
        }
        else
        {
            gameStatus = GameStatus.INGAME;
            enabledLines = FindObjectsOfType<LineRenderer>();
            foreach (LineRenderer item in enabledLines)
            {
                item.gameObject.SetActive(false);
            }
            DOTween.To(() => codingPanel.offsetMin, value => codingPanel.offsetMin = value, new Vector2(1920, 0), 1).OnComplete(() =>
            {
                isMovingTab = false;
            });
        }
    }

    public static Vector3 ScreenToWorldPoint(bool isCanvasScale = true)
    {
        Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return new Vector3(point.x, point.y, 0) * (isCanvasScale ? CanvasScale : 1);
    }


}

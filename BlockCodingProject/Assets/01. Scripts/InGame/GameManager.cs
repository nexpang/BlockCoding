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

    private bool isMovingTab = false;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }

        CanvasSync.ScaleEdit(ingameCanvas);
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
        return new Vector3(point.x, point.y, 0) * (isCanvasScale ? CanvasSync.CanvasScale : 1);
    }


}

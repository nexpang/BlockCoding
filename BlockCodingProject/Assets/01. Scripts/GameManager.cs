using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<LineGenerate> lineCircles = new List<LineGenerate>();

    public RectTransform codingPanel;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
    }

    private void Update()
    {
#if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.Space))
        {
            MovingPanel();
        }
#endif
    }
    public void MovingPanel()
    {
        codingPanel.offsetMin = new Vector2(1920, 0);
    }

    public static Vector3 ScreenToWorldPoint()
    {
        Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return new Vector3(point.x, point.y, 0);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<LineGenerate> lineCircles = new List<LineGenerate>();

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
    }

    public static Vector3 ScreenToWorldPoint()
    {
        Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return new Vector3(point.x, point.y, 5);
    }
}

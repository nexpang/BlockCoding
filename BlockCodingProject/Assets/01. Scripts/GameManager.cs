using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<LineGenerate> lineCircles = new List<LineGenerate>();

    public GameObject codingCMCam;
    public GameObject ingameCMCam;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        codingCMCam.SetActive(true);
        ingameCMCam.SetActive(false);
    }

    private void Update()
    {

    }
    public void MovingPanel()
    {
        codingCMCam.SetActive(!codingCMCam.activeSelf);
        ingameCMCam.SetActive(!ingameCMCam.activeSelf);
    }

    public static Vector3 ScreenToWorldPoint()
    {
        Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return new Vector3(point.x, point.y, 0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TitleManager : MonoBehaviour
{
    public static TitleManager Instance;

    public RectTransform titleCanvas;
    public CanvasGroup startPanel;

    public LineRenderer leftLine;
    public LineRenderer rightLine;

    public GameObject logoText;

    private void Awake()
    {
        if(!Instance)
        {
            Instance = this;
        }
        logoText.SetActive(false);
        startPanel.alpha = 1;
        startPanel.blocksRaycasts = true;
        startPanel.interactable = true;
    }

    void Start()
    {
        CanvasSync.ScaleEdit(titleCanvas);
    }

    public void MainPanelStart()
    {
        startPanel.alpha = 0;
        startPanel.blocksRaycasts = false;
        startPanel.interactable = false;

        leftLine.startColor = Color.black;
        leftLine.endColor = Color.black;
        rightLine.startColor = Color.black;
        rightLine.endColor = Color.black;

        logoText.SetActive(true);
    }
}

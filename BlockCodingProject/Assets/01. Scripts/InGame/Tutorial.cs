using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Tutorial : MonoBehaviour
{
    public GameObject[] fades;
    public Button fadeClickPanel;

    private int index = 0;
    private Queue<GameObject> fadeQueue = new Queue<GameObject>();
    private CanvasGroup canvasGroup;

    public Text clickToNextTxt;

    private void Awake()
    {
        if(GameManager.currentStageIndex != 0)
        {
            gameObject.SetActive(false);
        }

        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        fadeClickPanel.onClick.AddListener(() =>
        {
            FadeDequeue();
        });

        clickToNextTxt.DOFade(0, 1).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);

        if (GameManager.currentStageIndex == 0)
        {
            FadeShow(new int[3] { 0, 1, 2 });
        }
    }

    public void FadeShow(int[] index)
    {
        for (int i = 0; i < index.Length; i++)
        {
            fadeQueue.Enqueue(fades[index[i]]);
        }

        FadeDequeue();
    }

    private void FadeDequeue()
    {
        if (fadeQueue.Count > 0)
        {
            PanelActive(true);
            GameObject fade = fadeQueue.Dequeue();

            for (int i = 0; i < fades.Length; i++)
            {
                fades[i].SetActive(false);
            }

            fade.SetActive(true);
        }
        else
        {
            for (int i = 0; i < fades.Length; i++)
            {
                fades[i].SetActive(false);
            }
            PanelActive(false);
        }
    }

    private void PanelActive(bool value)
    {
        canvasGroup.alpha = value ? 1 : 0;
        canvasGroup.blocksRaycasts = value;
        canvasGroup.interactable = value;
    }
}

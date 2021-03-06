using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.EventSystems;

public enum GameStatus
{
    INGAME,
    CODING
}

[Serializable]
public struct StageBox
{
    public GameObject ingameStage;
    public GameObject codingStage;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameStatus gameStatus;
    
    [HideInInspector]
    public List<LineGenerate> lineCircles = new List<LineGenerate>();
    [HideInInspector]
    public LineRenderer[] enabledLines;

    public RectTransform codingPanel;
    public RectTransform ingameCanvas;

    public StageBox[] stages;

    [Header("Clear")]
    public RectTransform clearPanel;
    public ParticleSystem clearParticle;

    [Tooltip("-1이면 기본값, 다른 값으면 그 값으로 바꾼다.")]
    public int DEBUG_currentIndex = -1; // 디버그용
    public static int currentStageIndex = 0;
    public static bool skipTitleScene = false;

    private bool isMovingTab = false;
    public static int clearTime;

    [Header("Tutorial")]
    public Tutorial tutorial;
    private bool tuto_isTrigger = false;

    public bool isClear { get; private set; } = false;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }

        if (DEBUG_currentIndex != -1) // 디버그
        {
            currentStageIndex = DEBUG_currentIndex;
        }

        CanvasSync.ScaleEdit(ingameCanvas);

        for (int i = 0; i<stages.Length;i++)
        {
            stages[i].codingStage.SetActive(false);
            stages[i].ingameStage.SetActive(false);
        }

        stages[currentStageIndex].ingameStage.SetActive(true);
        stages[currentStageIndex].codingStage.SetActive(true);

        if (gameStatus == GameStatus.INGAME)
        {
            StartCoroutine(LateStart());
        }
    }

    private void Start()
    {
        UIManager.ResetFadeInOut(true, () => { });
        StartCoroutine(Timer());
        PlaySound.PlayBGM(PlaySound.audioBox.BGM_inGame);
    }

    IEnumerator LateStart()
    {
        yield return null;
        yield return null;
        yield return null;
        enabledLines = FindObjectsOfType<LineRenderer>();
        foreach (LineRenderer item in enabledLines)
        {
            item.gameObject.SetActive(false);
        }
    }

    IEnumerator Timer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            clearTime++;
        }
    }

    public void MovingPanel(bool moveToCoding)
    {
        if (isMovingTab) return;

        isMovingTab = true;

        if (moveToCoding)
        {
            gameStatus = GameStatus.CODING;
            if(!tuto_isTrigger)
            {
                if(currentStageIndex == 0)
                {
                    tuto_isTrigger = true;
                    tutorial.FadeShow(new int[2] { 3, 4 });
                }
            }
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
            UIManager.ClickBlock(true);
            enabledLines = FindObjectsOfType<LineRenderer>();
            foreach (LineRenderer item in enabledLines)
            {
                item.gameObject.SetActive(false);
            }
            DOTween.To(() => codingPanel.offsetMin, value => codingPanel.offsetMin = value, new Vector2(1920, 0), 1).OnComplete(() =>
            {
                isMovingTab = false;
                UIManager.ClickBlock(false);
            });
        }
    }

    public static Vector3 ScreenToWorldPoint(bool isCanvasScale = true)
    {
        Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return new Vector3(point.x, point.y, 0) * (isCanvasScale ? CanvasSync.CanvasScale : 1);
    }

    public static void StageClear()
    {
        if (!Instance.isClear)
        {
            Instance.isClear = true;

            Instance.clearPanel.transform.DOScaleX(1, 0.5f);
            Instance.clearParticle.Play();
            Instance.Invoke("MoveToTitle", 3);
            PlaySound.PlaySFX(PlaySound.audioBox.SFX_stageClear);
            PlaySound.SetFade(PlaySound.bgmSource, 0);

            int beforeScore = SecurityPlayerPrefs.GetInt($"stage{currentStageIndex}_timer", -1);

            if(beforeScore != -1)
            {
                if (beforeScore > clearTime)
                {
                    SecurityPlayerPrefs.SetInt($"stage{currentStageIndex}_timer", clearTime);
                }
            }
            else
            {
                SecurityPlayerPrefs.SetInt($"stage{currentStageIndex}_timer", clearTime);
            }
        }
    }

    public void MoveToTitle()
    {
        UIManager.ResetFadeInOut(false, () => {
            skipTitleScene = true;
            PoolManager.ResetPool();
            SceneManager.LoadScene("Title");
        });
    }

    public void StageReset()
    {
        PlaySound.PlaySFX(PlaySound.audioBox.SFX_reset);
        UIManager.ResetFadeInOut(false, () => {
            PoolManager.ResetPool();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
    }

    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}

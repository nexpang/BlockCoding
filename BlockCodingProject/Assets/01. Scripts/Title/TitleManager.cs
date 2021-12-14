using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public static TitleManager Instance;

    public StageData stageData;

    public RectTransform titleCanvas;
    public CanvasGroup startPanel;
    public CanvasGroup stagePanel;
    public RectTransform stageBlock;

    public CanvasGroup optionPanel;
    public RectTransform optionBlock;

    public TitleLine leftLine;
    public TitleLine rightLine;
    public Animator cursorAnim;

    public GameObject logoText;

    [Header("Buttons")]
    public Button startBtn; 
    public Button settingBtn; 
    public Button settingExitBtn; 
    public Button leaveBtn; 
    public Button stagePrevBtn; 
    public Button stageNextBtn;
    public Button stageStartBtn;

    [Header("Stage")]
    public Text stageNameTxt;
    public Text stageLoreTxt;
    public Text stageTimeTxt;
    public Image stageImg;
    public GameObject stageClearIcon;
    private int stageIndex = 0;

    private bool isSkipped = false;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }

        if (!GameManager.skipTitleScene)
        {
            startPanel.alpha = 1;
            startPanel.blocksRaycasts = true;
            startPanel.interactable = true;

            stagePanel.alpha = 0;
            stagePanel.blocksRaycasts = false;
            stagePanel.interactable = false;

            stageBlock.transform.localScale = new Vector3(1, 0, 1);

            logoText.SetActive(false);

            startBtn.gameObject.SetActive(false);
            settingBtn.gameObject.SetActive(false);
            leaveBtn.gameObject.SetActive(false);

            startBtn.transform.localScale = Vector3.zero;
            settingBtn.transform.localScale = Vector3.zero;
            leaveBtn.transform.localScale = Vector3.zero;
        }
        else
        {
            StartSceneSkip();
            stageIndex = GameManager.currentStageIndex;

            stagePanel.alpha = 1;
            stagePanel.blocksRaycasts = true;
            stagePanel.interactable = true;

            stageBlock.transform.localScale = Vector3.one;
        }

        RefreshStage();
    }

    void Start()
    {
        CanvasSync.ScaleEdit(titleCanvas);

        startBtn.onClick.AddListener(() =>
        {
            StagePanel(true);
        });

        settingBtn.onClick.AddListener(() =>
        {
            SettingPanel(true);
        });

        settingExitBtn.onClick.AddListener(() =>
        {
            SettingPanel(false);
        });

        leaveBtn.onClick.AddListener(() =>
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; //play모드를 false로.
#else
                    Application.Quit();
#endif
        });

        stagePrevBtn.onClick.AddListener(() =>
        {
            PlaySound.PlaySFX(PlaySound.audioBox.SFX_buttonClick);

            stageIndex--;
            RefreshStage();
        });

        stageNextBtn.onClick.AddListener(() =>
        {
            PlaySound.PlaySFX(PlaySound.audioBox.SFX_buttonClick);

            stageIndex++;
            RefreshStage();
        });

        stageStartBtn.onClick.AddListener(() =>
        {
            DOTween.KillAll();
            SceneManager.LoadScene("InGame");
        });

        if(isSkipped)
        {
            PlaySound.PlayBGM(PlaySound.audioBox.BGM_title);
        }
    }

    public void MainPanelStart()
    {
        Sequence seq = DOTween.Sequence();
        logoText.SetActive(true);
        logoText.transform.localScale = new Vector3(1, 0, 1);
        PlaySound.PlaySFX(PlaySound.audioBox.SFX_gameStart);
        Color lineColor = Color.white;

        cursorAnim.SetTrigger("idle");

        seq.AppendInterval(2);
        seq.Append(
            logoText.transform.DOScaleY(1, 0.5f)
        );
        seq.AppendInterval(2);
        seq.AppendCallback(() =>
        {
            startPanel.alpha = 0;
            startPanel.blocksRaycasts = false;
            startPanel.interactable = false;

            PlaySound.PlaySFX(PlaySound.audioBox.SFX_fingerSnap);
            PlaySound.PlayBGM(PlaySound.audioBox.BGM_title);
            DOTween.To(() => lineColor, value =>
            {
                leftLine.line.startColor = value;
                leftLine.line.endColor = value;
                rightLine.line.startColor = value;
                rightLine.line.endColor = value;
            }, Color.black, 1);
        });

        seq.AppendInterval(1);
        seq.Append(startBtn.transform.DOScale(1, 0.5f).OnStart(() =>
        {
            startBtn.gameObject.SetActive(true);
        }));
        seq.Append(settingBtn.transform.DOScale(1, 0.5f).OnStart(() =>
        {
            settingBtn.gameObject.SetActive(true);
        }));
        seq.Append(leaveBtn.transform.DOScale(1, 0.5f).OnStart(() =>
        {
            leaveBtn.gameObject.SetActive(true);
        }));


    }

    public void StagePanel(bool value)
    {
        PlaySound.PlaySFX(PlaySound.audioBox.SFX_buttonClick);
        PanelMove(stagePanel, stageBlock, value);
    }

    public void SettingPanel(bool value)
    {
        PlaySound.PlaySFX(PlaySound.audioBox.SFX_buttonClick);
        PanelMove(optionPanel, optionBlock, value);
    }

    public static void PanelMove(CanvasGroup panel, RectTransform block, bool value)
    {
        if (value)
        {
            panel.alpha = 1;
            panel.interactable = true;
            panel.blocksRaycasts = true;
            block.DOScaleY(1, 0.5f).SetEase(Ease.OutBounce);
        }
        else
        {
            block.DOScaleY(0, 0.5f).SetEase(Ease.OutBounce).OnComplete(() =>
            {
                panel.alpha = 0;
                panel.blocksRaycasts = false;
                panel.interactable = false;
            });
        }
    }

    private void RefreshStage()
    {
        if (stageIndex == 0)
        {
            stagePrevBtn.gameObject.SetActive(false);
        }
        else
        {
            stagePrevBtn.gameObject.SetActive(true);
        }

        if (stageIndex > stageData.stageInfos.Length - 2)
        {
            stageNextBtn.gameObject.SetActive(false);
        }
        else
        {
            stageNextBtn.gameObject.SetActive(true);
        }

        stageNameTxt.text = $"Stage {stageData.stageInfos[stageIndex].stageName}";
        stageLoreTxt.text = $"\"{stageData.stageInfos[stageIndex].stageLore}\"";
        int currentTime = SecurityPlayerPrefs.GetInt($"stage{stageIndex}_timer", -1);
        stageClearIcon.SetActive(currentTime != -1);
        stageTimeTxt.text = "최고기록\n" + TimeCalculate(currentTime);
        stageImg.sprite = stageData.stageInfos[stageIndex].stageSprite;
        GameManager.currentStageIndex = stageIndex;
    }

    private string TimeCalculate(int time)
    {
        if(time == -1)
        {
            return "--:--";
        }
        else
        {
            return (time / 60).ToString("00") + ":" + (time % 60).ToString("00");
        }
    }

    private void StartSceneSkip()
    {
        logoText.SetActive(true);
        logoText.transform.localScale = Vector3.one;

        startPanel.alpha = 0;
        startPanel.blocksRaycasts = false;
        startPanel.interactable = false;

        startBtn.transform.localScale = Vector3.one;
        startBtn.gameObject.SetActive(true);
        settingBtn.transform.localScale = Vector3.one;
        settingBtn.gameObject.SetActive(true);
        leaveBtn.transform.localScale = Vector3.one;
        leaveBtn.gameObject.SetActive(true);

        leftLine.line.startColor = Color.black;
        leftLine.line.endColor = Color.black;
        rightLine.line.startColor = Color.black;
        rightLine.line.endColor = Color.black;

        leftLine.line.gameObject.SetActive(true);

        Vector3[] lineWayPoints = new Vector3[2];

        lineWayPoints[0] = Vector2.zero;
        lineWayPoints[1] = (rightLine.transform.position - leftLine.transform.position) * CanvasSync.canvasScaleValue;

        leftLine.line.SetPositions(lineWayPoints);

        leftLine.isTouchable = false;
        rightLine.isTouchable = false;

        isSkipped = true;
    }
}

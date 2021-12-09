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


    public LineRenderer leftLine;
    public LineRenderer rightLine;

    public GameObject logoText;

    [Header("Buttons")]
    public Button startBtn; 
    public Button settingBtn; 
    public Button leaveBtn; 
    public Button stagePrevBtn; 
    public Button stageNextBtn;
    public Button stageStartBtn;

    [Header("Stage")]
    public Text stageNameTxt;
    public Image stageImg;
    private int stageIndex = 0;

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
        }
        else
        {
            StartSceneSkip();
            stageIndex = GameManager.currentStageIndex;

            stagePanel.alpha = 1;
            stagePanel.blocksRaycasts = true;
            stagePanel.interactable = true;

            stageBlock.transform.localScale = new Vector3(1, 1, 1);
        }

        RefreshStage();
    }

    void Start()
    {
        CanvasSync.ScaleEdit(titleCanvas);

        logoText.SetActive(false);

        startBtn.gameObject.SetActive(false);
        settingBtn.gameObject.SetActive(false);
        leaveBtn.gameObject.SetActive(false);

        startBtn.transform.localScale = Vector3.zero;
        settingBtn.transform.localScale = Vector3.zero;
        leaveBtn.transform.localScale = Vector3.zero;


        startBtn.onClick.AddListener(() =>
        {
            StagePanel(true);
        });

        settingBtn.onClick.AddListener(() =>
        {

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
            stageIndex--;
            RefreshStage();
        });

        stageNextBtn.onClick.AddListener(() =>
        {
            stageIndex++;
            RefreshStage();
        });

        stageStartBtn.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("InGame");
        });
    }

    public void MainPanelStart()
    {
        Sequence seq = DOTween.Sequence();
        logoText.SetActive(true);
        logoText.transform.localScale = new Vector3(1, 0, 1);
        PlaySound.PlaySFX(PlaySound.audioBox.SFX_gameStart);
        Color lineColor = Color.white;

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

            DOTween.To(() => lineColor, value =>
            {
                leftLine.startColor = value;
                leftLine.endColor = value;
                rightLine.startColor = value;
                rightLine.endColor = value;
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
        if(value)
        {
            stagePanel.alpha = 1;
            stagePanel.interactable = true;
            stagePanel.blocksRaycasts = true;
            stageBlock.DOScaleY(1, 0.5f).SetEase(Ease.OutBounce);
        }
        else
        {
            stageBlock.DOScaleY(0, 0.5f).SetEase(Ease.OutBounce).OnComplete(() =>
            {
                stagePanel.alpha = 0;
                stagePanel.blocksRaycasts = false;
                stagePanel.interactable = false;
            });
        }
    }

    public void SettingPanel(bool value)
    {
        if (value)
        {
            optionPanel.alpha = 1;
            optionPanel.interactable = true;
            optionPanel.blocksRaycasts = true;
            optionBlock.DOScaleY(1, 0.5f).SetEase(Ease.OutBounce);
        }
        else
        {
            optionBlock.DOScaleY(0, 0.5f).SetEase(Ease.OutBounce).OnComplete(() =>
            {
                optionPanel.alpha = 0;
                optionPanel.blocksRaycasts = false;
                optionPanel.interactable = false;
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
        stageImg.sprite = stageData.stageInfos[stageIndex].stageSprite;
        GameManager.currentStageIndex = stageIndex;
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

        leftLine.startColor = Color.black;
        leftLine.endColor = Color.black;
        rightLine.startColor = Color.black;
        rightLine.endColor = Color.black;
    }
}

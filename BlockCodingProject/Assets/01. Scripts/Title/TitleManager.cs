using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public static TitleManager Instance;

    public RectTransform titleCanvas;
    public CanvasGroup startPanel;
    public CanvasGroup stagePanel;
    public RectTransform stageBlock;

    public LineRenderer leftLine;
    public LineRenderer rightLine;

    public GameObject logoText;

    [Header("Buttons")]
    public Button startBtn; 
    public Button settingBtn; 
    public Button leaveBtn; 

    private void Awake()
    {
        if(!Instance)
        {
            Instance = this;
        }
        
        startPanel.alpha = 1;
        startPanel.blocksRaycasts = true;
        startPanel.interactable = true;
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
            stagePanel.alpha = 1;
            stagePanel.interactable = true;
            stagePanel.blocksRaycasts = true;
            stageBlock.DOScaleY(1, 0.5f).SetEase(Ease.OutBounce);
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
    }

    public void MainPanelStart()
    {
        Sequence seq = DOTween.Sequence();
        logoText.SetActive(true);
        logoText.transform.localScale = new Vector3(1, 0, 1);
        Color lineColor = Color.white;

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

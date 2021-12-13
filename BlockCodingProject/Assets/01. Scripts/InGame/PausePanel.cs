using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour
{
    [Space(20)]
    public Text stageNameTxt;
    public Text stageLoreTxt;

    [Space(20)]
    public StageData data;

    [Space(20)]
    public CanvasGroup pausePanel;
    public RectTransform pauseBlock;

    [Space(20)]
    public CanvasGroup settingPanel;
    public RectTransform settingBlock;

    [Space(20)]
    public Button pauseBtn;
    public Button resumeBtn;
    public Button settingBtn;
    public Button settingExitBtn;
    public Button exitBtn;

    // Start is called before the first frame update
    void Start()
    {
        pauseBtn.onClick.AddListener(() =>
        {
            TitleManager.PanelMove(pausePanel, pauseBlock, true);
            PlaySound.SetFade(PlaySound.bgmSource, 0);
            stageNameTxt.text = $"Stage {data.stageInfos[GameManager.currentStageIndex].stageName}";
            stageLoreTxt.text = $"\"{data.stageInfos[GameManager.currentStageIndex].stageLore}\"";
        });

        resumeBtn.onClick.AddListener(() =>
        {
            TitleManager.PanelMove(pausePanel, pauseBlock, false);
            PlaySound.SetFade(PlaySound.bgmSource, 1);
        });

        settingBtn.onClick.AddListener(() =>
        {
            TitleManager.PanelMove(settingPanel, settingBlock, true);
        });

        settingExitBtn.onClick.AddListener(() =>
        {
            TitleManager.PanelMove(settingPanel, settingBlock, false);
        });

        exitBtn.onClick.AddListener(() =>
        {
            GameManager.Instance.MoveToTitle();
        });
    }
}

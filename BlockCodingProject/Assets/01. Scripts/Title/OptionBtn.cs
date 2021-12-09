using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionBtn : MonoBehaviour
{
    public enum SettingType
    {
        BGM,
        SFX
    }

    public Sprite onSprite;
    public Sprite offSprite;
    public SettingType type;

    private Button button;
    private Image buttonImg;
    private Dictionary<SettingType, AudioSource> sourcesDic = new Dictionary<SettingType, AudioSource>();

    private void Awake()
    {
        button = GetComponent<Button>();
        buttonImg = GetComponent<Image>();
    }

    private void Start()
    {
        sourcesDic.Add(SettingType.BGM, PlaySound.bgmSource);
        sourcesDic.Add(SettingType.SFX, PlaySound.sfxSource);

        bool isMute = sourcesDic[type].mute;
        button.onClick.AddListener(() =>
        {
            isMute = sourcesDic[type].mute;
            buttonImg.sprite = isMute ? onSprite : offSprite;

            PlaySound.SetMute(sourcesDic[type], !isMute);
        });

        buttonImg.sprite = isMute ? offSprite : onSprite;
    }
}

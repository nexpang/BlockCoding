using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TitleLetter : MonoBehaviour
{
    private RectTransform rect;

    public bool startUp;
    private bool start = true;

    public float delay;
    public float time;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void Start()
    {
        Move(startUp);
    }

    private void Move(bool up)
    {
        if(up)
        {
            rect.DOAnchorPosY(start ? 10 : 20, time).SetDelay(delay).SetRelative().OnComplete(() =>
             {
                 start = false;
                 Move(false);
             }).SetEase(Ease.InOutSine);
        }
        else
        {
            rect.DOAnchorPosY(start ? -10 : -20, time).SetDelay(delay).SetRelative().OnComplete(() =>
            {
                start = false;
                Move(true);
            }).SetEase(Ease.InOutSine);
        }
    }
}

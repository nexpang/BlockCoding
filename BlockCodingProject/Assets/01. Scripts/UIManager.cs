using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager Instance;

    public Slider zoomSlider;
    private const float zoomScale = 10;

    private void Awake()
    {
        if(!Instance)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        float reverseValue = -zoomSlider.value + 1;

        CameraMove.ZoomCam(3f + reverseValue * zoomScale);
        zoomSlider.onValueChanged.AddListener(x => {
            reverseValue = -zoomSlider.value + 1;
            CameraMove.ZoomCam(3f + reverseValue * zoomScale);
        });
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public Slider zoomSlider;
    public Transform blockPanel;
    private const float zoomScale = 0.5f;

    public static float currentZoomValue = 1f;

    private void Awake()
    {
        if(!Instance)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        currentZoomValue = 0.6f + zoomSlider.value * zoomScale;
        blockPanel.transform.localScale = new Vector3(currentZoomValue, currentZoomValue, currentZoomValue);

        zoomSlider.onValueChanged.AddListener(x => {
            currentZoomValue = 0.6f + zoomSlider.value * zoomScale;
            blockPanel.transform.localScale = new Vector3(currentZoomValue, currentZoomValue, currentZoomValue);
        });

    }
}

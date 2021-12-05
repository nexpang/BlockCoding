using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasSync
{
    public static float canvasScaleValue;
    public static float CanvasScale { get { return canvasScaleValue / UIManager.currentZoomValue; } }

    public static void ScaleEdit(RectTransform canvas)
    {
        canvasScaleValue = 1 / canvas.transform.localScale.x;
    }
}

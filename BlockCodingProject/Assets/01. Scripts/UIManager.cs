using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager Instance;

    public SpriteRenderer rangeCircle;

    private void Awake()
    {
        if(!Instance)
        {
            Instance = this;
        }
    }

    public static void ShowCircleRange(Vector2 pos, float remainDis)
    {
        Instance.rangeCircle.gameObject.SetActive(true);
        Instance.rangeCircle.transform.position = pos;
        Instance.rangeCircle.size = new Vector2(remainDis * 2, remainDis * 2);
        Instance.rangeCircle.GetComponent<CircleCollider2D>().radius = remainDis;
    }

    public static void DisabledCircleRange()
    {
        Instance.rangeCircle.gameObject.SetActive(false);
    }
}

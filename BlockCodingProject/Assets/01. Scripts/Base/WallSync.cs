using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class WallSync : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnGUI()
    {
        if (gameObject.activeSelf)
        {
            boxCollider.size = rect.sizeDelta;
        }
    }
}

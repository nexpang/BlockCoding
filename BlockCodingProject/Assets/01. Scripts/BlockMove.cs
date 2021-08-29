using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockMove : MonoBehaviour
{
    Vector3 dir = Vector3.zero;
    Outline outline;

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    private void OnMouseEnter()
    {
        outline.enabled = true;
    }

    private void OnMouseExit()
    {
        outline.enabled = false;
    }

    private void OnMouseDown()
    {
        dir = GetMousePos() - transform.position;
    }

    private void OnMouseDrag()
    {
        transform.position = GetMousePos() - dir;
    }

    private Vector3 GetMousePos()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return new Vector3(mousePos.x, mousePos.y, 0);
    }
}

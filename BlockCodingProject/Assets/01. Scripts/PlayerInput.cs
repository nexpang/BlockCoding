using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInput : MonoBehaviour
{
    private bool isDrag = false;
    private Vector3 clickPos;
    private Vector3 movePos;

    private Vector3 blockPanelPos;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                blockPanelPos = UIManager.Instance.blockPanel.position;
                isDrag = true;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDrag = false;
        }

        if(isDrag)
        {
            if(!EventSystem.current.IsPointerOverGameObject())
            {
                movePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                Vector3 dir = movePos - clickPos;

                UIManager.Instance.blockPanel.position = blockPanelPos + dir;
            }
        }
    }
}

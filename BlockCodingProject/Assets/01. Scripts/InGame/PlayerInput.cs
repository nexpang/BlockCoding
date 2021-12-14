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

    public Vector2 blockPanelMin;
    public Vector2 blockPanelMax;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (GameManager.Instance.gameStatus == GameStatus.CODING)
            {
                if (!GameManager.IsPointerOverUIObject())
                {
                    clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    blockPanelPos = UIManager.Instance.blockPanel.position;
                    isDrag = true;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDrag = false;
        }

        if(isDrag)
        {
            if(!GameManager.IsPointerOverUIObject())
            {
                movePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                Vector3 dir = movePos - clickPos;

                Vector3 blockPos = UIManager.Instance.blockPanel.position;

                blockPos = blockPanelPos + dir;
                blockPos.x = Mathf.Clamp(blockPos.x, blockPanelMin.x, blockPanelMax.x);
                blockPos.y = Mathf.Clamp(blockPos.y, blockPanelMin.y, blockPanelMax.y);

                UIManager.Instance.blockPanel.position = blockPos;
            }
        }
    }
}

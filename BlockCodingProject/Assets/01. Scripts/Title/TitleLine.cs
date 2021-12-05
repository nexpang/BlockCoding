using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TitleLine : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public TitleLine targetHole;

    public TitleLine connectedHole;
    public LineRenderer line;

    public GameObject outline;

    private Vector3[] lineWayPoints = new Vector3[2];
    private float disMag;

    private bool isTouchable = true;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isTouchable) return;

        if (connectedHole != null)
        {
            if (connectedHole.line != null)
            {
                connectedHole.line.gameObject.SetActive(false);
            }
            connectedHole.connectedHole = null;
        }
        connectedHole = null;
        line.gameObject.SetActive(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isTouchable) return;

        // 포지션 설정
        {
            lineWayPoints[0] = Vector3.zero;
            lineWayPoints[1] = GameManager.ScreenToWorldPoint() - (transform.position * CanvasSync.canvasScaleValue);

            line.SetPositions(lineWayPoints);
        }

        Vector3 dis = targetHole.transform.position - (lineWayPoints[1] + (transform.position * CanvasSync.canvasScaleValue)) / CanvasSync.canvasScaleValue;
        disMag = dis.sqrMagnitude;

        if (disMag < 1)
        {
            targetHole.outline.SetActive(true);
        }
        else
        {
            targetHole.outline.SetActive(false);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isTouchable) return;

        if (disMag < 2)
        {
            Debug.Log("붙혀짐 : " + targetHole.gameObject.name);

            targetHole.DisconnetLineAll();
            targetHole.connectedHole = this;

            connectedHole = targetHole;

            lineWayPoints[1] = (targetHole.transform.position - transform.position) * CanvasSync.canvasScaleValue;
            targetHole.outline.SetActive(false);
            line.SetPositions(lineWayPoints);

            TitleManager.Instance.MainPanelStart();
            isTouchable = false;
            targetHole.isTouchable = false;
        }
        else
        {
            line.gameObject.SetActive(false);
        }
    }

    public void DisconnetLineAll()
    {
        if (line != null)
        {
            line.gameObject.SetActive(false);
        }

        if (connectedHole != null)
        {
            if (connectedHole.line != null)
            {
                connectedHole.line.gameObject.SetActive(false);
            }
            connectedHole.connectedHole = null;
        }
        connectedHole = null;
    }
}

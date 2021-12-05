using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public enum LineType
{
    INPUT,
    OUTPUT
}

public class LineGenerate : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public LineType lineType;
    public LineGenerate connectedHole;
    public LineRenderer line;
    public bool isTouchable = true;

    private Vector3[] lineWayPoints = new Vector3[2];
    private float lowestDis = 0;
    private int lowestDisCircle = -1;
    private Outline outline;

    private bool isCantConnect = false;

    [NonSerialized] public BlockScript myBlock;

    void Awake()
    {
        outline = GetComponent<Outline>();
    }

    void Start()
    {
        GameManager.Instance.lineCircles.Add(this);

        StartCoroutine(SetLineStart());
    }

    IEnumerator SetLineStart()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        if (connectedHole != null)
        {
            myBlock.OnConnected(connectedHole.myBlock);
            connectedHole.myBlock.OnConnected(myBlock);
            yield return null;
            connectedHole.connectedHole = this;
            line.gameObject.SetActive(true);
            if (!isTouchable)
            {
                connectedHole.isTouchable = false;
                line.startColor = Color.red;
                line.endColor = Color.red;
            }

            lineWayPoints[0] = Vector3.zero;
            lineWayPoints[1] = Vector3.zero;

            DOTween.To(() => lineWayPoints[1], value =>
            {
                lineWayPoints[1] = value;
                line.SetPositions(lineWayPoints);
            }, (connectedHole.transform.position - transform.position) * CanvasSync.CanvasScale, 1);
        }
    }

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
            connectedHole.myBlock.OnDisconnected(myBlock);
            myBlock.OnDisconnected(connectedHole.myBlock);
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
            lineWayPoints[1] = GameManager.ScreenToWorldPoint() - (transform.position * CanvasSync.CanvasScale);

            Vector3 dir = lineWayPoints[1] - lineWayPoints[0];

            Vector3 worldDir = GameManager.ScreenToWorldPoint(false) - (transform.position * UIManager.currentZoomValue);

            dir = Vector3.ClampMagnitude(dir, 5 * CanvasSync.canvasScaleValue);
            worldDir = Vector3.ClampMagnitude(worldDir, 5 * UIManager.currentZoomValue);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, worldDir, worldDir.magnitude, 1 << LayerMask.NameToLayer("Wall"));

            isCantConnect = hit;
            if (hit)
            {
                if (!UIManager.Instance.wallBlockEffect.activeSelf)
                {
                    UIManager.Instance.wallBlockEffect.SetActive(true);
                }
                UIManager.Instance.wallBlockEffect.transform.position = hit.point;
                Debug.Log("벽에 닿음");
            }
            else
            {
                if (UIManager.Instance.wallBlockEffect.activeSelf)
                {
                    UIManager.Instance.wallBlockEffect.SetActive(false);
                }
            }

            lineWayPoints[1] = lineWayPoints[0] + dir;

            line.SetPositions(lineWayPoints);
        }

        if (isCantConnect) return;

        lowestDis = 10000;
        lowestDisCircle = -1;
        for(int i = 0;i < GameManager.Instance.lineCircles.Count;i++)
        {
            GameManager.Instance.lineCircles[i].outline.enabled = false;

            if (GameManager.Instance.lineCircles[i] == this) continue; // 만약 이게 나거나
            if (GameManager.Instance.lineCircles[i].myBlock == this.myBlock) continue; // 블록이 같거나
            if (GameManager.Instance.lineCircles[i].lineType == this.lineType) continue; // 줄 타입이 같다면 넘긴다.
            if (!GameManager.Instance.lineCircles[i].isTouchable) continue;

            Vector3 dis = GameManager.Instance.lineCircles[i].transform.position - (lineWayPoints[1] + (transform.position * CanvasSync.CanvasScale)) / CanvasSync.CanvasScale;
            if(lowestDis > dis.sqrMagnitude)
            {
                lowestDis = dis.sqrMagnitude;
                lowestDisCircle = i;
            }
        }

        if (lowestDisCircle != -1)
        {
            if (lowestDis < 1)
            {
                GameManager.Instance.lineCircles[lowestDisCircle].outline.enabled = true;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isTouchable) return;

        if (lowestDis < 1)
        {
            if (lowestDisCircle != -1)
            {
                Debug.Log("붙혀짐 : " + GameManager.Instance.lineCircles[lowestDisCircle].gameObject.name);

                GameManager.Instance.lineCircles[lowestDisCircle].DisconnetLineAll();
                GameManager.Instance.lineCircles[lowestDisCircle].connectedHole = this;

                Effect_Connect effect = PoolManager.GetItem<Effect_Connect>();
                effect.transform.position = GameManager.Instance.lineCircles[lowestDisCircle].transform.position;

                connectedHole = GameManager.Instance.lineCircles[lowestDisCircle];
                myBlock.OnConnected(connectedHole.myBlock);

                connectedHole.myBlock.OnConnected(myBlock);

                lineWayPoints[1] = (GameManager.Instance.lineCircles[lowestDisCircle].transform.position - transform.position) * CanvasSync.CanvasScale;
                GameManager.Instance.lineCircles[lowestDisCircle].outline.enabled = false;
                line.SetPositions(lineWayPoints);
            }
        }
        else
        {
            line.gameObject.SetActive(false);
        }

        UIManager.Instance.wallBlockEffect.SetActive(false);
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
            connectedHole.myBlock.OnDisconnected(myBlock);
            connectedHole.connectedHole = null;
            myBlock.OnDisconnected(connectedHole.myBlock);
        }
        connectedHole = null;
    }
}

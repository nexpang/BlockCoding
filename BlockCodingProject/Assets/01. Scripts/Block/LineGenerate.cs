using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

    private Vector3[] lineWayPoints = new Vector3[2];
    private float lowestDis = 0;
    private int lowestDisCircle = -1;
    private Outline outline;

    [NonSerialized] public BlockScript myBlock;

    void Awake()
    {
        outline = GetComponent<Outline>();
    }

    void Start()
    {
        GameManager.Instance.lineCircles.Add(this);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (connectedHole != null)
        {
            if(connectedHole.line != null)
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
        // 포지션 설정
        { 
            lineWayPoints[0] = new Vector3(transform.position.x, transform.position.y, 5);
            lineWayPoints[1] = GameManager.ScreenToWorldPoint();

            Vector3 dir = lineWayPoints[1] - lineWayPoints[0];
            dir = Vector3.ClampMagnitude(dir, 5);

            lineWayPoints[1] = lineWayPoints[0] + dir;

            line.SetPositions(lineWayPoints);
        }

        lowestDis = 10000;
        lowestDisCircle = -1;
        for(int i = 0;i< GameManager.Instance.lineCircles.Count;i++)
        {
            GameManager.Instance.lineCircles[i].outline.enabled = false;

            if (GameManager.Instance.lineCircles[i] == this) continue; // 만약 이게 나거나
            if (GameManager.Instance.lineCircles[i].myBlock == this.myBlock) continue; // 블록이 같거나
            if (GameManager.Instance.lineCircles[i].lineType == this.lineType) continue; // 줄 타입이 같다면 넘긴다.

            Vector3 dis = GameManager.Instance.lineCircles[i].transform.position - lineWayPoints[1];
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
        if (lowestDis < 1)
        {
            if (lowestDisCircle != -1)
            {
                Debug.Log("붙혀짐 : " + GameManager.Instance.lineCircles[lowestDisCircle].gameObject.name);

                connectedHole = GameManager.Instance.lineCircles[lowestDisCircle];
                GameManager.Instance.lineCircles[lowestDisCircle].DisconnetLineAll();
                GameManager.Instance.lineCircles[lowestDisCircle].connectedHole = this;

                lineWayPoints[1] = GameManager.Instance.lineCircles[lowestDisCircle].transform.position;
                GameManager.Instance.lineCircles[lowestDisCircle].outline.enabled = false;
                line.SetPositions(lineWayPoints);
            }
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

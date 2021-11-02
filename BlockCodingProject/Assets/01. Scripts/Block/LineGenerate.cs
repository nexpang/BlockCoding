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

public class LineGenerate : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public LineType lineType;
    public LineGenerate connectedHole;
    public LineRenderer line;

    private RectTransform rt;
    private Vector3[] lineWayPoints = new Vector3[7];
    private List<LineCorner> lineCorners = new List<LineCorner>();

    //private float lowestDis = 0;
    //private int lowestDisCircle = -1;
    private Outline outline;

    public int defaultCornerCount = 5;
    public float defaultDis = 5;
    public float atLeastDis = 0.5f;
    private float remainDis = 5;

    [NonSerialized] public BlockScript myBlock;

    void Awake()
    {
        outline = GetComponent<Outline>();
        rt = GetComponent<RectTransform>();
    }

    void Start()
    {
        GameManager.Instance.lineCircles.Add(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (GameManager.Instance.currentSelectCircle == this) return; // 만약 또 클릭하면 리턴한다.

        if(GameManager.Instance.currentSelectCircle != null)
        {
            GameManager.Instance.currentSelectCircle.DisableCircle(); // 이미 선택된게 있다면 그 원은 선 짓기 중단한다.
        }

        GameManager.Instance.currentSelectCircle = this;

        outline.enabled = true;
        UIManager.ShowCircleRange(rt.transform.position, defaultDis);

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
        lineWayPoints[0] = new Vector3(transform.position.x, transform.position.y, 5);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
/*        rt.anchoredPosition = new Vector3(rt.anchoredPosition.x, rt.anchoredPosition.y, 0.5f);
        UIManager.ShowCircleRange(rt.transform.position, 5);

        if (connectedHole != null)
        {
            if (connectedHole.line != null)
            {
                connectedHole.line.gameObject.SetActive(false);
            }
            connectedHole.connectedHole = null;
        }
        connectedHole = null;
        line.gameObject.SetActive(true);*/
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 포지션 설정
/*        {
            lineWayPoints[0] = new Vector3(transform.position.x, transform.position.y, 5);
            lineWayPoints[1] = GameManager.ScreenToWorldPoint();

            Vector3 dir = lineWayPoints[1] - lineWayPoints[0];
            dir = Vector3.ClampMagnitude(dir, 5);

            lineWayPoints[1] = lineWayPoints[0] + dir;

            line.SetPositions(lineWayPoints);
        }*/

/*        lowestDis = 10000;
        lowestDisCircle = -1;
        for (int i = 0; i < GameManager.Instance.lineCircles.Count; i++)
        {
            GameManager.Instance.lineCircles[i].outline.enabled = false;

            if (GameManager.Instance.lineCircles[i] == this) continue; // 만약 이게 나거나
            if (GameManager.Instance.lineCircles[i].myBlock == this.myBlock) continue; // 블록이 같거나
            if (GameManager.Instance.lineCircles[i].lineType == this.lineType) continue; // 줄 타입이 같다면 넘긴다.

            Vector3 dis = GameManager.Instance.lineCircles[i].transform.position - lineWayPoints[1];
            if (lowestDis > dis.sqrMagnitude)
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
        }*/
    }

    public void OnEndDrag(PointerEventData eventData)
    {
/*        rt.anchoredPosition = new Vector3(rt.anchoredPosition.x, rt.anchoredPosition.y, 0f);
        UIManager.DisabledCircleRange();

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
        }*/
    }

    public void CheckDistance(Vector3 pos)
    {
        if (atLeastDis >= remainDis) return;

        float distance;
        if (lineCorners.Count > 0)
        {
            Vector3 dir = lineCorners[lineCorners.Count - 1].transform.position - pos;
            distance = dir.magnitude;
        }
        else
        {
            Vector3 dir = transform.position - pos;
            distance = dir.magnitude;
        }

        if(remainDis >= distance)
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                GameManager.Instance.currentSelectCircle.DisableCircle();
                GameManager.Instance.currentSelectCircle = null;
                return;
            }

            AddCorner(pos);
        }
    }

    public void DisableCircle()
    {
        outline.enabled = false;
        UIManager.DisabledCircleRange();
        DisconnetLineAll();
    }

    public void DisconnetLineAll()
    {
        if (line != null)
        {
            line.positionCount = 0;
            line.gameObject.SetActive(false);
        }

        if (lineCorners.Count > 0)
        {
            DisableCorner(lineCorners);
        }

        if (connectedHole != null)
        {
            if (connectedHole.line != null)
            {
                connectedHole.line.gameObject.SetActive(false);
            }

            if (connectedHole.lineCorners.Count > 0)
            {
                DisableCorner(connectedHole.lineCorners);
            }

            connectedHole.connectedHole = null;
        }

        remainDis = defaultDis;
        connectedHole = null;
    }

    private void LineAnimation()
    {
        for (int i = line.positionCount - 1; i >= 0; i--)
        {
            if (i - 1 >= 0)
            {
                //line.Setline.GetPositions[i - 1];

            }
        }
    }

    public void AddCorner(Vector3 pos)
    {
        if (defaultCornerCount <= lineCorners.Count) return;

        if(lineCorners.Count > 0)
        {
            Vector3 dir = lineCorners[lineCorners.Count - 1].transform.position - pos;

            if(dir.magnitude < atLeastDis)
            {
                return;
            }
            remainDis -= dir.magnitude;
        }
        else
        {
            Vector3 dir = transform.position - pos;

            if (dir.magnitude < atLeastDis)
            {
                return;
            }
            remainDis -= dir.magnitude;
        }

        UIManager.DisabledCircleRange();

        LineCorner newCorner = PoolManager.GetItem<LineCorner>();
        newCorner.transform.position = pos;

        lineCorners.Add(newCorner);
        lineWayPoints[lineCorners.Count] = new Vector3(newCorner.transform.position.x, newCorner.transform.position.y, 5);
        line.positionCount = lineCorners.Count + 1;
        line.SetPositions(lineWayPoints);
        UIManager.ShowCircleRange(newCorner.transform.position, remainDis);
    }

    public void DisableCorner(List<LineCorner> lineCorners)
    {
        for (int i = lineCorners.Count - 1; i >= 0; i--)
        {
            lineCorners[i].gameObject.SetActive(false);
            lineCorners.RemoveAt(i);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public EdgeCollider2D edgeCollider;
    public int segmentCount = 1;
    public int maxSegmentCount = 30;
    public int defaultSegmentCnt;
    public int constraintLoop = 15;
    public float segmentLength = 0.01f;
    public float rodeWidth = 0.1f;
    //public Vector2 gravity = new Vector2(0f, -9.81f);
    [Space(10f)]
    public Transform startTransfrom;
    public Transform endTransfrom;
    public LayerMask hitLayer;

    
    public List<Segment> segments = new List<Segment>();


    public bool isConnect = false;

    private void Reset()
    {
        TryGetComponent(out lineRenderer);
        TryGetComponent(out edgeCollider);
    }

    private void Awake()
    {
        defaultSegmentCnt = segmentCount;
        Vector2 segmentPos = startTransfrom.position;
        for (int i = 0; i < segmentCount; i++)
        {
            segments.Add(new Segment(segmentPos));
        }
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            for (int i = 0; i < segments.Count; i++)
            {
                segments[i].position = startTransfrom.position;
            }
        }
    }

    private void FixedUpdate()
    {
        if (segmentCount != defaultSegmentCnt)
        {
            if (segmentCount < defaultSegmentCnt)
            {
                int count = segmentCount;
                for (int i = 1; i <= defaultSegmentCnt - count; i++)
                {
                    segments.Remove(segments[count - i]);
                }
            }
            else
            {
                for (int i = 1; i <= segmentCount - defaultSegmentCnt; i++)
                {
                    segments.Add(new Segment(startTransfrom.position));
                }
            }
            defaultSegmentCnt = segmentCount;
        }
        UpdateSegments();
        for (int i = 0; i < constraintLoop; i++)
        {
            ApplyContraint();
            AdjustCollision();
        }
        DrawRode();
    }

    private void DrawRode()
    {
        lineRenderer.startWidth = rodeWidth;
        lineRenderer.endWidth = rodeWidth;
        Vector3[] segmentPositions = new Vector3[segments.Count];
        Vector2[] colliderPositions = new Vector2[segments.Count];
        for (int i = 0; i < segments.Count; i++)
        {
            segmentPositions[i] = segments[i].position;
            colliderPositions[i] = segments[i].position;
        }
        lineRenderer.positionCount = segmentPositions.Length;
        lineRenderer.SetPositions(segmentPositions);

        if(edgeCollider)
        {
            edgeCollider.edgeRadius = rodeWidth * 0.5f;
            edgeCollider.points = colliderPositions;
        }
    }

    private void UpdateSegments()
    {
        for (int i = 0; i < segments.Count; i++)
        {
            //segments[i].velocity = segments[i].position - segments[i].previousPos;
            segments[i].previousPos = segments[i].position;
            //segments[i].position += gravity * Time.fixedDeltaTime * Time.fixedDeltaTime;
            //segments[i].position += segments[i].velocity;
        }
    }

    private void ApplyContraint()
    {
        segments[0].position = startTransfrom.position;
        segments[segments.Count-1].position = endTransfrom.position;
        for (int i = 0; i < segments.Count-1; i++)
        {
            float dist = (segments[i].position - segments[i + 1].position).magnitude;
            float difference = segmentLength - dist;
            Vector2 dir = (segments[i + 1].position - segments[i].position).normalized;

            Vector2 movement = dir * difference;
            if(i==0)
            {
                segments[i + 1].position += movement;
            }
            else
            {
                if(isConnect)
                {
                    if (i == segments.Count - 2)
                    {
                        segments[i].position -= movement;
                    }
                    segments[i].position -= movement * 0.5f;
                    segments[i + 1].position += movement * 0.5f;
                }
                else
                {
                    segments[i].position -= movement * 0.5f;
                    segments[i + 1].position += movement * 0.5f;

                    if (i == segments.Count - 2)
                    {
                        endTransfrom.position = segments[i].position;
                    }
                }
            }
        }
    }

    private void AdjustCollision()
    {
        for (int i = 0; i < segments.Count; i++)
        {
            Vector2 dir = segments[i].position - segments[i].previousPos;
            RaycastHit2D hit = Physics2D.CircleCast(segments[i].position, rodeWidth * 0.5f, dir.normalized, 0f, hitLayer);
            if(hit)
            {
                segments[i].position = hit.point + hit.normal * rodeWidth * 0.5f;
                segments[i].previousPos = segments[i].position;
            }
        }
    }

    [System.Serializable]
    public class Segment
    {
        public Vector2 previousPos;
        public Vector2 position;
        public Vector2 velocity;

        public Segment(Vector2 _position)
        {
            previousPos = _position;
            position = _position;
            velocity = Vector2.zero;
        }
    }
}

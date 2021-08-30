using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockMove : MonoBehaviour
{
    public Transform UpAttachPoint;   // 이 블록 위에 블록을 붙힐 수 있는가
    public Transform DownAttachPoint; // 이 블록 아래에 블록을 붙힐 수 있는가

    public BlockMove upBlock;  // 위에 있는 블록
    public BlockMove downBlock; // 아래에 있는 블록

    protected bool isUpAttachable;
    protected bool isDownAttachable;

    [SerializeField]
    protected bool upAttachReady = false;
    [SerializeField]
    protected bool downAttachReady = false;
    public List<BlockMove> toBeAttachBlockList;

    Vector3 dir = Vector3.zero;
    Outline outline;
    protected Transform mainBlockParent;
    public List<BlockMove> allDownBlocks = new List<BlockMove>();
    BoxCollider2D[] boxColliders;

    private void Awake()
    {
        outline = GetComponent<Outline>();
        mainBlockParent = transform.parent;
    }

    private void Start()
    {
        isUpAttachable = UpAttachPoint != null;
        isDownAttachable = DownAttachPoint != null;
        boxColliders = mainBlockParent.GetComponentsInChildren<BoxCollider2D>();

        GameManager.Instance.downDetect += () =>
        {
            allDownBlocks = GetDownAllBlocks();
        };
    }

    private void OnMouseEnter()
    {
        outline.effectColor = Color.white;
        outline.enabled = true;
    }

    private void OnMouseExit()
    {
        outline.enabled = false;
    }

    private void OnMouseDown()
    {
        dir = GetMousePos() - mainBlockParent.transform.position;
        mainBlockParent.transform.SetParent(GameManager.Instance.clickedObjectBox);
        GameManager.Instance.currentClickedObj = this.gameObject;

        for (int i = 0; i < allDownBlocks.Count; i++)
        {
            allDownBlocks[i].mainBlockParent.SetParent(mainBlockParent);
        }
        StartCoroutine(RefreshCollider());
    }

    private void OnMouseDrag()
    {
        mainBlockParent.transform.position = GetMousePos() - dir;
    }

    private void OnMouseUpAsButton()
    {
        if (upBlock != null)
        {
            upBlock.allDownBlocks.Clear();
            upBlock.downBlock = null;
            upBlock = null;
        }

        if (downBlock == null)
        {
            allDownBlocks.Clear();
        }

        if (toBeAttachBlockList.Count > 0)
        {
            UpAttachReady();
            DownAttachReady();
            BlockHighlight(toBeAttachBlockList[0].outline, false);
            toBeAttachBlockList.Clear();
        }
        else
        {
/*            for (int i = 0; i < GameManager.Instance.allBlocks.Length; i++)
            {
                if (GameManager.Instance.allBlocks[i].upBlock == this)
                {
                    GameManager.Instance.allBlocks[i].upBlock = null;
                }

                if (GameManager.Instance.allBlocks[i].downBlock == this)
                {
                    GameManager.Instance.allBlocks[i].downBlock = null;
                    GameManager.Instance.allBlocks[i].allDownBlocks.Clear();
                }
            }*/
            OnNoneBlockDetect();
        }

        GameManager.Instance.downDetect();

        mainBlockParent.transform.SetParent(GameManager.Instance.blockBox);
        for (int i = 0; i < allDownBlocks.Count; i++)
        {
            allDownBlocks[i].mainBlockParent.SetParent(GameManager.Instance.blockBox);
        }
    }

    void UpAttachReady()
    {
        if (upAttachReady)
        {
            if (!isDownAttachable) return;

            BlockPointSet(new Vector2(0, 0), toBeAttachBlockList[0].UpAttachPoint.position);
            downBlock = toBeAttachBlockList[0];
            toBeAttachBlockList[0].upBlock = this;
            if (toBeAttachBlockList[0].GetHierarchySort() == 0)
            {
                HierarchySort(0);
            }
            else
            {
                HierarchySort(toBeAttachBlockList[0].GetHierarchySort() - 1);
            }
            upAttachReady = false;
        }
    }

    void DownAttachReady()
    {
        if (downAttachReady)
        {
            if (!isUpAttachable) return;

            BlockPointSet(new Vector2(0, 1), toBeAttachBlockList[0].DownAttachPoint.position);
            upBlock = toBeAttachBlockList[0];
            toBeAttachBlockList[0].downBlock = this;
            HierarchySort(toBeAttachBlockList[0].GetHierarchySort() + 1);
            downAttachReady = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger)
        {
            if (GameManager.Instance.currentClickedObj == this.gameObject)
            {
                for (int i = 0; i < toBeAttachBlockList.Count; i++)
                {
                    if (toBeAttachBlockList[i] == collision.FoundBlock()) return;
                }

                for (int i = 0; i < allDownBlocks.Count; i++)
                {
                    if (allDownBlocks[i] == collision.FoundBlock()) return;
                }

                for (int i = 0; i < boxColliders.Length; i++)
                {
                    if (boxColliders[i] == collision.GetComponent<BoxCollider2D>()) return;
                }

                if (collision.CompareTag("UpColl"))
                {
                    if (!isDownAttachable) return;
                    upAttachReady = true; // 해당 블록 위에 붙을 준비 완료
                }

                if (collision.CompareTag("DownColl"))
                {
                    if (!isUpAttachable) return;
                    downAttachReady = true; // 해당 블록 아래에 붙을 준비 완료
                }

                toBeAttachBlockList.Add(collision.FoundBlock());
                BlockHighlight();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.isTrigger)
        {
            if (GameManager.Instance.currentClickedObj == this.gameObject)
            {
                if (collision.CompareTag("UpColl"))
                {
                    upAttachReady = false;
                }

                if (collision.CompareTag("DownColl"))
                {
                    downAttachReady = false;
                }

                BlockHighlight(collision.FoundBlock().GetComponent<Outline>(), false);
                toBeAttachBlockList.Remove(collision.FoundBlock());
            }
        }
    }

    private void BlockHighlight()
    {
        if (toBeAttachBlockList.Count > 0)
        {
            toBeAttachBlockList[0].GetComponent<Outline>().effectColor = new Color(0.5f, 0.5f, 1);
            toBeAttachBlockList[0].GetComponent<Outline>().enabled = true;
            for (int i = 1; i < toBeAttachBlockList.Count; i++)
            {
                toBeAttachBlockList[i].GetComponent<Outline>().enabled = false;
            }
        }
    }

    private void BlockHighlight(Outline outline, bool value)
    {
        outline.enabled = value;
    }

    private Vector3 GetMousePos()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return new Vector3(mousePos.x, mousePos.y, 0);
    }

    protected void HierarchySort(int index)
    {
        mainBlockParent.transform.SetSiblingIndex(index);
    }

    protected int GetHierarchySort()
    {
        return mainBlockParent.transform.GetSiblingIndex();
    }

    protected void BlockPointSet(Vector2 pivot, Vector2 position)
    {
        mainBlockParent.transform.position = position;
        mainBlockParent.GetComponent<RectTransform>().pivot = pivot;
    }

    protected IEnumerator RefreshCollider()
    {
        foreach(BoxCollider2D item in boxColliders)
        {
            item.enabled = false;
        }
        yield return null;
        foreach (BoxCollider2D item in boxColliders)
        {
            item.enabled = true;
        }
    }

    protected virtual void OnNoneBlockDetect()
    {
        mainBlockParent.transform.SetParent(GameManager.Instance.unAttachedObjBox);
    }

    protected List<BlockMove> GetDownAllBlocks()
    {
        allDownBlocks.Clear();
        DownBlockSave(this);

        return allDownBlocks;
    }

    void DownBlockSave(BlockMove block)
    {
        if (block.downBlock != null)
        {
            allDownBlocks.Add(block.downBlock);
            if(allDownBlocks.Count > 10)
            {
                Debug.LogError("무한루프 감지");
                return;
            }
            DownBlockSave(block.downBlock);

        }
    }
}

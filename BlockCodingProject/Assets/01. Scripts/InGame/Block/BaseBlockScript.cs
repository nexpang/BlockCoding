using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBlockScript : BlockScript
{
    public bool isFixedUpdate = false;
    public LineGenerate output;

    private void Awake()
    {
        output.myBlock = this;
    }

    public override void OnConnected(BlockScript connectedBy)
    {
        base.OnConnected(connectedBy);
        ChildBlockScript childBlock = connectedBy.GetComponent<ChildBlockScript>();
        if(childBlock != null)
        {
            if(isFixedUpdate) childBlock.onFixedPlay = BlockAbility;
            else childBlock.onPlay = BlockAbility;
        }
        else // 확장 블록 이라면
        {
            print("확장 블럭 감지"); 
        }
    }

    public override void OnDisconnected(BlockScript disconnectedBy)
    {
        base.OnDisconnected(disconnectedBy);
        ChildBlockScript childBlock = disconnectedBy.GetComponent<ChildBlockScript>();
        if (childBlock != null)
        {
            if (isFixedUpdate) childBlock.onFixedPlay = null;
            else childBlock.onPlay = null;
        }
    }

    public virtual void BlockAbility(GameObject obj)
    {

    }
}

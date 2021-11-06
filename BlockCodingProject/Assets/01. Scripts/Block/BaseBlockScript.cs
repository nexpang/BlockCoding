using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBlockScript : BlockScript
{
    public bool isFixedUpdate = false;
    public LineGenerate output;

    [HideInInspector]
    public ChildBlockScript childBlock;

    private void Awake()
    {
        output.myBlock = this;
    }

    public override void OnConnected(BlockScript connectedBy)
    {
        base.OnConnected(connectedBy);
        childBlock = output.connectedHole.myBlock.GetComponent<ChildBlockScript>();
        if(childBlock != null)
        {
            if(isFixedUpdate) childBlock.onFixedPlay = BlockAbility;
            else childBlock.onPlay = BlockAbility;
        }
    }

    public override void OnDisconnected(BlockScript disconnectedBy)
    {
        base.OnDisconnected(disconnectedBy);
        if (childBlock != null)
        {
            if (isFixedUpdate) childBlock.onFixedPlay = null;
            else childBlock.onPlay = null;
        }
    }

    public virtual void BlockAbility()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBlockScript : BlockScript
{
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
            childBlock.onPlay = BlockAbility;
        }
    }

    public override void OnDisconnected(BlockScript disconnectedBy)
    {
        base.OnDisconnected(disconnectedBy);
        if (childBlock != null)
        {
            childBlock.onPlay = null;
        }
    }

    public virtual void BlockAbility()
    {

    }
}

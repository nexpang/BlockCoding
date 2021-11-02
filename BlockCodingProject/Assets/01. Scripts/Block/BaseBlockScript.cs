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

    public override void OnConnected()
    {
        base.OnConnected();
        childBlock = output.connectedHole.myBlock.GetComponent<ChildBlockScript>();
        if(childBlock != null)
        {
            childBlock.onPlay = BlockAbility;
        }
    }

    public override void OnDisconnected()
    {
        base.OnDisconnected();
        if (childBlock != null)
        {
            childBlock.onPlay = null;
        }
    }

    public virtual void BlockAbility()
    {

    }
}

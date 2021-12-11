using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define_Portal : BlockScript
{
    public LineGenerate upOutput;
    public LineGenerate downOutput;

    public GameObject input_portal;
    public GameObject output_portal;

    private void Awake()
    {
        upOutput.myBlock = this;
        downOutput.myBlock = this;
    }

    private void PlayerTeleport(GameObject obj)
    {
        for (int i = 0; i < Define_Player.defined_Player.Count; i++)
        {
            if (obj == Define_Player.defined_Player[i])
            {
                
            }
        }
    }

    public override void OnConnected(BlockScript connectedBy, LineGenerate line)
    {
        Debug.Log(line.gameObject.name);
        base.OnConnected(connectedBy, line);
        ChildBlockScript child = connectedBy.GetComponent<ChildBlockScript>();

        if (child != null)
        {
            if (line == upOutput)
            {
                input_portal = child.inGameObj;
                ObjectCollider coll = child.inGameObj.GetComponent<ObjectCollider>();
                coll.ActionPlay();
            }

            if (line == downOutput)
            {
                output_portal = child.inGameObj;
            }
        }
    }

    public override void OnDisconnected(BlockScript disconnectedBy, LineGenerate line)
    {
        base.OnDisconnected(disconnectedBy, line);
        ChildBlockScript child = disconnectedBy.GetComponent<ChildBlockScript>();

        if (child != null)
        {
            if (line == upOutput)
            {
                input_portal = null;
            }

            if (line == downOutput)
            {
                output_portal = null;
            }
        }
    }
}

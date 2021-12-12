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
                if (output_portal != null)
                {
                    obj.transform.position = output_portal.transform.position;

                    Effect_Teleport input_effect = PoolManager.GetItem<Effect_Teleport>();
                    Effect_Teleport output_effect = PoolManager.GetItem<Effect_Teleport>();

                    input_effect.transform.position = input_portal.transform.position;
                    output_effect.transform.position = output_portal.transform.position;
                }
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
                coll.enterAction = PlayerTeleport;
                coll.ActionPlay();
            }

            if (line == downOutput)
            {
                output_portal = child.inGameObj;

                if (input_portal != null)
                {
                    ObjectCollider coll = input_portal.GetComponent<ObjectCollider>();
                    coll.ActionPlay();
                }
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
                ObjectCollider coll = child.inGameObj.GetComponent<ObjectCollider>();
                coll.enterAction = null;
            }

            if (line == downOutput)
            {
                output_portal = null;
            }
        }
    }
}

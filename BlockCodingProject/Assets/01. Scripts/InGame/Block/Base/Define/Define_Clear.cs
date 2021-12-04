using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define_Clear : BaseBlockScript
{
    private void PlayerClear(GameObject obj)
    {
        for (int i = 0; i < Define_Player.defined_Player.Count; i++)
        {
            if (obj == Define_Player.defined_Player[i])
            {
                StageClear();
            }
        }
    }

    public override void OnConnected(BlockScript connectedBy)
    {
        base.OnConnected(connectedBy);
        ChildBlockScript child = connectedBy.GetComponent<ChildBlockScript>();
        if (child != null)
        {
            PlayerClear(child.inGameObj);
            ObjectCollider coll = child.inGameObj.GetComponent<ObjectCollider>();
            coll.enterAction = PlayerClear;
            coll.ActionPlay();
        }
    }

    public override void OnDisconnected(BlockScript disconnectedBy)
    {
        base.OnDisconnected(disconnectedBy);
        ChildBlockScript child = disconnectedBy.GetComponent<ChildBlockScript>();

        if (child != null)
        {
            ObjectCollider coll = child.inGameObj.GetComponent<ObjectCollider>();
            coll.enterAction = null;
        }
    }

    public void StageClear()
    {
        print("Å¬¸®¾î");
    }
}
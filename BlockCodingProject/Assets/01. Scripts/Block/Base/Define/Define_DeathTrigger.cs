using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define_DeathTrigger : BaseBlockScript
{
    private void PlayerDeath(GameObject obj)
    {
        for (int i = 0; i < Define_Player.defined_Player.Count; i++)
        {
            if (obj == Define_Player.defined_Player[i])
            {
                Destroy(obj);
            }
        }
    }

    public override void OnConnected(BlockScript connectedBy)
    {
        base.OnConnected(connectedBy);
        ChildBlockScript child = connectedBy.GetComponent<ChildBlockScript>();

        ObjectCollider coll = child.inGameObj.GetComponent<ObjectCollider>();
        coll.enterAction = PlayerDeath;
        coll.ActionPlay();
    }

    public override void OnDisconnected(BlockScript disconnectedBy)
    {
        base.OnDisconnected(disconnectedBy);
        ChildBlockScript child = disconnectedBy.GetComponent<ChildBlockScript>();

        ObjectCollider coll = child.inGameObj.GetComponent<ObjectCollider>();
        coll.enterAction = null;
    }
}
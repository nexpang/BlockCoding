using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Define_Player : BaseBlockScript
{
    public static List<GameObject> defined_Player = new List<GameObject>();

    public override void OnConnected(BlockScript connectedBy)
    {
        base.OnConnected(connectedBy);
        ChildBlockScript child = connectedBy.GetComponent<ChildBlockScript>();

        defined_Player.Add(child.inGameObj);

        ObjectCollider coll = child.inGameObj.GetComponent<ObjectCollider>();
        if (coll != null)
        {
            for(int i = 0; i<coll.collisionList.Count;i++)
            {
                ObjectCollider item_coll = coll.collisionList[i].GetComponent<ObjectCollider>();
                if (item_coll != null)
                {
                    item_coll.ActionPlay();
                }
            }
        }
    }

    public override void OnDisconnected(BlockScript disconnectedBy)
    {
        base.OnDisconnected(disconnectedBy);
        ChildBlockScript child = disconnectedBy.GetComponent<ChildBlockScript>();

        defined_Player.Remove(child.inGameObj);
    }
}

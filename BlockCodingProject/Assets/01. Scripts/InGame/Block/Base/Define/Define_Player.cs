using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Define_Player : BaseBlockScript
{
    public static List<GameObject> defined_Player = new List<GameObject>();

    public override void OnConnected(BlockScript connectedBy, LineGenerate line)
    {
        base.OnConnected(connectedBy, line);
        ChildBlockScript child = connectedBy.GetComponent<ChildBlockScript>();

        if (child != null)
        {
            defined_Player.Add(child.inGameObj);
            print("Ãß°¡µÊ : " + child.inGameObj.name);

            ObjectCollider coll = child.inGameObj.GetComponent<ObjectCollider>();
            coll.ActionPlay();
        }
    }

    public override void OnDisconnected(BlockScript disconnectedBy, LineGenerate line)
    {
        base.OnDisconnected(disconnectedBy, line);
        ChildBlockScript child = disconnectedBy.GetComponent<ChildBlockScript>();

        if (child != null)
        {
            defined_Player.Remove(child.inGameObj);
        }
    }
}

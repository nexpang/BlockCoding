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
        print(defined_Player.Count + " " + defined_Player[0]);
    }

    public override void OnDisconnected(BlockScript disconnectedBy)
    {
        base.OnDisconnected(disconnectedBy);
        ChildBlockScript child = disconnectedBy.GetComponent<ChildBlockScript>();

        defined_Player.Remove(child.inGameObj);
        print(defined_Player.Count);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour
{
    public bool isConnected = false;

    public virtual void OnDisconnected(BlockScript disconnectedBy)
    {
        isConnected = false;
    }

    public virtual void OnConnected(BlockScript connectedBy)
    {
        isConnected = true;
    }
}

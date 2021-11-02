using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour
{
    public bool isConnected = false;

    public virtual void OnDisconnected()
    {
        isConnected = false;
    }

    public virtual void OnConnected()
    {
        isConnected = true;
    }
}

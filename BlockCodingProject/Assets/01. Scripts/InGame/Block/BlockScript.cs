using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour
{
    public virtual void OnDisconnected(BlockScript disconnectedBy, LineGenerate line)
    {
        
    }

    public virtual void OnConnected(BlockScript connectedBy, LineGenerate line)
    {

    }
}

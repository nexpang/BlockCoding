using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExClass
{
    public static BlockMove FoundBlock(this Collider2D collider)
    {
        Transform parent = collider.transform.parent;
        
        for(int i = 0; i < parent.transform.childCount;i++)
        {
            if(parent.GetChild(i).GetComponent<BlockMove>() != null)
            {
                return parent.GetChild(i).GetComponent<BlockMove>();
            }
        }

        return null;
    }
}

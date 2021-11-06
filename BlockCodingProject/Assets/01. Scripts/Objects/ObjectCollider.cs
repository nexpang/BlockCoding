using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCollider : MonoBehaviour
{
    [HideInInspector]
    public List<GameObject> collisionList = new List<GameObject>();
    public Action<GameObject> enterAction = null;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collisionList.Add(collision.gameObject);
        if (enterAction != null)
        {
            enterAction(collision.gameObject);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        collisionList.Remove(collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collisionList.Add(collision.gameObject);
        if (enterAction != null)
        {
            enterAction(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collisionList.Remove(collision.gameObject);
    }

    public void ActionPlay()
    {
        if (enterAction != null)
        {
            for(int i = 0; i<collisionList.Count;i++)
            {
                enterAction(collisionList[i]);
            }
        }
    }
}

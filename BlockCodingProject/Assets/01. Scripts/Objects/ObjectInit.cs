using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform obj in transform.GetComponentsInChildren<Transform>())
        {
            int zPos = (int)obj.position.y;
            obj.position = new Vector3(obj.position.x, obj.position.y, obj.position.y);
        }
    }
}

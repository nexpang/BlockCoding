using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public GameObject lineCornerPrefab;

    private void Awake()
    {
        PoolManager.CreatePool<LineCorner>(lineCornerPrefab, this.transform, 10);
    }
}

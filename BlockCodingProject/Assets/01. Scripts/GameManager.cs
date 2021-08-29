using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Transform blockBox;
    public Transform clickedObjectBox;

    private void Awake()
    {
        Instance = this;
    }
}

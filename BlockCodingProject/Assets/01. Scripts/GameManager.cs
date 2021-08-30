using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Transform blockBox;
    public Transform clickedObjectBox;
    public Transform unAttachedObjBox;

    public GameObject currentClickedObj;
    public BlockMove[] allBlocks;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        allBlocks = FindObjectsOfType<BlockMove>();
    }
}

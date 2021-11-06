using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildBlockScript : BlockScript
{
    public GameObject inGameObj;
    public LineGenerate input;
    public Action onPlay = null;
    public Action onFixedPlay = null;

    private void Awake()
    {
        input.myBlock = this;
    }

    private void Update()
    {
        if(onPlay != null)
        {
            onPlay();
        }
    }

    private void FixedUpdate()
    {
        if (onFixedPlay != null)
        {
            onFixedPlay();
        }
    }
}

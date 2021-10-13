using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour
{
    public LineGenerate input;
    public LineGenerate output;

    private void Awake()
    {
        input.myBlock = this;
        output.myBlock = this;
    }
}

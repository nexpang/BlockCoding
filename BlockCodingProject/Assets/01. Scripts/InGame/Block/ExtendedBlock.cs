using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtendedBlock : BlockScript
{
    public LineGenerate input;

    public LineGenerate upOutput;
    public LineGenerate downOutput;

    private void Awake()
    {
        input.myBlock = this;
        upOutput.myBlock = this;
        downOutput.myBlock = this;
    }

    public override void OnConnected(BlockScript connectedBy)
    {
        BaseBlockScript baseBlock = connectedBy.GetComponent<BaseBlockScript>();

        if (baseBlock != null) // 연결된것이 input쪽이라면.
        {
            baseBlock.output.connectedHole = input;

            if (upOutput.connectedHole != null) // 위쪽에 이미 연결되어있다.
            {
                upOutput.connectedHole.myBlock.OnConnected(input.myBlock);
                input.connectedHole.myBlock.OnConnected(upOutput.connectedHole.myBlock);
            }

            if (downOutput.connectedHole != null) // 아래쪽에 이미 연결되어있다.
            {
                downOutput.connectedHole.myBlock.OnConnected(input.myBlock);
                input.connectedHole.myBlock.OnConnected(downOutput.connectedHole.myBlock);
            }
        }
        else // 연결된 쪽이 output 쪽이라면.
        {
            if (input.connectedHole != null) // input쪽에 이미 연결되어있다.
            {
                connectedBy.OnConnected(input.myBlock);
                input.connectedHole.myBlock.OnConnected(connectedBy);
            }
        }
    }

    public override void OnDisconnected(BlockScript disconnectedBy)
    {
        BaseBlockScript baseBlock = disconnectedBy.GetComponent<BaseBlockScript>();

        if (baseBlock != null) // 해제된것이 input쪽이라면.
        {

            if (upOutput.connectedHole != null) // 위쪽에 이미 연결되어있다.
            {
                upOutput.connectedHole.myBlock.OnDisconnected(baseBlock);
                baseBlock.OnDisconnected(upOutput.connectedHole.myBlock);
            }

            if (downOutput.connectedHole != null) // 아래쪽에 이미 연결되어있다.
            {
                downOutput.connectedHole.myBlock.OnDisconnected(baseBlock);
                baseBlock.OnDisconnected(downOutput.connectedHole.myBlock);
            }
        }
        else // 연결된 쪽이 output 쪽이라면.
        {
            if (input.connectedHole != null) // input쪽에 이미 연결되어있다.
            {
                disconnectedBy.OnDisconnected(input.myBlock);
                input.connectedHole.myBlock.OnDisconnected(disconnectedBy);
            }
        }
    }
}

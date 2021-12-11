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

    public override void OnConnected(BlockScript connectedBy, LineGenerate line)
    {
        BaseBlockScript baseBlock = connectedBy.GetComponent<BaseBlockScript>();

        if (baseBlock != null) // 연결된것이 input쪽이라면.
        {
            baseBlock.output.connectedHole = input;

            if (upOutput.connectedHole != null) // 위쪽에 이미 연결되어있다.
            {
                upOutput.connectedHole.myBlock.OnConnected(baseBlock, input);
                input.connectedHole.myBlock.OnConnected(upOutput.connectedHole.myBlock, line); // 
            }

            if (downOutput.connectedHole != null) // 아래쪽에 이미 연결되어있다.
            {
                downOutput.connectedHole.myBlock.OnConnected(baseBlock, input);
                input.connectedHole.myBlock.OnConnected(downOutput.connectedHole.myBlock, line); // 
            }
        }
        else // 연결된 쪽이 output 쪽이라면.
        {
            if (input.connectedHole != null) // input쪽에 이미 연결되어있다.
            {
                connectedBy.OnConnected(input.myBlock, input);
                input.connectedHole.myBlock.OnConnected(connectedBy, line); //
            }
        }
    }

    public override void OnDisconnected(BlockScript disconnectedBy, LineGenerate line)
    {
        BaseBlockScript baseBlock = disconnectedBy.GetComponent<BaseBlockScript>();

        if (baseBlock != null) // 해제된것이 input쪽이라면.
        {

            if (upOutput.connectedHole != null) // 위쪽에 이미 연결되어있다.
            {
                upOutput.connectedHole.myBlock.OnDisconnected(baseBlock, line);
                baseBlock.OnDisconnected(upOutput.connectedHole.myBlock, line);
            }

            if (downOutput.connectedHole != null) // 아래쪽에 이미 연결되어있다.
            {
                downOutput.connectedHole.myBlock.OnDisconnected(baseBlock, line);
                baseBlock.OnDisconnected(downOutput.connectedHole.myBlock, line);
            }
        }
        else // 연결된 쪽이 output 쪽이라면.
        {
            if (input.connectedHole != null) // input쪽에 이미 연결되어있다.
            {
                disconnectedBy.OnDisconnected(input.myBlock, line);
                input.connectedHole.myBlock.OnDisconnected(disconnectedBy, line);
            }
        }
    }
}

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

        if (baseBlock != null) // ����Ȱ��� input���̶��.
        {
            baseBlock.output.connectedHole = input;

            if (upOutput.connectedHole != null) // ���ʿ� �̹� ����Ǿ��ִ�.
            {
                upOutput.connectedHole.myBlock.OnConnected(baseBlock, input);
                input.connectedHole.myBlock.OnConnected(upOutput.connectedHole.myBlock, line); // 
            }

            if (downOutput.connectedHole != null) // �Ʒ��ʿ� �̹� ����Ǿ��ִ�.
            {
                downOutput.connectedHole.myBlock.OnConnected(baseBlock, input);
                input.connectedHole.myBlock.OnConnected(downOutput.connectedHole.myBlock, line); // 
            }
        }
        else // ����� ���� output ���̶��.
        {
            if (input.connectedHole != null) // input�ʿ� �̹� ����Ǿ��ִ�.
            {
                connectedBy.OnConnected(input.myBlock, input);
                input.connectedHole.myBlock.OnConnected(connectedBy, line); //
            }
        }
    }

    public override void OnDisconnected(BlockScript disconnectedBy, LineGenerate line)
    {
        BaseBlockScript baseBlock = disconnectedBy.GetComponent<BaseBlockScript>();

        if (baseBlock != null) // �����Ȱ��� input���̶��.
        {

            if (upOutput.connectedHole != null) // ���ʿ� �̹� ����Ǿ��ִ�.
            {
                upOutput.connectedHole.myBlock.OnDisconnected(baseBlock, line);
                baseBlock.OnDisconnected(upOutput.connectedHole.myBlock, line);
            }

            if (downOutput.connectedHole != null) // �Ʒ��ʿ� �̹� ����Ǿ��ִ�.
            {
                downOutput.connectedHole.myBlock.OnDisconnected(baseBlock, line);
                baseBlock.OnDisconnected(downOutput.connectedHole.myBlock, line);
            }
        }
        else // ����� ���� output ���̶��.
        {
            if (input.connectedHole != null) // input�ʿ� �̹� ����Ǿ��ִ�.
            {
                disconnectedBy.OnDisconnected(input.myBlock, line);
                input.connectedHole.myBlock.OnDisconnected(disconnectedBy, line);
            }
        }
    }
}

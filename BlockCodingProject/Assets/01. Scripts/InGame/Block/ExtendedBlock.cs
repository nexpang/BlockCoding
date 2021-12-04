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

        if (baseBlock != null) // ����Ȱ��� input���̶��.
        {
            baseBlock.output.connectedHole = input;

            if (upOutput.connectedHole != null) // ���ʿ� �̹� ����Ǿ��ִ�.
            {
                upOutput.connectedHole.myBlock.OnConnected(input.myBlock);
                input.connectedHole.myBlock.OnConnected(upOutput.connectedHole.myBlock);
            }

            if (downOutput.connectedHole != null) // �Ʒ��ʿ� �̹� ����Ǿ��ִ�.
            {
                downOutput.connectedHole.myBlock.OnConnected(input.myBlock);
                input.connectedHole.myBlock.OnConnected(downOutput.connectedHole.myBlock);
            }
        }
        else // ����� ���� output ���̶��.
        {
            if (input.connectedHole != null) // input�ʿ� �̹� ����Ǿ��ִ�.
            {
                connectedBy.OnConnected(input.myBlock);
                input.connectedHole.myBlock.OnConnected(connectedBy);
            }
        }
    }

    public override void OnDisconnected(BlockScript disconnectedBy)
    {
        BaseBlockScript baseBlock = disconnectedBy.GetComponent<BaseBlockScript>();

        if (baseBlock != null) // �����Ȱ��� input���̶��.
        {

            if (upOutput.connectedHole != null) // ���ʿ� �̹� ����Ǿ��ִ�.
            {
                upOutput.connectedHole.myBlock.OnDisconnected(baseBlock);
                baseBlock.OnDisconnected(upOutput.connectedHole.myBlock);
            }

            if (downOutput.connectedHole != null) // �Ʒ��ʿ� �̹� ����Ǿ��ִ�.
            {
                downOutput.connectedHole.myBlock.OnDisconnected(baseBlock);
                baseBlock.OnDisconnected(downOutput.connectedHole.myBlock);
            }
        }
        else // ����� ���� output ���̶��.
        {
            if (input.connectedHole != null) // input�ʿ� �̹� ����Ǿ��ִ�.
            {
                disconnectedBy.OnDisconnected(input.myBlock);
                input.connectedHole.myBlock.OnDisconnected(disconnectedBy);
            }
        }
    }
}

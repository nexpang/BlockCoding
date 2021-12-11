using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input_Joystick : BaseBlockScript
{
    public override void OnConnected(BlockScript connectedBy, LineGenerate line)
    {
        base.OnConnected(connectedBy, line);
        ChildBlockScript child = connectedBy.GetComponent<ChildBlockScript>();

        if (child != null)
        {
            ObjectCollider coll = child.inGameObj.GetComponent<ObjectCollider>();
            if (coll != null)
            {
                coll.SetCollision();
                for (int i = 0; i < coll.collisionList.Count; i++)
                {
                    ObjectCollider item_coll = coll.collisionList[i].GetComponent<ObjectCollider>();
                    if (item_coll != null)
                    {
                        item_coll.ActionPlay();
                    }
                }
            }
        }
    }

    public override void OnDisconnected(BlockScript disconnectedBy, LineGenerate line)
    {
        base.OnDisconnected(disconnectedBy, line);
        ChildBlockScript child = disconnectedBy.GetComponent<ChildBlockScript>();

        if (child != null)
        {
            ObjectCollider coll = child.inGameObj.GetComponent<ObjectCollider>();
            if (coll != null)
            {
                coll.SetTrigger();
            }
        }
    }

    public override void BlockAbility(GameObject obj)
    {

        Vector2 joyStickDir = new Vector2(JoyStickInput.horizontalRaw, JoyStickInput.verticalRaw);

        if (obj != null)
        {
            Animator anim = obj.GetComponent<Animator>();

            if (anim != null)
            {
                anim.SetInteger("Horizontal", JoyStickInput.horizontalRaw);
                anim.SetInteger("Vertical", JoyStickInput.verticalRaw);

            }

            if (!GameManager.Instance.isClear)
            {
                obj.transform.Translate(joyStickDir * 4 * Time.deltaTime);
            }
            //childBlock.inGameObj.transform.position = new Vector3(childBlock.inGameObj.transform.position.x, childBlock.inGameObj.transform.position.y, childBlock.inGameObj.transform.position.y);
        }
    }
}

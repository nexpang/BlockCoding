using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input_Joystick : BaseBlockScript
{
    public override void BlockAbility()
    {
        Vector2 joyStickDir = new Vector2(JoyStickInput.horizontalRaw, JoyStickInput.verticalRaw);

        Animator anim = childBlock.inGameObj.GetComponent<Animator>();

        if (anim != null)
        {
            anim.SetInteger("Horizontal", JoyStickInput.horizontalRaw);
            anim.SetInteger("Vertical", JoyStickInput.verticalRaw);

        }

        childBlock.inGameObj.transform.Translate(joyStickDir * 4 * Time.deltaTime);
        //Debug.Log(childBlock.inGameObj.name + "가 이동한다고");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input_Joystick : BaseBlockScript
{
    public override void BlockAbility()
    {
        Vector2 joyStickDir = new Vector2(JoyStickInput.horizontalRaw, JoyStickInput.verticalRaw);
   
        if (childBlock.inGameObj != null)
        {
            Animator anim = childBlock.inGameObj.GetComponent<Animator>();

            if (anim != null)
            {
                anim.SetInteger("Horizontal", JoyStickInput.horizontalRaw);
                anim.SetInteger("Vertical", JoyStickInput.verticalRaw);

            }

            childBlock.inGameObj.transform.Translate(joyStickDir * 4 * Time.deltaTime);


            //childBlock.inGameObj.transform.position = new Vector3(childBlock.inGameObj.transform.position.x, childBlock.inGameObj.transform.position.y, childBlock.inGameObj.transform.position.y);
        }
    }
}

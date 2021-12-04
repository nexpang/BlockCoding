using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input_Joystick : BaseBlockScript
{
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

            obj.transform.Translate(joyStickDir * 4 * Time.deltaTime);


            //childBlock.inGameObj.transform.position = new Vector3(childBlock.inGameObj.transform.position.x, childBlock.inGameObj.transform.position.y, childBlock.inGameObj.transform.position.y);
        }
    }
}

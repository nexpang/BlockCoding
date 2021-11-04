using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input_Joystick : BaseBlockScript
{
    public override void BlockAbility()
    {
        Vector2 joySticDir = new Vector2(JoyStickInput.horizontalRaw, JoyStickInput.verticalRaw);

        childBlock.inGameObj.transform.Translate(joySticDir * 4 * Time.deltaTime);
        //Debug.Log(childBlock.inGameObj.name + "가 이동한다고");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define_DeathTrigger : BaseBlockScript
{
    private void PlayerDeath(GameObject obj)
    {
        for (int i = 0; i < Define_Player.defined_Player.Count; i++)
        {
            if (obj == Define_Player.defined_Player[i])
            {
                CameraMove.ShakeCam(3, 0.5f);
                Effect_Death effect = PoolManager.GetItem<Effect_Death>();
                effect.transform.position = obj.transform.position;
                Death(obj);
            }
        }
    }

    public override void OnConnected(BlockScript connectedBy)
    {
        base.OnConnected(connectedBy);
        ChildBlockScript child = connectedBy.GetComponent<ChildBlockScript>();

        if (child != null)
        {
            PlayerDeath(child.inGameObj);

            ObjectCollider coll = child.inGameObj.GetComponent<ObjectCollider>();
            coll.enterAction = PlayerDeath;
            coll.ActionPlay();
        }
    }

    public override void OnDisconnected(BlockScript disconnectedBy)
    {
        base.OnDisconnected(disconnectedBy);
        ChildBlockScript child = disconnectedBy.GetComponent<ChildBlockScript>();

        if (child != null)
        {
            ObjectCollider coll = child.inGameObj.GetComponent<ObjectCollider>();
            coll.enterAction = null;
        }
    }

    private void Death(GameObject obj)
    {
        PlaySound.PlaySFX(PlaySound.audioBox.SFX_playerDead);
        obj.SetActive(false);
    }
}
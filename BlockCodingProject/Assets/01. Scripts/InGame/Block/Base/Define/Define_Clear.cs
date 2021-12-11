using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define_Clear : BaseBlockScript
{
    private Dictionary<ChildBlockScript, GameObject> effectDic = new Dictionary<ChildBlockScript, GameObject>();

    private void PlayerClear(GameObject obj)
    {
        for (int i = 0; i < Define_Player.defined_Player.Count; i++)
        {
            if (obj == Define_Player.defined_Player[i])
            {
                Effect_StageClear effect = PoolManager.GetItem<Effect_StageClear>();
                effect.transform.position = obj.transform.position;

                StageClear();
            }
        }
    }

    public override void OnConnected(BlockScript connectedBy, LineGenerate line)
    {
        base.OnConnected(connectedBy, line);
        ChildBlockScript child = connectedBy.GetComponent<ChildBlockScript>();
        if (child != null)
        {
            PlayerClear(child.inGameObj);
            ObjectCollider coll = child.inGameObj.GetComponent<ObjectCollider>();

            Effect_Clear effect = PoolManager.GetItem<Effect_Clear>();
            effect.transform.SetParent(child.inGameObj.transform);
            effect.transform.localPosition = Vector2.zero;
            effectDic[child] = effect.gameObject;

            coll.enterAction = PlayerClear;
            coll.ActionPlay();
        }
    }

    public override void OnDisconnected(BlockScript disconnectedBy, LineGenerate line)
    {
        base.OnDisconnected(disconnectedBy, line);
        ChildBlockScript child = disconnectedBy.GetComponent<ChildBlockScript>();

        if (child != null)
        {
            ObjectCollider coll = child.inGameObj.GetComponent<ObjectCollider>();

            effectDic[child].SetActive(false);
            effectDic[child].transform.SetParent(GameManager.Instance.transform);
            effectDic.Remove(child);

            coll.enterAction = null;
        }
    }

    public void StageClear()
    {
        GameManager.StageClear();
    }
}
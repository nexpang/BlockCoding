using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectPoolManager : MonoBehaviour
{
    public GameObject clearEffectPrefab;
    public GameObject connectEffectPrefab;
    public GameObject deathEffectPrefab;
    public GameObject stageClearEffectPrefab;
    public GameObject teleportEffectPrefab;

    private void Awake()
    {
        PoolManager.CreatePool<Effect_Clear>(clearEffectPrefab, transform);
        PoolManager.CreatePool<Effect_Connect>(connectEffectPrefab, transform);
        PoolManager.CreatePool<Effect_Death>(deathEffectPrefab, transform);
        PoolManager.CreatePool<Effect_StageClear>(stageClearEffectPrefab, transform, 1);
        PoolManager.CreatePool<Effect_Teleport>(teleportEffectPrefab, transform, 2);
    }

    private void Start()
    {
/*        SceneManager.activeSceneChanged += (scene, scene2) =>
        {
            Debug.Log("�� ��ȯ���� ���� Ǯ ����");
            PoolManager.ResetPool();
        };*/
    }
}

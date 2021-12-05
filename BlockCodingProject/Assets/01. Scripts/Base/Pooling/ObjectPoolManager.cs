using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectPoolManager : MonoBehaviour
{
    public GameObject clearEffectPrefab;
    public GameObject connectEffectPrefab;

    private void Awake()
    {
        PoolManager.CreatePool<Effect_Clear>(clearEffectPrefab, transform);
        PoolManager.CreatePool<Effect_Connect>(connectEffectPrefab, transform);
    }

    private void Start()
    {
        SceneManager.activeSceneChanged += (scene, scene2) =>
        {
            Debug.Log("씬 전환으로 인한 풀 리셋");
            PoolManager.ResetPool();
        };
    }
}

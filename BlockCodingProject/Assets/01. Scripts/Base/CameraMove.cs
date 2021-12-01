using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class CameraMove : MonoBehaviour
{
    public static CameraMove Instance { get; private set; }

    public CinemachineVirtualCamera inGameCam;

    private CinemachineBasicMultiChannelPerlin inGameCamBPerlin;

    private bool isShake = false;
    private float currentTime = 0f; // ��鸮�� �ð�

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("�ټ��� ī�޶� ��ũ��Ʈ�� �������Դϴ�.");
        }
        Instance = this;
        inGameCamBPerlin = inGameCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public static void ShakeCam(float intensity, float time)
    {
        // �ڷ�ƾ ȣ��
        if (!Instance.isShake)
        {
            Instance.isShake = true;
            Instance.StartCoroutine(Instance.ShakeUpdate(intensity, time));
        }
        else
        {
            Instance.inGameCamBPerlin.m_AmplitudeGain = intensity;
            Instance.currentTime = time;
        }
    }

    public static void ZoomCam(float zoom)
    {
        Instance.inGameCam.m_Lens.OrthographicSize = zoom;
    }

    public IEnumerator ShakeUpdate(float intensity, float time)
    {
        inGameCamBPerlin.m_AmplitudeGain = intensity;
        currentTime = 0;

        while (true)
        {
            yield return null;
            currentTime += Time.deltaTime;
            if (currentTime >= time)
            {
                break;
            }
            inGameCamBPerlin.m_AmplitudeGain = Mathf.Lerp(intensity, 0f, currentTime / time);
        }
        isShake = false;

        inGameCamBPerlin.m_AmplitudeGain = 0;
    }
}
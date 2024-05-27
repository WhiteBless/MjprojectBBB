using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamShake : MonoBehaviour
{
    CinemachineVirtualCamera CMVCam;
    float ShakeTime;

    void Awake()
    {
        CMVCam = GetComponent<CinemachineVirtualCamera>();
        GameManager.GMInstance.CamShakeRef = this;
    }

    public void ShakeCam(float _intensity, float _time)
    {
        CinemachineBasicMultiChannelPerlin CamChannel = CMVCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        CamChannel.m_AmplitudeGain = _intensity;
        ShakeTime = _time;
    }

    void Update()
    {
        if (ShakeTime > 0)
        {
            ShakeTime -= Time.deltaTime;
            if (ShakeTime <= 0.0f)
            {
                CinemachineBasicMultiChannelPerlin CamChannel = CMVCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                CamChannel.m_AmplitudeGain = 0f;
            }
        }
    }
}

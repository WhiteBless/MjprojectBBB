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
        // 시네머신 버추얼 컴포넌트
        CMVCam = GetComponent<CinemachineVirtualCamera>();
        // 게임 매니저에 넘겨줌
        GameManager.GMInstance.CamShakeRef = this;


    }

    void Start()
    {
        CMVCam.Follow = GameManager.GMInstance.Get_PlaySceneManager().CurCharacter.transform.GetChild(1);
        CMVCam.LookAt = GameManager.GMInstance.Get_PlaySceneManager().CurCharacter.transform.GetChild(1);

        // 흔들리게 하는 컴포넌트를 가져옴
        CinemachineBasicMultiChannelPerlin CamChannel = CMVCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        if (CamChannel.m_AmplitudeGain != 0)
        {
            CamChannel.m_AmplitudeGain = 0;
        }
    }

    public void ShakeCam(float _intensity, float _time)
    {
        // 흔들리게 하는 컴포넌트를 가져옴
        CinemachineBasicMultiChannelPerlin CamChannel = CMVCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        // 흔들림 강도를 매개변수로 받은 값으로 설정
        CamChannel.m_AmplitudeGain = _intensity;
        // 흔들리는 시간 설정
        ShakeTime = _time;
    }

    void Update()
    {
        // 적용 된 ShakeTime만큼 적용
        if (ShakeTime > 0)
        {
            // ShakeTime 감소
            ShakeTime -= Time.deltaTime;
            if (ShakeTime <= 0.0f)
            {
                // 흔들리게 하는 컴포넌트 가져옴
                CinemachineBasicMultiChannelPerlin CamChannel = CMVCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                CamChannel.m_AmplitudeGain = 0f;
            }
        }
    }
}

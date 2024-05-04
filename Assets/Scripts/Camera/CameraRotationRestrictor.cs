using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraRotationRestrictor : MonoBehaviour
{
    // 카메라의 회전을 허용할지 여부
    public bool allowRotation = false;

    private void LateUpdate()
    {
        // 카메라의 회전을 허용하지 않으면 카메라의 회전을 초기 상태로 고정
        if (!allowRotation)
        {
            transform.rotation = Quaternion.identity;
        }
    }
}

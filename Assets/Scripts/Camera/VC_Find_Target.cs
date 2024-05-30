using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VC_Find_Target : MonoBehaviour
{
    CinemachineVirtualCamera VCaM;

    void Start()
    {
        VCaM = GetComponent<CinemachineVirtualCamera>();

        VCaM.LookAt = GameManager.GMInstance.Get_PlaySceneManager().CurCharacter.transform.GetChild(1);
    }
}

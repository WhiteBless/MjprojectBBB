using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_ActiveT : MonoBehaviour
{
    [SerializeField]
    GameObject[] ActiveObj;

    void OnEnable()
    {
        // 비활성화 된 오브젝트 다 활성화
        for (int i = 0; i < ActiveObj.Length; i++)
        {
            if (ActiveObj[i].activeSelf == false)
            {
                ActiveObj[i].SetActive(true);
            }
        }
    }
}

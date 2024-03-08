using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateZero : MonoBehaviour
{
    void Update()
    {
        // rotation을 항상 0으로 설정
        transform.rotation = Quaternion.identity;
    }
}

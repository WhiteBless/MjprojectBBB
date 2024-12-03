using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderRope_Rotate : MonoBehaviour
{
    Vector3 currentVector = Vector3.back;
    [SerializeField]
    float RotSpeed;

    // Update is called once per frame
    void Update()
    {
        // 로컬 축 기준으로 회전
        transform.Rotate(currentVector, RotSpeed * Time.deltaTime, Space.Self);
    }
}

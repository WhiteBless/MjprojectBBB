using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject toObj; // 워프 포인트 오브젝트

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WarpPlayer"))
        {
            // 플레이어를 워프 포인트 위치로 이동
            other.transform.position = toObj.transform.position;

            // 포탈 효과음 재생 로직은 유지
            //audioSource.PlayOneShot(portalSound, 1.0f); // 포탈 효과음 재생 코드
        }
    }
}

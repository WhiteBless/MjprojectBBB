using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class potal : MonoBehaviour
{
    public GameObject targetObj;
    public GameObject toObj;
    private bool isWarping = false; // 워프 중인지 체크하는 변수를 추가

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            targetObj = other.gameObject;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !isWarping)
        {
            StartCoroutine(TeleportRoutine());
        }
    }

    IEnumerator TeleportRoutine()
    {
        isWarping = true; // 워프 시작
        yield return new WaitForSeconds(1); // 1초 대기
        targetObj.transform.position = toObj.transform.position; // 플레이어 이동
        isWarping = false; // 워프 종료
    }
}
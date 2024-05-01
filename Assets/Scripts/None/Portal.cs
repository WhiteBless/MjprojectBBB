using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Portal : MonoBehaviour
{
    public GameObject toObj; // 워프 포인트를 나타내는 오브젝트

    private void OnTriggerEnter(Collider other)
    {
        // 이 코드는 글로벌 좌표를 사용하여 오브젝트의 위치와 회전을 설정합니다.
        Transform objectTransform = other.transform;

        other.gameObject.GetComponent<NavMeshAgent>().enabled = false;
        // 오브젝트의 위치와 회전을 toObj 오브젝트의 글로벌 위치와 회전으로 설정합니다.
        objectTransform.position = toObj.transform.position; // 글로벌 위치 설정
        objectTransform.rotation = toObj.transform.rotation; // 글로벌 회전 설정
        other.gameObject.GetComponent<NavMeshAgent>().enabled = true;
    }
}

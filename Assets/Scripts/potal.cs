using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject targetObj;
    public GameObject toObj;
    private bool isWarping = false;

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
        isWarping = true;
        yield return new WaitForSeconds(1);

        // 플레이어 이동
        targetObj.transform.position = toObj.transform.position;

        // 자식 오브젝트들도 같은 위치로 이동
        foreach (Transform childTransform in targetObj.transform)
        {
            childTransform.position = toObj.transform.position;
        }

        isWarping = false;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject targetObj;
    public GameObject toObj;
    private bool isWarping = false;

    public AudioSource audioSource; // 효과음을 재생할 AudioSource
    public AudioClip portalSound; // 포탈 효과음

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            targetObj = other.gameObject;

            // 포탈 효과음 재생
            audioSource.PlayOneShot(portalSound, 1.0f); // 포탈 효과음 재생 (볼륨은 1.0)
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
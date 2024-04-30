using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject toObj; // 워프 포인트 오브젝트

    private void OnTriggerEnter(Collider other)
    {
        // Collider가 자식 오브젝트인 경우 부모를 찾아서 이동시킨다
        Transform parentTransform = null;

        // 태그가 WarpPlayer인 오브젝트 또는 부모 오브젝트를 찾는다
        if (other.CompareTag("WarpPlayer"))
        {
            parentTransform = other.transform;
        }
        else if (other.transform.parent != null && other.transform.parent.CompareTag("WarpPlayer"))
        {
            parentTransform = other.transform.parent;
        }

        if (parentTransform != null)
        {
            // 부모 오브젝트(또는 본인)의 위치를 워프 포인트 위치로 이동
            parentTransform.position = toObj.transform.position;

            // 명시적으로 포탈 효과음 재생 로직을 유지해야 할 경우 여기에 추가
            //audioSource.PlayOneShot(portalSound, 1.0f); // 포탈 효과음 재생 코드
        }
    }
}

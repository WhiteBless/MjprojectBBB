using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class WarpDestination
{
    public Collider warpPortal; // 워프 포탈
    public Transform warpDestination; // 워프 도착 지점
}

public class Warp : MonoBehaviour
{
    public WarpDestination[] warpDestinations; // 워프 도착 지점들의 배열
    public float warpDelay = 0.5f; // 워프까지의 딜레이 시간
    public bool isWarping = false; // 워프 진행 중인지 여부를 나타내는 변수

    public void OnTriggerEnter(Collider other)
    {
        if (!isWarping && CanWarp(other))
        {
            StartCoroutine(WarpPlayer(other.gameObject, other));
        }
    }

    public bool CanWarp(Collider other)
    {
        // 플레이어와 충돌한 경우에만 워프 가능하도록 설정
        return other.CompareTag("Player");
    }

    public IEnumerator WarpPlayer(GameObject Charaacter, Collider portal)
    {
        isWarping = true;

        // 워프 애니메이션, 이펙트, 사운드 등을 추가하여 시각적인 피드백 제공 가능

        yield return new WaitForSeconds(warpDelay);

        // 워프 포탈과 일치하는 도착지를 찾음
        foreach (var warpDestination in warpDestinations)
        {
            if (warpDestination.warpPortal == portal)
            {
                // 플레이어를 선택된 도착 지점으로 이동
                Charaacter.transform.position = warpDestination.warpDestination.position;
                break;
            }
        }

        isWarping = false;
    }
}

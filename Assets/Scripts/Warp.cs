using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour
{
    public Collider[] warpPortals; // 워프 포탈들의 배열
    public Transform[] warpDestinations; // 워프 도착 지점들의 배열

    public float warpDelay = 0.5f; // 워프까지의 딜레이 시간

    private bool isWarping = false; // 워프 진행 중인지 여부를 나타내는 변수

    private void OnTriggerEnter(Collider other)
    {
        if (!isWarping && CanWarp(other))
        {
            StartCoroutine(WarpPlayer(other.gameObject));
        }
    }

    private bool CanWarp(Collider other)
    {
        // 플레이어와 충돌한 경우에만 워프 가능하도록 설정
        return other.CompareTag("Player");
    }

    private IEnumerator WarpPlayer(GameObject player)
    {
        isWarping = true;

        // 워프 애니메이션, 이펙트, 사운드 등을 추가하여 시각적인 피드백 제공 가능

        yield return new WaitForSeconds(warpDelay);

        // 워프 포탈과 일치하는 도착지를 찾음
        for (int i = 0; i < warpPortals.Length; i++)
        {
            if (warpPortals[i] == GetComponent<Collider>())
            {
                if (i < warpDestinations.Length)
                {
                    // 플레이어를 선택된 도착 지점으로 이동
                    player.transform.position = warpDestinations[i].position;
                }
                break;
            }
        }

        isWarping = false;
    }
}
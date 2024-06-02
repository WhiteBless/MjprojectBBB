using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenBossClear : MonoBehaviour
{
    public float openSpeed = 5f; // 문 열림 속도
    public float openHeight = 100f; // 문이 열릴 때 이동할 높이
    public Boss_HP_Controller bossHPController; // 보스 HP 컨트롤러 참조

    private bool isOpening = false; // 문이 열리고 있는지 확인
    private Vector3 closedPosition; // 문 닫힘 위치
    private Vector3 openPosition; // 문 열림 위치

    void Start()
    {
        // 초기 문 위치 저장
        closedPosition = transform.position;
        // 문 열림 위치 계산 (현재 위치에서 y축으로 openHeight만큼 이동)
        openPosition = new Vector3(closedPosition.x, closedPosition.y + openHeight, closedPosition.z);
    }

    void Update()
    {
        // 보스가 사망했는지 확인
        if (bossHPController.isDead && !isOpening)
        {
            isOpening = true; // 문 열림 상태로 설정
        }

        if (isOpening)
        {
            // 문 열림
            transform.position = Vector3.MoveTowards(transform.position, openPosition, openSpeed * Time.deltaTime);
            if (transform.position == openPosition)
            {
                isOpening = false; // 문이 완전히 열리면 열림 상태 중지
            }
        }
    }
}

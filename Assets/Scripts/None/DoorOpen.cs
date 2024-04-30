using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    public AudioSource doorOpenSound; // 문 열림 사운드
    public AudioSource doorCloseSound; // 문 닫힘 사운드
    public float openSpeed = 5f; // 문 열림 속도
    public float closeSpeed = 5f; // 문 닫힘 속도
    public float openHeight = 3f; // 문이 열릴 때 이동할 높이

    private bool isOpening = false; // 문이 열리고 있는지 확인
    private bool isClosing = false; // 문이 닫히고 있는지 확인
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
        if (isOpening)
        {
            // 문 열림
            transform.position = Vector3.MoveTowards(transform.position, openPosition, openSpeed * Time.deltaTime);
            if (transform.position == openPosition)
            {
                isOpening = false; // 문이 완전히 열리면 열림 상태 중지
            }
        }
        else if (isClosing)
        {
            // 문 닫힘
            transform.position = Vector3.MoveTowards(transform.position, closedPosition, closeSpeed * Time.deltaTime);
            if (transform.position == closedPosition)
            {
                isClosing = false; // 문이 완전히 닫히면 닫힘 상태 중지
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.DoorOpening);

            isOpening = true; // 문 열림 상태로 설정
            isClosing = false; // 문 닫힘 상태 해제
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (openHeight == 100) {
            if (other.CompareTag("Player"))
            {
                GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.DoorClosing);
                GetComponent<BoxCollider>().isTrigger = false;
                isClosing = true; // 문 닫힘 상태로 설정
                isOpening = false; // 문 열림 상태 해제
            }
        }
    }
}

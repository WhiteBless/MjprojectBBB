using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DoorOpen : MonoBehaviour
{
    public AudioSource doorSound; // 철문 사운드를 위한 오디오 소스
    public float openSpeed = 5f; // 철문이 열리는 속도

    private bool isOpening = false; // 철문이 현재 열리고 있는지 상태를 추적

    void Update()
    {
        if (isOpening)
        {
            // 철문을 위로 열림
            transform.Translate(Vector3.up * openSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            doorSound.Play(); // 철문 열리는 사운드 재생
            isOpening = true; // 철문 열림 상태로 설정
        }
    }
}
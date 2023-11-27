using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLookAt : MonoBehaviour
{
    public GameObject player; // Player 객체를 참조하는 변수
    public BossAnimator bossAnimator; // BossAnimator 스크립트를 참조하는 변수
    public AudioSource audioSource; // 배경 음악을 재생하기 위한 오디오 소스
    public AudioClip bgmClip; // 배경 음악 클립

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // Player 태그를 가진 오브젝트를 찾아서 player 변수에 할당
        bossAnimator = GetComponent<BossAnimator>(); // BossAnimator 컴포넌트를 찾아서 bossAnimator 변수에 할당
        audioSource = GetComponent<AudioSource>(); // AudioSource 컴포넌트를 찾아서 audioSource 변수에 할당
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null && bossAnimator.AttRadyState) // player가 null이 아니고, AttRadyState가 true일 경우
        {
            Vector3 direction = player.transform.position - transform.position; // player와 현재 오브젝트 사이의 방향 벡터 계산
            Quaternion rotation = Quaternion.LookRotation(direction); // 방향 벡터를 바탕으로 회전 각도 계산
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 2); // 부드럽게 회전

            // 배경 음악 재생
            if (!audioSource.isPlaying) // 오디오 소스가 현재 재생 중이 아니라면
            {
                audioSource.clip = bgmClip; // 배경 음악 클립을 오디오 소스의 클립으로 설정
                audioSource.Play(); // 오디오 소스 재생
            }
        }
        else if (audioSource.isPlaying) // AttRadyState가 false이고, 오디오 소스가 현재 재생 중이라면
        {
            audioSource.Stop(); // 오디오 소스 재생 중지
        }
    }
}
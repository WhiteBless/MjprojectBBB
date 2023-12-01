using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLookAt : MonoBehaviour
{
    public GameObject player; // Player 객체를 참조하는 변수
    public BossAnimator bossAnimator; // BossAnimator 스크립트를 참조하는 변수
    public AudioSource audioSource; // 배경 음악을 재생하기 위한 오디오 소스
    public AudioClip bgmClip; // 배경 음악 클립

    public BossSkillP bossSkillP; // BossSkillP 스크립트를 참조하는 변수

    public bool canUseSkills = false; // 스킬 사용 가능 여부를 결정하는 변수
    public bool isLook = true; // 보스가 플레이어를 바라보는지 여부를 결정하는 변수

    public Vector3 direction;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // Player 태그를 가진 오브젝트를 찾아서 player 변수에 할당
        bossAnimator = GetComponent<BossAnimator>(); // BossAnimator 컴포넌트를 찾아서 bossAnimator 변수에 할당
        audioSource = GetComponent<AudioSource>(); // AudioSource 컴포넌트를 찾아서 audioSource 변수에 할당

        bossSkillP = GetComponent<BossSkillP>(); // BossSkillP 컴포넌트를 찾아서 bossSkillP 변수에 할당
        bossSkillP.enabled = false; // BossSkillP 스크립트를 처음에는 비활성화
    }

    void Update()
    {
        if (player != null && bossAnimator.AttRadyState && isLook)
        {
            direction = player.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 2);

            if (!audioSource.isPlaying)
            {
                audioSource.clip = bgmClip;
                audioSource.Play();
            }

            if (canUseSkills)
            {
                bossSkillP.enabled = true; // 스킬 사용 가능할 때 BossSkillP 스크립트 활성화
                for (int i = 0; i < 5; i++)
                {
                    bossSkillP.UseSkill((BossSkillP.BossSkill)i);
                }
            }
            else
            {
                bossSkillP.enabled = false; // 스킬 사용 불가능할 때 BossSkillP 스크립트 비활성화
            }
        }
        else if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject == player)
        {
            canUseSkills = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            canUseSkills = false;
        }
    }
}
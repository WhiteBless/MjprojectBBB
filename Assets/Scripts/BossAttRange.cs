using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttRange : MonoBehaviour
{
    public BossAnimator bossAnimator; // BossAnimator 스크립트를 참조하는 변수

    // Start is called before the first frame update
    void Start()
    {
        bossAnimator = GetComponentInParent<BossAnimator>(); // 부모 오브젝트의 BossAnimator 컴포넌트를 찾아서 bossAnimator 변수에 할당
    }

    // 플레이어가 공격 범위 안에 들어왔을 때
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("P")) // 충돌한 오브젝트의 태그가 P라면
        {
            bossAnimator.AttRadyState = true; // AttRadyState를 true로 설정
        }
    }

    // 플레이어가 공격 범위를 벗어났을 때
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("P")) // 충돌한 오브젝트의 태그가 Player라면
        {
            bossAnimator.AttRadyState = false; // AttRadyState를 false로 설정
        }
    }
}
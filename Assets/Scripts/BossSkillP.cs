using UnityEngine;
using System.Collections;
using static UnityEngine.GraphicsBuffer;
using System.Runtime.CompilerServices;
using UnityEditor.Rendering.LookDev;
using UnityEditor.Experimental.GraphView;
using GSpawn;

public class BossSkillP : MonoBehaviour
{
    public enum BossSkill
    {
        Skill1,
        Skill2,
        Skill3,
        Skill4
    }

    public BoxCollider boxCollider;
    public CapsuleCollider JumpAttackRange;

    public GameObject Target;
    public Animator animator;
    public BossLookAt bossLookAt;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        boxCollider = GetComponent<BoxCollider>();
    }

    public void UseSkill(BossSkill skill)
    {
        //Debug.Log("Skill " + (int)skill + " is used.");

        switch (skill)
        {
            case BossSkill.Skill1:
                //StartCoroutine(BossSkill1());
                StartCoroutine(BossSkill3());
                break;
            case BossSkill.Skill2:
                //StartCoroutine(BossSkill2());
                StartCoroutine(BossSkill3());
                break;
            case BossSkill.Skill3:
                StartCoroutine(BossSkill3());
                break;
            case BossSkill.Skill4:
                //StartCoroutine(BossSkill4());
                StartCoroutine(BossSkill3());
                break;
        }
    }

    IEnumerator BossSkill1()
    {
        // Skill1의 로직을 여기에 작성
        yield return null;
    }

    IEnumerator BossSkill2()
    {
        // Skill2의 로직을 여기에 작성
        yield return null;
    }

    IEnumerator BossSkill3()
    {
        Vector3 jumpStartPosition = transform.position;
        Vector3 jumpEndAttackVec = Target.transform.position;
        StartCoroutine(JumpDuring(jumpStartPosition, jumpEndAttackVec, 1.4f));

        bossLookAt.isLook = false;
        boxCollider.enabled = false;

        animator.SetTrigger("doJumpAttack");

        yield return new WaitForSeconds(3f);

        JumpAttackRange.enabled = true;
        boxCollider.enabled = true;
        animator.SetTrigger("doReturn");
        StartCoroutine(JumpDuring(jumpEndAttackVec, jumpStartPosition, 1.1f));

        yield return new WaitForSeconds(1f);

        JumpAttackRange.enabled = false;
        boxCollider.enabled = false;

        yield return new WaitForSeconds(10f);

        bossLookAt.isLook = true;

        boxCollider.enabled = true;
    }

    IEnumerator JumpDuring(Vector3 startPosition, Vector3 jumpAttackVec, float duration)
    {
        float startTime = Time.time;

        while (Time.time - startTime < duration)
        {
            // Dodge 동작 중에 플레이어를 움직입니다.
            float t = (Time.time - startTime) / duration; // 시간 계산
            transform.position = Vector3.Lerp(startPosition, jumpAttackVec, t); // 시작 지점에서 도착 지점까지 t만큼 시간 소요
            yield return null;
        }
        transform.position = jumpAttackVec;
    }

    IEnumerator BossSkill4()
    {
        // Skill4의 로직을 여기에 작성
        yield return null;
    }
}
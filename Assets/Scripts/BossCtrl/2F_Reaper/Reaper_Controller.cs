using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ReaperState
{
    RaidStart,      // 0
    Idle,           // 1
    Move,           // 2

    BaseAtk_0,      // 3
    BaseAtk_1,      // 4


    Dark_Hand,      // 5
    Dark_Decline,   // 6
}

public class Reaper_Controller : Boss_BehaviorCtrl_Base
{
    [Header("-----Reaper State-----")]
    public ReaperState reaperState;
    public float Skill_Think_Range;
    public float TargetDistance;
    public float LockCloseDistance;

    public GameObject Target;   // 플레이어
    public Reaper_Atk_Range Reaper_AtkRange;
    Vector3 dir; // 각도
    Rigidbody rigid;
    [SerializeField]
    float Boss_RotSpeed;
    [SerializeField]
    float moveSpeed;
    public bool isLock;               // 각도 조절 여부
    public bool isAttacking;          // 공격 중 인지 여부
    public bool CanAttack;

    [Header("-----Animation Var-----")]
    public Animator Reaper_animator;   // 애니메이터

    public bool isMove;         // 이동 여부
    public bool isTargetFind;   // 첫 조우 여부

    [Header("-----Skill Dark Decline-----")]
    public float Dark_Decline_Delay;

    #region Reaper_Rotate
    public override void LookAtPlayer()
    {
        // 플레이어를 찾을 수 없다면 실행 안함
        if (Target == null || isLock)
            return;

        // 플레이어를 바라보도록
        // this.transform.LookAt(Target.transform);

        dir = Target.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(dir, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * Boss_RotSpeed);
    }
    #endregion

    #region Reaper_Move
    public override void Move()
    {
        // 플레이어를 찾을 수 없다면 실행 안함
        if (Target == null)
            return;

        if (isMove && isAttacking == false)
        {
            Reaper_animator.SetBool("isMove", isMove);
            // 앞으로 이동
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
        //else if (!isMove && isAttacking == false)
        //{
        //    reaperState = ReaperState.Idle;
        //    Reaper_animator.SetBool("isMove", isMove);
        //}
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Reaper_animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();

        StartCoroutine(SeePlayer());
    }

    IEnumerator SeePlayer()
    {
        Reaper_animator.SetTrigger("IsFindPlayer");

        yield return new WaitForSeconds(Reaper_animator.GetCurrentAnimatorStateInfo(0).length + 3.0f);

        // 거리에 따라 원거리 공격 근거리 공격
        if (TargetDistance > Skill_Think_Range)
        {
            Reaper_Long_nextAct();
        }
        else if(TargetDistance <= Skill_Think_Range)
        {
            Reaper_Short_nextAct();
        }
    }

    private void FixedUpdate()
    {
        // 현재 상태가 움직이는 Move고 대상이 멀면
        if (reaperState == ReaperState.Move && TargetDistance > Skill_Think_Range)
        {
            Move();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 플레이어가 null이 아니라면
        if (Target != null)
        {
            TargetDistance = Vector3.Distance(Target.transform.position, this.transform.position);
        }

        // 거리가 설정한 거리보다 멀면 회전
        if (TargetDistance <= LockCloseDistance)
        {
            
        }
        else if (TargetDistance > LockCloseDistance)
        {
            LookAtPlayer();
        }  
    }


    #region Reaper_Next_Skill
    void Reaper_Short_nextAct()
    {
        // 현재 공격중이라면 return
        if (isAttacking == true)
        {
            return;
        }

        // 거리가 가까우면
        if (TargetDistance <= Skill_Think_Range)
        {
            // 랜덤 근접 공격
            int randomIndex = Random.Range(0, 3);

            switch (randomIndex)
            {
                case 0:
                    StartCoroutine(BaseAtk_0());
                    break;
                case 1:
                    StartCoroutine(BaseAtk_1());
                    break;
                case 2:
                    StartCoroutine(Dark_Decline());
                    break;
            }
        }
    }

    void Reaper_Long_nextAct()
    {
        // 현재 공격중이라면 return
        if (isAttacking == true)
        {
            return;
        }

        // 거리가 멀면
        if (TargetDistance > Skill_Think_Range)
        {
            // 원거리
            int randomIndex = Random.Range(0, 2);

            switch (randomIndex)
            {
                case 0:
                    // 이동
                    isMove = true;
                    reaperState = ReaperState.Move;
                    break;

                case 1:
                    StartCoroutine(Dark_Hand());
                    break;
            }
        }
    }
    #endregion

    #region Reaper_Atk_Think
    // TODO ## 2층 보스 패턴 적용
    public void Skill_Think()
    {
        if (isAttacking == true)
        {
            return;
        }

        if (TargetDistance <= Skill_Think_Range)
        {
            // 근접
            int randomIndex = Random.Range(0, 3);

            switch (randomIndex)
            {
                case 0:
                    Reaper_animator.SetTrigger("BaseAtk_0");
                    break;
                case 1:
                    Reaper_animator.SetTrigger("BaseAtk_1");
                    break;
                case 2:
                    Reaper_animator.SetTrigger("Dark_Decline");
                    break;      
            }
        }
        else if (TargetDistance > Skill_Think_Range)
        {
            // 원거리
            int randomIndex = Random.Range(0, 2);

            switch (randomIndex)
            {
                case 0:
                    isMove = true;
                    break;

                case 1:
                    
                    Reaper_animator.SetTrigger("Dark_Hand");
                    break;
            }
        }
    }

    #endregion

    #region Reaper_PlayerCheck

    public void PlayerCheck()
    {
        if (TargetDistance < Skill_Think_Range)
        {
            isMove = false;
            Reaper_Short_nextAct();
        }
    }

    #endregion

    #region Boss_Atk_0
    // TODO ## Reaper_BaseAtk_0
    IEnumerator BaseAtk_0()
    {
        // 현재 상태 변경
        reaperState = ReaperState.BaseAtk_0;
        // 애니메이션 실행
        Reaper_animator.SetTrigger("BaseAtk_0");
        // 공격 중
        isAttacking = true;
        // 이동 멈춤
        isMove = false;
        Reaper_animator.SetBool("isMove", isMove);

        yield return new WaitForSeconds(Reaper_animator.GetCurrentAnimatorStateInfo(0).length);
        // Debug.Log(Reaper_animator.GetCurrentAnimatorStateInfo(0).length);

        // 4초 후 공격 가능
        yield return new WaitForSeconds(4.0f);
        isAttacking = false;

        // 거리에 따른 다음 공격
        if (TargetDistance > Skill_Think_Range && isAttacking == false)
        {
            Reaper_Long_nextAct();
        }
        else if (TargetDistance <= Skill_Think_Range && isAttacking == false)
        {
            Reaper_Short_nextAct();
        }
    }
    #endregion

    #region Boss_Atk_1
    // TODO ## Reaper_BaseAtk_1
    IEnumerator BaseAtk_1()
    {
        // 현재 상태 변경
        reaperState = ReaperState.BaseAtk_1;
        // 애니메이션 실행
        Reaper_animator.SetTrigger("BaseAtk_1");
        // 공격 중
        isAttacking = true;
        // 이동 멈춤
        isMove = false;
        Reaper_animator.SetBool("isMove", isMove);

        yield return new WaitForSeconds(Reaper_animator.GetCurrentAnimatorStateInfo(0).length);
        Debug.Log(Reaper_animator.GetCurrentAnimatorStateInfo(0).length);

        // 4초 후 실행
        yield return new WaitForSeconds(4.0f);

        // 공격 가능
        isAttacking = false;

        // 거리에 따른 다음 공격
        if (TargetDistance > Skill_Think_Range && isAttacking == false)
        {
            Reaper_Long_nextAct();
        }
        else if (TargetDistance <= Skill_Think_Range && isAttacking == false)
        {
            Reaper_Short_nextAct();
        }
    }
    #endregion

    #region Boss_Atk_2_DarkDecline
    // TODO ## Reaper_DarkDecline
    IEnumerator Dark_Decline()
    {
        // 현재 상태 변경
        reaperState = ReaperState.Dark_Decline;
        // 공격 가능
        isAttacking = true;
        // 이동 멈춤
        isMove = false;
        Reaper_animator.SetBool("isMove", isMove);
        // 애니메이션 실행_1
        Reaper_animator.SetTrigger("Dark_Decline");

        // Dark_Decline_Delay 후 실행
        yield return new WaitForSeconds(Dark_Decline_Delay);
        // 애니메이션 실행_2
        Reaper_animator.SetTrigger("Dark_Decline");

        // Dark_Decline_Delay 후 실행
        yield return new WaitForSeconds(Dark_Decline_Delay);
        // 애니메이션 실행_3
        Reaper_animator.SetTrigger("Dark_Decline");

        // 4초 후 실행
        yield return new WaitForSeconds(4.0f);

        // 공격 가능
        isAttacking = false;

        // 거리에 따른 다음 공격
        if (TargetDistance > Skill_Think_Range && isAttacking == false)
        {
            Reaper_Long_nextAct();
        }
        else if (TargetDistance <= Skill_Think_Range && isAttacking == false)
        {
            Reaper_Short_nextAct();
        }
    }
    #endregion

    #region Reaper_Atk_3_DarkHand
    // TODO ## Reaper_DarkHand
    IEnumerator Dark_Hand()
    {
        // 상태 변경
        reaperState = ReaperState.Dark_Hand;
        // 애니메이션 작동
        Reaper_animator.SetTrigger("Dark_Hand");

        // 공격 중
        isAttacking = true;

        // 이동 멈춤
        isMove = false;
        Reaper_animator.SetBool("isMove", isMove);


        yield return new WaitForSeconds(Reaper_animator.GetCurrentAnimatorStateInfo(0).length);
        Debug.Log(Reaper_animator.GetCurrentAnimatorStateInfo(0).length);

        // 공격 가능
        isAttacking = false;

        // 2초 후
        yield return new WaitForSeconds(2.0f);

        // 거리에 따른 다음 공격
        if (TargetDistance > Skill_Think_Range && isAttacking == false)
        {
            Reaper_Long_nextAct();
        }
        else if (TargetDistance <= Skill_Think_Range && isAttacking == false)
        {
            Reaper_Short_nextAct();
        }
    }
    #endregion
}

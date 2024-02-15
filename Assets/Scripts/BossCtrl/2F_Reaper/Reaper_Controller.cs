using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ReaperState
{
    RaidStart,      // 0
    Idle,           // 1
    Move,           // 2
    Teleport,
    
    BaseAtk_0,      // 
    BaseAtk_1,      // 

    Dark_Hand,      // 
    Dark_Decline,   // 
}

public class Reaper_Controller : Boss_BehaviorCtrl_Base
{
    [Header("-----Reaper State-----")]
    public ReaperState reaperState;
    public bool isLock;               // 각도 조절 여부
    public bool isAttacking;          // 공격 중 인지 여부

    [Header("-----Reaper Reference-----")]
    public Reaper_Atk_Range Reaper_AtkRange; //  
    public Boss_HP_Controller boss_hp_ctrl;  // HP 컨트롤러

    [Header("-----Reaper Variable-----")]
    public GameObject Target;       // 플레이어
    public float TargetDistance;    // 플레이어와의 거리

    [Header("-----Reaper State Variable-----")]
    public int MaxHP;   // 리퍼 체력

    [SerializeField]
    float Boss_RotSpeed;    //  회전 속도
    [SerializeField]
    float moveSpeed;        // 움직임 속도
    public float Skill_Think_Range; // 스킬 시전 가능 범위


    Vector3 dir; // 각도
    // Rigidbody rigid;

    [Header("-----Animation Var-----")]
    public Animator Reaper_animator;   // 애니메이터
    public bool isMove;         // 이동 여부

    [Header("-----Skill_ BaseAtk_0-----")]
    [SerializeField]
    float BaseAtk_0_LockTime; // 기본 공격_0 회전 제어

    [Header("-----Skill_ BaseAtk_1-----")]
    [SerializeField]
    float BaseAtk_1_LockTime; // // 기본 공격_1 회전 제어

    [Header("-----Skill_Dark_Decline-----")]
    public float Dark_Decline_Delay; // 어둠의 쇠락 스킬 모션 딜레이
    [SerializeField]
    float Decline_LockTime; // 회전 제한 시간
    [SerializeField]
    float Decline_UnLockTime; // 회전 제한 해제 시간

    #region Reaper_Rotate
    public override void LookAtPlayer()
    {
        // 플레이어를 찾을 수 없다면 실행 안함
        if (Target == null || isLock)
            return;

        // 플레이어를 바라보도록
        // this.transform.LookAt(Target.transform);

        dir = Target.transform.position - transform.position;
        // y축 정보 제거
        dir.y = 0.0f;

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
        boss_hp_ctrl = GetComponent<Boss_HP_Controller>();
        Reaper_animator = GetComponent<Animator>();
        // rigid = GetComponent<Rigidbody>();

        // 최대 체력 전달 현재 체력 
        boss_hp_ctrl.BossMaxHP = MaxHP;  
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

        LookAtPlayer();
    }

    #region Reaper_Next_Skill
    public void Reaper_Short_nextAct()
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

    public void Reaper_Long_nextAct()
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
            int randomIndex = Random.Range(0, 3);

            switch (randomIndex)
            {
                case 0:
                    // 이동
                    isMove = true;
                    reaperState = ReaperState.Move;
                    break;

                case 1:
                    StartCoroutine(Teleport());
                    break;

                case 2:
                    StartCoroutine(Dark_Hand());
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

    #region Reaper_Idle
    public void Reaper_Idle()
    {
        reaperState = ReaperState.Idle;
    }
    #endregion

    #region Boss_Reaper_Teleport
    IEnumerator Teleport()
    {
        reaperState = ReaperState.Teleport;
        Reaper_animator.SetTrigger("Teleport");


        // 플레이어 주위에서 랜덤한 각도와 거리로 텔레포트
        float randomAngle = Random.Range(0f, 360f);

        // 플레이어의 위치에서 오일러 각 만큼 위치에서 * (Skill_Think_Range - 3.0f)만큼 거리에 위치
        Vector3 randomDirection = Quaternion.Euler(0f, randomAngle, 0f) * Vector3.forward;
        Vector3 randomPosition = Target.transform.position + randomDirection * (Skill_Think_Range - 5.0f);

        // Y 좌표는 0으로 고정
        randomPosition.y = 1.5f;

        // 텔레포트
        transform.position = randomPosition;

        yield return new WaitForSeconds(0.1f);

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

        yield return new WaitForSeconds(BaseAtk_0_LockTime);
        isLock = true;

        yield return new WaitForSeconds(Reaper_animator.GetCurrentAnimatorStateInfo(0).length - BaseAtk_0_LockTime);
        // Debug.Log(Reaper_animator.GetCurrentAnimatorStateInfo(0).length);
        isLock = false;

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

        yield return new WaitForSeconds(BaseAtk_1_LockTime);
        isLock = true;

        yield return new WaitForSeconds(Reaper_animator.GetCurrentAnimatorStateInfo(0).length - BaseAtk_1_LockTime);
        Debug.Log(Reaper_animator.GetCurrentAnimatorStateInfo(0).length);
        isLock = false;

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

        yield return new WaitForSeconds(Decline_LockTime);
        isLock = true;

        yield return new WaitForSeconds(Decline_UnLockTime);
        isLock = false;

        // Dark_Decline_Delay 후 실행
        yield return new WaitForSeconds(Dark_Decline_Delay - (Decline_LockTime + Decline_UnLockTime));
        // 애니메이션 실행_2
        Reaper_animator.SetTrigger("Dark_Decline");

        yield return new WaitForSeconds(Decline_LockTime);
        isLock = true;

        yield return new WaitForSeconds(Decline_UnLockTime);
        isLock = false;

        // Dark_Decline_Delay 후 실행
        yield return new WaitForSeconds(Dark_Decline_Delay - (Decline_LockTime + Decline_UnLockTime));

        // 애니메이션 실행_3
        Reaper_animator.SetTrigger("Dark_Decline");

        yield return new WaitForSeconds(Decline_LockTime);
        isLock = true;

        yield return new WaitForSeconds(Decline_UnLockTime);
        isLock = false;

        // Dark_Decline_Delay 후 실행
        yield return new WaitForSeconds(Dark_Decline_Delay - (Decline_LockTime + Decline_UnLockTime));

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
        isLock = true;

        // 이동 멈춤
        isMove = false;
        Reaper_animator.SetBool("isMove", isMove);

        // 애니메이션이 끝나고 난 뒤
        yield return new WaitForSeconds(Reaper_animator.GetCurrentAnimatorStateInfo(0).length);
        Debug.Log(Reaper_animator.GetCurrentAnimatorStateInfo(0).length);

        // 2초 후
        yield return new WaitForSeconds(2.0f);
        // 공격 가능
        isAttacking = false;
        isLock = false;

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

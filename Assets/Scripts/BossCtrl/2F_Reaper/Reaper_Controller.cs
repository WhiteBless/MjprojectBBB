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
    Dark_Soul,
}

public enum Reaper_Awake
{
    NORMAL,
    AWAKENING,
}

public class Reaper_Controller : Boss_BehaviorCtrl_Base
{
    [Header("-----Reaper State-----")]
    public ReaperState reaperState;
    public Reaper_Awake reaperAwakeState;
    public bool isLock;               // 각도 조절 여부
    public bool isAttacking;          // 공격 중 인지 여부

    [Header("-----Reaper Reference-----")]
    public Reaper_Atk_Range Reaper_AtkRange; //  
    public Boss_HP_Controller boss_hp_ctrl;  // HP 컨트롤러
    [SerializeField]
    Reaper_ObjPool reaper_ObjPoolRef;

    [Header("-----Reaper Variable-----")]
    public GameObject Target;       // 플레이어
    public float TargetDistance;    // 플레이어와의 거리
    [SerializeField]
    GameObject DeathSycthe;

    [Header("-----Reaper State Variable-----")]
    public int MaxHP;   // 리퍼 체력

    [SerializeField]
    float Boss_RotSpeed;    //  회전 속도
    [SerializeField]
    float moveSpeed;        // 움직임 속도
    public float Skill_Think_Range; // 스킬 시전 가능 범위
    [SerializeField]
    GameObject Skill_Pos; // 스킬 생성 위치
    [SerializeField]
    GameObject Skill_Look; // 스킬이 바라보는 방향
    Vector3 dir; // 각도
    // Rigidbody rigid;

    [Header("-----Animation Var-----")]
    public Animator Reaper_animator;   // 애니메이터
    public bool isMove;         // 이동 여부
    [SerializeField]
    float nextActTime;

    [Header("-----Awakening-----")]
    [SerializeField]
    GameObject Aura;

    [Header("-----Skill_ BaseAtk_0-----")]
    [SerializeField]
    float BaseAtk_0_LockTime; // 기본 공격_0 회전 제어
    [SerializeField]
    GameObject BaseAtk_0_Eff;

    [Header("-----Skill_ BaseAtk_1-----")]
    [SerializeField]
    float BaseAtk_1_LockTime; // // 기본 공격_1 회전 제어
    [SerializeField]
    GameObject BaseAtk_1_Eff;

    [Header("-----Skill_Dark_Decline-----")]
    public float Dark_Decline_Delay; // 어둠의 쇠락 스킬 모션 딜레이
    [SerializeField]
    GameObject Dark_Decline_Slash;
    [SerializeField]
    float DarkDecline_Rot;
    [SerializeField]
    float DarkDecline_Dis;  // 어둠의 쇠락 생성 거리
    [SerializeField]
    float Decline_LockTime; // 회전 제한 시간
    [SerializeField]
    float Decline_UnLockTime; // 회전 제한 해제 시간

    [Header("-----Skill_Dark_Hand-----")]
    [SerializeField]
    GameObject CastingEff;

    [Header("-----Skill_Dark_Soul-----")]
    [SerializeField]
    GameObject DarkSoul_Skill_Eff;
    [SerializeField]
    float Slow_RotSpeed;
    [SerializeField]
    float DarkSoul_Running_Time;

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
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * (Boss_RotSpeed - Slow_RotSpeed));
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

    #region Start/Update
    // Start is called before the first frame update
    void Start()
    {
        boss_hp_ctrl = GetComponent<Boss_HP_Controller>();
        reaper_ObjPoolRef = GetComponent<Reaper_ObjPool>();
        Reaper_animator = GetComponent<Animator>();
        reaperAwakeState = Reaper_Awake.NORMAL;

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
    #endregion

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
            if (reaperAwakeState == Reaper_Awake.NORMAL)
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
            else if (reaperAwakeState == Reaper_Awake.AWAKENING)
            {
                // 랜덤 근접 공격
                int randomIndex = Random.Range(0, 4);

                switch (3)
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
                    case 3:
                        StartCoroutine(Dark_Soul());
                        break;
                }
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

    #region Reaper Equip / UnEquip Scythe

    public void Equip_Scythe()
    {
        DeathSycthe.SetActive(true);
    }

    public void UnEquip_Scythe()
    {
        DeathSycthe.SetActive(false);
    }

    #endregion

    #region Reaper_Idle
    public void Reaper_Idle()
    {
        // reaperState = ReaperState.Idle;
    }
    #endregion

    #region Awakening
    public void Aura_On()
    {
        Aura.SetActive(true);
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
        yield return new WaitForSeconds(nextActTime);
        isAttacking = false;

        // 각성
        if (boss_hp_ctrl.isAwakening == true && reaperAwakeState == Reaper_Awake.NORMAL)
        {
            // 각성 상태
            reaperAwakeState = Reaper_Awake.AWAKENING;
            Reaper_animator.SetTrigger("Awakening");
            nextActTime = 1.0f;
            // 다음행동 시간 + 1
            yield return new WaitForSeconds(nextActTime + 1.0f);
        }


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
    // 기본 공격 슬래쉬 이펙트 적용
    public void BaseAtk0_Eff()
    {
        StartCoroutine(Play_BaseAtk0_Eff());
    }

    IEnumerator Play_BaseAtk0_Eff()
    {
        BaseAtk_0_Eff.SetActive(true);

        yield return new WaitForSeconds(2.0f);

        BaseAtk_0_Eff.SetActive(false);
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
        yield return new WaitForSeconds(nextActTime);

        // 공격 가능
        isAttacking = false;

        // 각성
        if (boss_hp_ctrl.isAwakening == true && reaperAwakeState == Reaper_Awake.NORMAL)
        {
            // 각성 상태
            reaperAwakeState = Reaper_Awake.AWAKENING;
            Reaper_animator.SetTrigger("Awakening");
            nextActTime = 1.0f;
           
            // 다음행동 시간 + 1
            yield return new WaitForSeconds(nextActTime + 1.0f);
        }


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

    // 기본 공격 슬래쉬 이펙트 적용
    public void BaseAtk1_Eff()
    {
        StartCoroutine(Play_BaseAtk1_Eff());
    }

    IEnumerator Play_BaseAtk1_Eff()
    {
        BaseAtk_1_Eff.SetActive(true);

        yield return new WaitForSeconds(2.0f);

        BaseAtk_1_Eff.SetActive(false);
    }
    #endregion

    #region Boss_Atk_2_DarkDecline
    // TODO ## Reaper_DarkDecline
    IEnumerator Dark_Decline()
    {
        // 각성 전 어둠의 쇠락
        if (reaperAwakeState == Reaper_Awake.NORMAL)
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
            // 오브젝트 풀에서 이펙트 위치 조정시켜 생성
            GameObject DarkDeclineEff_1 = reaper_ObjPoolRef.GetDarkDeclineFromPool();
            DarkDeclineEff_1.transform.position = Skill_Pos.transform.position + Skill_Pos.transform.forward * DarkDecline_Dis;

            Vector3 d1 = DarkDeclineEff_1.transform.position - Skill_Look.transform.position;
            d1.y = 0.0f;
            Quaternion q1 = Quaternion.LookRotation(d1);
            DarkDeclineEff_1.transform.rotation = q1 * Quaternion.Euler(0f, 90f, 0f);

            yield return new WaitForSeconds(Decline_UnLockTime);
            isLock = false;

            // 오브젝트 풀로 비활성화
            //DarkDeclineEff_1.SetActive(false);

            // Dark_Decline_Delay 후 실행
            yield return new WaitForSeconds(Dark_Decline_Delay - (Decline_LockTime + Decline_UnLockTime));
            // 애니메이션 실행_2
            Reaper_animator.SetTrigger("Dark_Decline");

            yield return new WaitForSeconds(Decline_LockTime);
            isLock = true;
            // 오브젝트 풀에서 이펙트 위치 조정시켜 생성
            GameObject DarkDeclineEff_2 = reaper_ObjPoolRef.GetDarkDeclineFromPool();
            // DarkDeclineEff_2.transform.forward = Vector3.right;
            DarkDeclineEff_2.transform.position = Skill_Pos.transform.position + Skill_Pos.transform.forward * DarkDecline_Dis;

            Vector3 d2 = DarkDeclineEff_2.transform.position - Skill_Look.transform.position;
            d2.y = 0.0f;
            Quaternion q2 = Quaternion.LookRotation(d2);
            DarkDeclineEff_2.transform.rotation = q2 * Quaternion.Euler(0f, 90f, 0f);

            yield return new WaitForSeconds(Decline_UnLockTime);
            isLock = false;


            // Dark_Decline_Delay 후 실행
            yield return new WaitForSeconds(Dark_Decline_Delay - (Decline_LockTime + Decline_UnLockTime));

            // 애니메이션 실행_3
            Reaper_animator.SetTrigger("Dark_Decline");

            yield return new WaitForSeconds(Decline_LockTime);
            isLock = true;
            // 오브젝트 풀에서 이펙트 위치 조정시켜 생성
            GameObject DarkDeclineEff_3 = reaper_ObjPoolRef.GetDarkDeclineFromPool();
            //DarkDeclineEff_3.transform.forward = Vector3.right;
            DarkDeclineEff_3.transform.position = Skill_Pos.transform.position + Skill_Pos.transform.forward * DarkDecline_Dis;
            Vector3 d3 = DarkDeclineEff_3.transform.position - Skill_Look.transform.position;
            d3.y = 0.0f;
            Quaternion q3 = Quaternion.LookRotation(d3);
            DarkDeclineEff_3.transform.rotation = q3 * Quaternion.Euler(0f, 90f, 0f);

            yield return new WaitForSeconds(Decline_UnLockTime);
            isLock = false;

            // Dark_Decline_Delay 후 실행
            yield return new WaitForSeconds(Dark_Decline_Delay - (Decline_LockTime + Decline_UnLockTime));

            // 4초 후 실행
            yield return new WaitForSeconds(nextActTime);
            DarkDeclineEff_1.SetActive(false);
            DarkDeclineEff_2.SetActive(false);
            DarkDeclineEff_3.SetActive(false);

            // 공격 가능
            isAttacking = false;

            // 각성
            if (boss_hp_ctrl.isAwakening == true && reaperAwakeState == Reaper_Awake.NORMAL)
            {
                // 각성 상태
                reaperAwakeState = Reaper_Awake.AWAKENING;
                Reaper_animator.SetTrigger("Awakening");
                nextActTime = 1.0f;
                // 다음행동 시간 + 1
                yield return new WaitForSeconds(nextActTime + 1.0f);
            }

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
        else if (reaperAwakeState == Reaper_Awake.AWAKENING) // 어둠의 쇠락 각성 후---------------------------------------
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
            // 오브젝트 풀에서 이펙트 위치 조정시켜 생성
            GameObject DarkDeclineEff_1 = reaper_ObjPoolRef.GetDarkDecline2FromPool();
            DarkDeclineEff_1.transform.position = Skill_Pos.transform.position + Skill_Pos.transform.forward * DarkDecline_Dis;

            Vector3 d1 = DarkDeclineEff_1.transform.position - Skill_Look.transform.position;
            d1.y = 0.0f;
            Quaternion q1 = Quaternion.LookRotation(d1);
            DarkDeclineEff_1.transform.rotation = q1 * Quaternion.Euler(0f, 90f, 0f);

            yield return new WaitForSeconds(Decline_UnLockTime);
            isLock = false;

            // 오브젝트 풀로 비활성화
            //DarkDeclineEff_1.SetActive(false);

            // Dark_Decline_Delay 후 실행
            yield return new WaitForSeconds(Dark_Decline_Delay - (Decline_LockTime + Decline_UnLockTime));
            // 애니메이션 실행_2
            Reaper_animator.SetTrigger("Dark_Decline");

            yield return new WaitForSeconds(Decline_LockTime);
            isLock = true;
            // 오브젝트 풀에서 이펙트 위치 조정시켜 생성
            GameObject DarkDeclineEff_2 = reaper_ObjPoolRef.GetDarkDecline2FromPool();
            // DarkDeclineEff_2.transform.forward = Vector3.right;
            DarkDeclineEff_2.transform.position = Skill_Pos.transform.position + Skill_Pos.transform.forward * DarkDecline_Dis;

            Vector3 d2 = DarkDeclineEff_2.transform.position - Skill_Look.transform.position;
            d2.y = 0.0f;
            Quaternion q2 = Quaternion.LookRotation(d2);
            DarkDeclineEff_2.transform.rotation = q2 * Quaternion.Euler(0f, 90f, 0f);

            yield return new WaitForSeconds(Decline_UnLockTime);
            isLock = false;


            // Dark_Decline_Delay 후 실행
            yield return new WaitForSeconds(Dark_Decline_Delay - (Decline_LockTime + Decline_UnLockTime));

            // 애니메이션 실행_3
            Reaper_animator.SetTrigger("Dark_Decline");

            yield return new WaitForSeconds(Decline_LockTime);
            isLock = true;
            // 오브젝트 풀에서 이펙트 위치 조정시켜 생성
            GameObject DarkDeclineEff_3 = reaper_ObjPoolRef.GetDarkDecline2FromPool();
            //DarkDeclineEff_3.transform.forward = Vector3.right;
            DarkDeclineEff_3.transform.position = Skill_Pos.transform.position + Skill_Pos.transform.forward * DarkDecline_Dis;
            Vector3 d3 = DarkDeclineEff_3.transform.position - Skill_Look.transform.position;
            d3.y = 0.0f;
            Quaternion q3 = Quaternion.LookRotation(d3);
            DarkDeclineEff_3.transform.rotation = q3 * Quaternion.Euler(0f, 90f, 0f);

            yield return new WaitForSeconds(Decline_UnLockTime);
            isLock = false;

            // Dark_Decline_Delay 후 실행
            yield return new WaitForSeconds(Dark_Decline_Delay - (Decline_LockTime + Decline_UnLockTime));

            // 4초 후 실행
            yield return new WaitForSeconds(nextActTime);
            DarkDeclineEff_1.SetActive(false);
            DarkDeclineEff_2.SetActive(false);
            DarkDeclineEff_3.SetActive(false);

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
    }
    // 슬래쉬 이펙트 생성
    public void Dark_Decline_Eff()
    {
        StartCoroutine(Play_Dark_Decline_Eff());
    }

    IEnumerator Play_Dark_Decline_Eff()
    {
        Dark_Decline_Slash.SetActive(true);

        yield return new WaitForSeconds(2.0f);

        Dark_Decline_Slash.SetActive(false);
    }
    #endregion

    #region Reaper_Atk_3_DarkHand
    // TODO ## Reaper_DarkHand / Reaper_DarkHand2
    IEnumerator Dark_Hand()
    {
        // 어둠의 손짓 각성 전 
        if (reaperAwakeState == Reaper_Awake.NORMAL)
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
            // Debug.Log(Reaper_animator.GetCurrentAnimatorStateInfo(0).length);

            // 2초 후
            yield return new WaitForSeconds(2.0f);
            // 공격 가능
            isAttacking = false;
            isLock = false;

            // 각성
            if (boss_hp_ctrl.isAwakening == true && reaperAwakeState == Reaper_Awake.NORMAL)
            {
                // 각성 상태
                reaperAwakeState = Reaper_Awake.AWAKENING;
                Reaper_animator.SetTrigger("Awakening");
                nextActTime = 1.0f;
                // 다음행동 시간 + 1
                yield return new WaitForSeconds(nextActTime + 1.0f);
            }

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
        else // 어둠의 손짓 각성 후
        {
            // 상태 변경
            reaperState = ReaperState.Dark_Hand;
            // 애니메이션 작동
            Reaper_animator.SetTrigger("UnEquip_Scythe");
            // 공격 중
            isAttacking = true;
            isLock = true;
            // 이동 멈춤
            isMove = false;
            Reaper_animator.SetBool("isMove", isMove);
            // 애니메이션이 끝나고 난 뒤
            yield return new WaitForSeconds(2.0f);

            // 애니메이션 작동
            Reaper_animator.SetTrigger("Casting");
            // 애니메이션이 끝나고 난 뒤
            yield return new WaitForSeconds(Reaper_animator.GetCurrentAnimatorStateInfo(0).length + 2.0f);
            isLock = false;
            // 애니메이션 작동
            Reaper_animator.SetTrigger("Dark_Hand_2");
            // 애니메이션이 끝나고 난 뒤
            yield return new WaitForSeconds(4.0f);

            // 애니메이션 작동
            Reaper_animator.SetTrigger("Dark_Hand_2");
            // 애니메이션이 끝나고 난 뒤
            yield return new WaitForSeconds(4.0f);

            isLock = true;
            // 애니메이션 작동
            Reaper_animator.SetTrigger("Equip_Scythe");

            // 애니메이션이 끝나고 난 뒤
            yield return new WaitForSeconds(Reaper_animator.GetCurrentAnimatorStateInfo(0).length + nextActTime);

            isLock = false;
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
    }

    // TODO ## Reaper DarkHand 이펙트 생성
    public void DarkHand_Eff()
    {
        StartCoroutine(Play_DarkHand_Eff());
    }

    IEnumerator Play_DarkHand_Eff()
    {
        Vector3 Pos = Target.transform.position;

        yield return new WaitForSeconds(1.5f);

        // 오브젝트 풀에서 이펙트 위치 조정시켜 생성
        GameObject DarkHnad_Explosion = reaper_ObjPoolRef.GetDarkHandFromPool();
        DarkHnad_Explosion.transform.position = Pos;

        yield return new WaitForSeconds(5.0f);
        DarkHnad_Explosion.SetActive(false);
    }

    // 캐스팅 이펙트 
    public void Casting_Eff()
    {
        StartCoroutine(Play_Casting_Eff());
    }
    IEnumerator Play_Casting_Eff()
    {
        CastingEff.SetActive(true);
        CastingEff.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        yield return new WaitForSeconds(5.0f);

        CastingEff.SetActive(false);
    }

    // 폭발이펙트 생성
    // TODO ## Reaper DarkHand2 이펙트 생성
    public void DarkHand2_Eff()
    {
        StartCoroutine(Play_DarkHand2_Eff());
    }

    IEnumerator Play_DarkHand2_Eff()
    {
        Vector3 Pos = Target.transform.position;

        yield return new WaitForSeconds(0.5f);

        // 오브젝트 풀에서 이펙트 위치 조정시켜 생성
        GameObject DarkHnad2_Explosion = reaper_ObjPoolRef.GetDarkHand2FromPool();
        DarkHnad2_Explosion.transform.GetChild(2).gameObject.SetActive(true);

        DarkHnad2_Explosion.transform.position = Pos;

        yield return new WaitForSeconds(10.0f);
        // 원 위치
        DarkHnad2_Explosion.transform.GetChild(2).GetChild(0).transform.localPosition = Vector3.zero;
        DarkHnad2_Explosion.transform.GetChild(2).GetChild(1).transform.localPosition = Vector3.zero;
        DarkHnad2_Explosion.SetActive(false);
    }

    #endregion

    #region Reaper_Atk_4_DarkSoul
    // TODO ## Reaper_DarkSoul
    IEnumerator Dark_Soul()
    {
        // 현재 상태 변경
        reaperState = ReaperState.Dark_Soul;
        // 공격 가능
        isAttacking = true;
        // 이동 멈춤
        isMove = false;
        Reaper_animator.SetBool("isMove", isMove);
        // 애니메이션 실행_1
        Reaper_animator.SetTrigger("Dark_Soul");
        Slow_RotSpeed = 6.0f;

        yield return new WaitForSeconds(DarkSoul_Running_Time);
        // 공격 가능
        isAttacking = false;
        Slow_RotSpeed = 0.0f;

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

    public void DarkSoul_Eff()
    {
        StartCoroutine(Play_DarkSoul_Eff());
    }

    IEnumerator Play_DarkSoul_Eff()
    {
        Reaper_animator.SetFloat("DarkSoulSpeed", 0.2f);
        DarkSoul_Skill_Eff.SetActive(true);
        

        yield return new WaitForSeconds(DarkSoul_Running_Time - 4.0f);
        Reaper_animator.SetFloat("DarkSoulSpeed", 1.0f);

        
        yield return new WaitForSeconds(2.0f);
        DarkSoul_Skill_Eff.SetActive(false);
    }

    #endregion
}

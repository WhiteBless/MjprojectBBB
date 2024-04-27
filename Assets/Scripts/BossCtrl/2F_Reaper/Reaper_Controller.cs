using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
public enum ReaperState
{
    RaidStart,      // 0
    Idle,           // 1
    Move,           // 2
    Teleport,       // 3
    
    BaseAtk_0,      // 4
    BaseAtk_1,      // 5

    Dark_Hand,      // 6
    Dark_Decline,   // 7
    Dark_Soul,      // 8
    Dark_Ball,      // 9

    Dark_Token,
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
    [SerializeField]
    int NextSkillNum;

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
    [SerializeField]
    GameObject BaseAtk_Collider;
    [SerializeField]
    GameObject BaseAtk_GuideLine;

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
    [SerializeField]
    GameObject Dark_Decline_Box_Collider; // 박스 콜라이더
    [SerializeField]
    GameObject Dark_Decline_Circle_Collider; // 써클 콜라이더
    [SerializeField]
    GameObject Dark_Decline_GuideLine;

    [Header("-----Skill_Dark_Hand-----")]
    [SerializeField]
    GameObject CastingEff;
    [SerializeField]
    GameObject DarkHand_GuideLine;
    [SerializeField]
    GameObject DarkHand2_GuideLine;

    [Header("-----Skill_Dark_Soul-----")]
    [SerializeField]
    GameObject DarkSoul_Skill_Eff;
    [SerializeField]
    float Slow_RotSpeed;
    [SerializeField]
    float DarkSoul_Running_Time;
    [SerializeField]
    GameObject DarkSoul_Collider;
    [SerializeField]
    GameObject DarkSoul_GuideLine;

    [Header("-----Skill_Dark_Ball-----")]
    [SerializeField]
    Transform Center_Tr;                    // 보스 위치 시킬 센터값
    [SerializeField]
    Transform DarkBall_Pos;                 // 구체 생설 시킬 위치
    [SerializeField]
    GameObject Pattern_Pillar_Normal;       // 노말 패턴 기둥
    [SerializeField]    
    GameObject Pattern_Pillar_Awakening;    // 각성 후 패턴 기둥
    [SerializeField]
    float DarkBall_Delay;                   // 공 나오는 딜레이 시간
    [SerializeField]
    float Finish_DarkBall;                  // 마지막 공 나오는 시간
    [SerializeField]
    GameObject[] DarkBall_Pilar;            // 어둠의 구체 배열
    [SerializeField]
    GameObject[] DarkBall_Pilar_Awakening;  // 어둠의 구체 각성 후 기둥 배열
    [SerializeField]
    GameObject[] DarkBall_Awakening;        // 어둠의 구체 각성후 배열(색 있는 구체)
    [SerializeField]
    GameObject DarkBall_Soul_Eff;           // 각성 후 구체와 같이 나오는 어둠의 영혼(보라) 
    [SerializeField]
    int Awakening_Ball_Index;               // 각성 후 구체의 인덱스 값
    [SerializeField]
    float DarkBall_Razer_Time;              // 어둠의 구체 중 영혼 시간

    [Header("-----Skill_Dark_Token-----")]
    [SerializeField]
    bool[] DarkToken_END;
    [SerializeField]
    int Use_SpAtk_Count;
    [SerializeField]
    GameObject Flooring_Effect;
    [SerializeField]
    GameObject[] Token_obj;
    [SerializeField]
    GameObject[] Token_GuideLine;
    [SerializeField]
    float Token_Delay;


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
    public void Reaper_Short_nextAct(int _skillnum)
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
                // 사용했던 스킬 연속 사용 금지
                while(true)
                {
                    // 랜덤 근접 공격
                    int randomIndex = Random.Range(0, 3);

                    if (randomIndex != _skillnum)
                    {
                        NextSkillNum = randomIndex;
                        break;
                    }
                }
                

                switch (NextSkillNum)
                {
                    case 0: // 기본 공격
                        StartCoroutine(BaseAtk_0());
                        break;
                    case 1: // 기본공격
                        StartCoroutine(BaseAtk_1());
                        break;
                    case 2: // 어둠의 쇠락
                        StartCoroutine(Dark_Decline());
                        break;

                }
            }
            else if (reaperAwakeState == Reaper_Awake.AWAKENING)
            {

                while (true)
                {
                    // 랜덤 근접 공격
                    int randomIndex = Random.Range(0, 4);

                    if (randomIndex != _skillnum)
                    {
                        NextSkillNum = randomIndex;
                        break;
                    }
                }

                switch (NextSkillNum)
                {
                    case 0: // 기본 공격
                        StartCoroutine(BaseAtk_0());
                        break;
                    case 1: // 기본 공격
                        StartCoroutine(BaseAtk_1());
                        break;
                    case 2: // 어둠의 쇠락
                        StartCoroutine(Dark_Decline());
                        break;
                    case 3: // 어둠의 영혼
                        StartCoroutine(Dark_Soul());
                        break;
                }
            }       
        }
    }

    public void Reaper_Long_nextAct(int _skillnum)
    {
        // 현재 공격중이라면 return
        if (isAttacking == true)
        {
            return;
        }

        // 거리가 멀면
        if (TargetDistance > Skill_Think_Range)
        {

            if (reaperAwakeState == Reaper_Awake.NORMAL)
            {
                // 사용했던 스킬 연속 사용 금지
                while (true)
                {
                    // 랜덤 근접 공격
                    int randomIndex = Random.Range(0, 4);

                    if (randomIndex != _skillnum)
                    {
                        NextSkillNum = randomIndex;
                        break;
                    }
                }

                switch (NextSkillNum)
                {
                    case 0:
                        StartCoroutine(BossMove());
                        break;
                    case 1:
                         StartCoroutine(Teleport());
                        break;
                    case 2:
                        StartCoroutine(Dark_Hand());
                        break;
                    case 3: // 어둠의 구체
                        StartCoroutine(Dark_Ball());
                        break;
                }
            }
            else if (reaperAwakeState == Reaper_Awake.AWAKENING)
            {
                // 사용했던 스킬 연속 사용 금지
                while (true)
                {
                    // 랜덤 근접 공격
                    int randomIndex = Random.Range(0, 5);

                    if (randomIndex != _skillnum)
                    {
                        NextSkillNum = randomIndex;
                        break;
                    }
                }

                switch (NextSkillNum)
                {
                    case 0:
                        StartCoroutine(BossMove());
                        break;
                    case 1:
                        StartCoroutine(Teleport());
                        break;
                    case 2:
                        StartCoroutine(Dark_Hand());
                        break;
                    case 3:
                        StartCoroutine(Dark_Hand2());
                        break;
                    case 4: // 어둠의 구체
                        StartCoroutine(Dark_Ball());
                        break;
                }
            }
        }
    }

    public void Reaper_Special_nextAct()
    {
        // 현재 공격중이라면 return
        if (isAttacking == true)
        {
            return;
        }

        // 어둠의 증표 시작
        StartCoroutine(Dark_Token());

        // Use_SpAtk_Count 횟수 째 사용 true 
        DarkToken_END[Use_SpAtk_Count] = true;
        // 사용 횟수 카운트
        Use_SpAtk_Count++;
        // Debug.Log(Use_SpAtk_Count);
    }
    #endregion

    #region Reaper_PlayerCheck

    public void PlayerCheck()
    {
        if (TargetDistance < Skill_Think_Range)
        {
            isMove = false;
            StopAllCoroutines();
            Reaper_Short_nextAct(Random.Range(0, 3));
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

    #region ReaperDeath
    public void Death()
    {
        StopAllCoroutines();
        isLock = true;
    }
    #endregion

    #region Boss_Reaper_Teleport
    IEnumerator Teleport()
    {
        reaperState = ReaperState.Teleport;
        Reaper_animator.SetTrigger("Teleport");
        // 이동 멈춤
        isMove = false;
        Reaper_animator.SetBool("isMove", isMove);
        // 플레이어 주위에서 랜덤한 각도와 거리로 텔레포트
        float randomAngle = Random.Range(0f, 360f);

        // 플레이어의 위치에서 오일러 각 만큼 위치에서 * (Skill_Think_Range - 3.0f)만큼 거리에 위치
        Vector3 randomDirection = Quaternion.Euler(0f, randomAngle, 0f) * Vector3.forward;
        Vector3 randomPosition = Target.transform.position + randomDirection * (Skill_Think_Range - 7.0f);

        // Y 좌표는 0으로 고정
        randomPosition.y = 1.5f;

        // 텔레포트
        transform.position = randomPosition;

        yield return new WaitForSeconds(0.5f);

        // 거리에 따른 다음 공격
        if (TargetDistance > Skill_Think_Range && isAttacking == false)
        {
            Reaper_Long_nextAct(1);
        }
        else if (TargetDistance <= Skill_Think_Range && isAttacking == false)
        {
            Reaper_Short_nextAct(10);
        }
    }

    #endregion

    #region Boss_Move
    IEnumerator BossMove()
    {
        yield return new WaitForSeconds(1.4f);
        // 이동
        isMove = true;
        reaperState = ReaperState.Move;
    }
    #endregion

    #region Boss_Awake
    void AwakeBoss()
    {
        // 각성 상태
        reaperAwakeState = Reaper_Awake.AWAKENING;
        Reaper_animator.SetTrigger("Awakening");
        nextActTime = 1.0f;
        // 각성 기둥 활성화
        Pattern_Pillar_Awakening.SetActive(true);
        // 일반 기둥 비활성화
        Pattern_Pillar_Normal.SetActive(false);
    }
    #endregion

    #region Boss_Atk_0
    // TODO ## Reaper_BaseAtk_0
    IEnumerator BaseAtk_0()
    {
        if (boss_hp_ctrl.isDead == true)
        {
            yield break;
        }

        BaseAtk_GuideLine.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        BaseAtk_GuideLine.SetActive(false);

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


        // 4초 후 공격 가능
        yield return new WaitForSeconds(nextActTime);
        isAttacking = false;

        // 각성
        if (boss_hp_ctrl.isAwakening == true && reaperAwakeState == Reaper_Awake.NORMAL)
        {
            AwakeBoss();
            yield return new WaitForSeconds(nextActTime + 1.0f);
        }


        // 광역 공격 가능 여부 체크
        if (boss_hp_ctrl.isReaper_SP_ATK_1 == true && !DarkToken_END[0])
        {
            Reaper_Special_nextAct();
            yield break;
        }
        else if (boss_hp_ctrl.isReaper_SP_ATK_2 == true && !DarkToken_END[1])
        {
            Reaper_Special_nextAct();
            yield break;
        }
        else if (boss_hp_ctrl.isReaper_SP_ATK_3 == true && !DarkToken_END[2])
        {
            Reaper_Special_nextAct();
            yield break;
        }

        // 거리에 따른 다음 공격
        if (TargetDistance > Skill_Think_Range && isAttacking == false)
        {
            Reaper_Long_nextAct(0);
        }
        else if (TargetDistance <= Skill_Think_Range && isAttacking == false)
        {
            Reaper_Short_nextAct(0);
        }
    }
    // 기본 공격 슬래쉬 이펙트 적용
    public void BaseAtk0_Eff()
    {
        StartCoroutine(Play_BaseAtk0_Eff());
    }

    IEnumerator Play_BaseAtk0_Eff()
    {
        // 이펙트 피격범위 생성
        BaseAtk_0_Eff.SetActive(true);
        BaseAtk_Collider.SetActive(true);

        // 피격범위 끄기
        yield return new WaitForSeconds(0.1f);
        BaseAtk_Collider.SetActive(false);

        // 회전 제한
        yield return new WaitForSeconds(1.9f);
        isLock = false;
        BaseAtk_0_Eff.SetActive(false);
    }

    #endregion

    #region Boss_Atk_1
    // TODO ## Reaper_BaseAtk_1
    IEnumerator BaseAtk_1()
    {
        if (boss_hp_ctrl.isDead == true)
        {
            yield break;
        }

        BaseAtk_GuideLine.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        BaseAtk_GuideLine.SetActive(false);

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

        // 4초 후 실행
        yield return new WaitForSeconds(nextActTime);
        // 공격 가능
        isAttacking = false;
        // 각성
        if (boss_hp_ctrl.isAwakening == true && reaperAwakeState == Reaper_Awake.NORMAL)
        {
            AwakeBoss();
            // 다음행동 시간 + 1
            yield return new WaitForSeconds(nextActTime + 1.0f);
        }

        // 광역 공격 가능 여부 체크
        if (boss_hp_ctrl.isReaper_SP_ATK_1 == true && !DarkToken_END[0])
        {
            Reaper_Special_nextAct();
            yield break;
        }
        else if (boss_hp_ctrl.isReaper_SP_ATK_2 == true && !DarkToken_END[1])
        {
            Reaper_Special_nextAct();
            yield break;
        }
        else if (boss_hp_ctrl.isReaper_SP_ATK_3 == true && !DarkToken_END[2])
        {
            Reaper_Special_nextAct();
            yield break;
        }


        // 거리에 따른 다음 공격
        if (TargetDistance > Skill_Think_Range && isAttacking == false)
        {
            Reaper_Long_nextAct(1);
        }
        else if (TargetDistance <= Skill_Think_Range && isAttacking == false)
        {
            Reaper_Short_nextAct(1);
        }
    }

    // 기본 공격 슬래쉬 이펙트 적용
    public void BaseAtk1_Eff()
    {
        StartCoroutine(Play_BaseAtk1_Eff());
    }

    IEnumerator Play_BaseAtk1_Eff()
    {
        // 이펙트 피격범위 생성
        BaseAtk_1_Eff.SetActive(true);
        BaseAtk_Collider.SetActive(true);

        // 피격범위 끄기
        yield return new WaitForSeconds(0.1f);
        BaseAtk_Collider.SetActive(false);

        // 회전 제한
        yield return new WaitForSeconds(1.9f);
        isLock = false;
        BaseAtk_1_Eff.SetActive(false);
       
    }
    #endregion

    #region Boss_Atk_2_DarkDecline
    // TODO ## Reaper_DarkDecline
    IEnumerator Dark_Decline()
    {
        if (boss_hp_ctrl.isDead == true)
        {
            yield break;
        }

        // 각성 전 어둠의 쇠락
        if (reaperAwakeState == Reaper_Awake.NORMAL)
        {
            // 현재 상태 변경
            reaperState = ReaperState.Dark_Decline;

           
            // yield return new WaitForSeconds(0.2f);

            // 공격 가능
            isAttacking = true;
            // 이동 멈춤
            isMove = false;
            Reaper_animator.SetBool("isMove", isMove);

            // 애니메이션 실행_1
            Reaper_animator.SetTrigger("Dark_Decline");
            // 가이드라인 활성화
            Dark_Decline_GuideLine.SetActive(true);

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

            //yield return new WaitForSeconds(0.2f);

            // Dark_Decline_Delay 후 실행
            yield return new WaitForSeconds(Dark_Decline_Delay - (Decline_LockTime + Decline_UnLockTime));

            // 애니메이션 실행_2
            Reaper_animator.SetTrigger("Dark_Decline");
            // 가이드라인 활성화
            Dark_Decline_GuideLine.SetActive(true);

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

            // 가이드라인 활성화
            //Dark_Decline_GuideLine.SetActive(true);
            //yield return new WaitForSeconds(0.2f);

            // Dark_Decline_Delay 후 실행
            yield return new WaitForSeconds(Dark_Decline_Delay - (Decline_LockTime + Decline_UnLockTime));

            // 애니메이션 실행_3
            Reaper_animator.SetTrigger("Dark_Decline");
            // 가이드라인 활성화
            Dark_Decline_GuideLine.SetActive(true);

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
                AwakeBoss();
                // 다음행동 시간 + 1
                yield return new WaitForSeconds(nextActTime + 1.0f);
            }

            // 광역 공격 가능 여부 체크
            if (boss_hp_ctrl.isReaper_SP_ATK_1 == true && !DarkToken_END[0])
            {
                Reaper_Special_nextAct();
                yield break;
            }
            else if (boss_hp_ctrl.isReaper_SP_ATK_2 == true && !DarkToken_END[1])
            {
                Reaper_Special_nextAct();
                yield break;
            }
            else if (boss_hp_ctrl.isReaper_SP_ATK_3 == true && !DarkToken_END[2])
            {
                Reaper_Special_nextAct();
                yield break;
            }


            // 거리에 따른 다음 공격
            if (TargetDistance > Skill_Think_Range && isAttacking == false)
            {
                Reaper_Long_nextAct(2);
            }
            else if (TargetDistance <= Skill_Think_Range && isAttacking == false)
            {
                Reaper_Short_nextAct(2);
            }
        }
        else if (reaperAwakeState == Reaper_Awake.AWAKENING) // 어둠의 쇠락 각성 후---------------------------------------
        {
            // 현재 상태 변경
            reaperState = ReaperState.Dark_Decline;

            // 가이드라인 활성화
            // Dark_Decline_GuideLine.SetActive(true);
            // yield return new WaitForSeconds(0.2f);

            // 공격 가능
            isAttacking = true;
            // 이동 멈춤
            isMove = false;
            Reaper_animator.SetBool("isMove", isMove);
            // 애니메이션 실행_1
            Reaper_animator.SetTrigger("Dark_Decline");
            // 가이드라인 활성화
            Dark_Decline_GuideLine.SetActive(true);

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

            // 가이드라인 활성화
            //Dark_Decline_GuideLine.SetActive(true);
            //yield return new WaitForSeconds(0.2f);

            // Dark_Decline_Delay 후 실행
            yield return new WaitForSeconds(Dark_Decline_Delay - (Decline_LockTime + Decline_UnLockTime));
            // 애니메이션 실행_2
            Reaper_animator.SetTrigger("Dark_Decline");

            // 가이드라인 활성화
            Dark_Decline_GuideLine.SetActive(true);

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

            // 가이드라인 활성화
            //Dark_Decline_GuideLine.SetActive(true);
            //yield return new WaitForSeconds(0.2f);

            // Dark_Decline_Delay 후 실행
            yield return new WaitForSeconds(Dark_Decline_Delay - (Decline_LockTime + Decline_UnLockTime));

            // 애니메이션 실행_3
            Reaper_animator.SetTrigger("Dark_Decline");

            // 가이드라인 활성화
            Dark_Decline_GuideLine.SetActive(true);

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

            if (boss_hp_ctrl.isReaper_SP_ATK_3 == true && !DarkToken_END[2])
            {
                Reaper_Special_nextAct();
                yield break;
            }

            // 거리에 따른 다음 공격
            if (TargetDistance > Skill_Think_Range && isAttacking == false)
            {
                Reaper_Long_nextAct(10);
            }
            else if (TargetDistance <= Skill_Think_Range && isAttacking == false)
            {
                Reaper_Short_nextAct(2);
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
        // 가이드라인 비활성화
        Dark_Decline_GuideLine.SetActive(false);

        // 어둠의 쇠락 구체 범위 활성화
        yield return new WaitForSeconds(0.2f);
        Dark_Decline_Circle_Collider.SetActive(true);

        // 어둠의 쇠락 구체 범위 비활성화, 박스범위 활성화
        yield return new WaitForSeconds(1.4f);
        Dark_Decline_Circle_Collider.SetActive(false);
        Dark_Decline_Box_Collider.SetActive(true);

        yield return new WaitForSeconds(0.2f);
        Dark_Decline_Box_Collider.SetActive(false);
        // 이펙트 삭제
        yield return new WaitForSeconds(0.1f);

        Dark_Decline_Slash.SetActive(false);
    }
    #endregion

    #region Reaper_Atk_3_DarkHand
    // TODO ## Reaper_DarkHand / Reaper_DarkHand2
    IEnumerator Dark_Hand()
    {
        if (boss_hp_ctrl.isDead == true)
        {
            yield break;
        }

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
        yield return new WaitForSeconds(3.0f);

        //  애니메이션이 끝나고 난 뒤 2초 후
        yield return new WaitForSeconds(2.0f);
        // 공격 가능
        isAttacking = false;
        isLock = false;

        // 각성
        if (boss_hp_ctrl.isAwakening == true && reaperAwakeState == Reaper_Awake.NORMAL)
        {
            AwakeBoss();
            // 다음행동 시간 + 1
            yield return new WaitForSeconds(nextActTime + 1.0f);
        }

        // 광역 공격 가능 여부 체크
        if (boss_hp_ctrl.isReaper_SP_ATK_1 == true && !DarkToken_END[0])
        {
            Reaper_Special_nextAct();
            yield break;
        }
        else if (boss_hp_ctrl.isReaper_SP_ATK_2 == true && !DarkToken_END[1])
        {
            Reaper_Special_nextAct();
            yield break;
        }
        else if (boss_hp_ctrl.isReaper_SP_ATK_3 == true && !DarkToken_END[2])
        {
            Reaper_Special_nextAct();
            yield break;
        }

        // 거리에 따른 다음 공격
        if (TargetDistance > Skill_Think_Range && isAttacking == false)
        {
            Reaper_Long_nextAct(2);
        }
        else if (TargetDistance <= Skill_Think_Range && isAttacking == false)
        {
            Reaper_Short_nextAct(10);
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

        //가이드라인 활성화, 위치 조정
        DarkHand_GuideLine.SetActive(true);
        DarkHand_GuideLine.transform.position = Pos;

        yield return new WaitForSeconds(1.5f);

        // 가이드라인 비활성화
        DarkHand_GuideLine.SetActive(false);

        // 오브젝트 풀에서 이펙트 위치 조정시켜 생성
        GameObject DarkHnad_Explosion = reaper_ObjPoolRef.GetDarkHandFromPool();

        // 만약 콜라이더가 꺼져 있다면
        if (DarkHnad_Explosion.GetComponent<CapsuleCollider>().enabled == false)
        {
            DarkHnad_Explosion.GetComponent<CapsuleCollider>().enabled = true;
        }

        DarkHnad_Explosion.transform.position = Pos;


        yield return new WaitForSeconds(0.5f);
        DarkHnad_Explosion.GetComponent<CapsuleCollider>().enabled = false;

        yield return new WaitForSeconds(3.0f);
        DarkHnad_Explosion.SetActive(false);
    }
    #endregion

    #region Reaper_Atk_4_DarkHand2
    IEnumerator Dark_Hand2()
    {
        if (boss_hp_ctrl.isDead == true)
        {
            yield break;
        }

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

        if (boss_hp_ctrl.isReaper_SP_ATK_3 == true && !DarkToken_END[2])
        {
            Reaper_Special_nextAct();
            yield break;
        }

        // 거리에 따른 다음 공격
        if (TargetDistance > Skill_Think_Range && isAttacking == false)
        {
            Reaper_Long_nextAct(3);
        }
        else if (TargetDistance <= Skill_Think_Range && isAttacking == false)
        {
            Reaper_Short_nextAct(10);
        }
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

        // 가이드라인 활성화 및 위치 조정
        DarkHand2_GuideLine.SetActive(true);
        DarkHand2_GuideLine.transform.position = Pos;

        yield return new WaitForSeconds(0.5f);

        DarkHand2_GuideLine.SetActive(false);

        // 오브젝트 풀에서 이펙트 위치 조정시켜 생성
        GameObject DarkHnad2_Explosion = reaper_ObjPoolRef.GetDarkHand2FromPool();
        DarkHnad2_Explosion.transform.GetChild(2).gameObject.SetActive(true);
        DarkHnad2_Explosion.transform.GetChild(1).GetChild(8).GetComponent<CapsuleCollider>().enabled = true;


        DarkHnad2_Explosion.transform.position = Pos;

        yield return new WaitForSeconds(1.0f);
        DarkHnad2_Explosion.transform.GetChild(1).GetChild(8).gameObject.SetActive(false);

        yield return new WaitForSeconds(8.0f);
        // 원 위치
        DarkHnad2_Explosion.transform.GetChild(2).GetChild(0).transform.localPosition = Vector3.zero;
        DarkHnad2_Explosion.transform.GetChild(2).GetChild(1).transform.localPosition = Vector3.zero;
        DarkHnad2_Explosion.SetActive(false);
    }

    #endregion

    #region Reaper_Atk_5_DarkSoul
    // TODO ## Reaper_DarkSoul
    IEnumerator Dark_Soul()
    {
        if (boss_hp_ctrl.isDead == true)
        {
            yield break;
        }

        // 현재 상태 변경
        reaperState = ReaperState.Dark_Soul;
        // 가이드라인 활성화
        DarkSoul_GuideLine.SetActive(true);

        // 가이드라인 비활성후 시작
        // yield return new WaitForSeconds(1.0f);

        // 공격 가능
        isAttacking = true;
        // 이동 멈춤
        isMove = false;
        Reaper_animator.SetBool("isMove", isMove);
        // 애니메이션 실행_1
        Reaper_animator.SetTrigger("Dark_Soul");
        Slow_RotSpeed = 6.0f;

        yield return new WaitForSeconds(1.5f);
        // 가이드라인 비활성화
        DarkSoul_GuideLine.SetActive(false);

        yield return new WaitForSeconds(0.5f);
        DarkSoul_Collider.SetActive(true);

        yield return new WaitForSeconds(DarkSoul_Running_Time - 4.0f);
        DarkSoul_Collider.SetActive(false);

        yield return new WaitForSeconds(1.0f);
        // 공격 가능
        isAttacking = false;
        Slow_RotSpeed = 0.0f;

        if (boss_hp_ctrl.isReaper_SP_ATK_3 == true && !DarkToken_END[2])
        {
            Reaper_Special_nextAct();
            yield break;
        }

        // 거리에 따른 다음 공격
        if (TargetDistance > Skill_Think_Range && isAttacking == false)
        {
            Reaper_Long_nextAct(10);
        }
        else if (TargetDistance <= Skill_Think_Range && isAttacking == false)
        {
            Reaper_Short_nextAct(3);
        }
    }

    public void DarkSoul_Eff()
    {
        StartCoroutine(Play_DarkSoul_Eff());
    }

    IEnumerator Play_DarkSoul_Eff()
    {
        if (reaperState == ReaperState.Dark_Soul) // 각성 전 
        {
            Reaper_animator.SetFloat("DarkSoulSpeed", 0.2f);
            DarkSoul_Skill_Eff.SetActive(true);


            yield return new WaitForSeconds(DarkSoul_Running_Time - 4.0f);
            Reaper_animator.SetFloat("DarkSoulSpeed", 1.0f);


            yield return new WaitForSeconds(2.0f);
            DarkSoul_Skill_Eff.SetActive(false);
        }
        else if (reaperState == ReaperState.Dark_Ball)// 각성 후
        {
       

            Reaper_animator.SetFloat("DarkSoulSpeed", 0.1f);
            DarkBall_Soul_Eff.SetActive(true);


            yield return new WaitForSeconds(DarkBall_Razer_Time);
            Reaper_animator.SetFloat("DarkSoulSpeed", 1.0f);
            Debug.Log(1);
            // 영혼 콜라이더 이펙트 끄기
            DarkSoul_Collider.SetActive(false);

            yield return new WaitForSeconds(2.0f);
            DarkBall_Soul_Eff.SetActive(false);
        }
    }

    #endregion

    #region Atk_6_Dark_Ball
    // TODO ## Reaper_DarkBall
    IEnumerator Dark_Ball()
    {
        if (boss_hp_ctrl.isDead == true)
        {
            yield break;
        }

        // 각성 전 어둠의 구체
        if (reaperAwakeState == Reaper_Awake.NORMAL)
        {
            for (int i = 0; i < DarkBall_Pilar.Length; i++)
            {
                DarkBall_Pilar[i].GetComponent<BoxCollider>().enabled = true;
                DarkBall_Pilar[i].GetComponent<DarkBall_Pilar_Ctrl>().Mat.SetColor("_EmissionColor",
                    new Color(83, 34, 191) * 0.01f);
                DarkBall_Pilar[i].GetComponent<DarkBall_Pilar_Ctrl>().isEnter = false;
            }

            // 상태 변경
            reaperState = ReaperState.Dark_Ball;
            // 애니메이션 작동
            Reaper_animator.SetTrigger("Teleport");
            // 중앙으로 이동
            this.transform.position = Center_Tr.position;
            // 공격 중
            isAttacking = true;
            // 이동 멈춤
            isMove = false;
            Reaper_animator.SetBool("isMove", isMove);

            yield return new WaitForSeconds(DarkBall_Delay);

            // 애니메이션 작동
            Reaper_animator.SetTrigger("Dark_Ball");

            yield return new WaitForSeconds(DarkBall_Delay);

            // 애니메이션 작동
            Reaper_animator.SetTrigger("Dark_Ball");

            yield return new WaitForSeconds(DarkBall_Delay);

            // 애니메이션 작동
            Reaper_animator.SetTrigger("Dark_Ball");

            yield return new WaitForSeconds(DarkBall_Delay);

            // 애니메이션 작동
            Reaper_animator.SetTrigger("Dark_Ball");

            yield return new WaitForSeconds(Finish_DarkBall);

            // 기둥 폭발
            for (int i = 0; i < DarkBall_Pilar.Length; i++)
            {
                // 만약 들어가지 않았다면
                if (DarkBall_Pilar[i].GetComponent<DarkBall_Pilar_Ctrl>().isEnter == false)
                {
                    // 들어가지않은 기둥 폭발
                    DarkBall_Pilar[i].GetComponent<DarkBall_Pilar_Ctrl>().Pilar_Explosion.SetActive(true);
                }
            }

            yield return new WaitForSeconds(2.0f);

            // 기둥 폭발
            for (int i = 0; i < DarkBall_Pilar.Length; i++)
            {
                // 폭발 비활성화
                DarkBall_Pilar[i].GetComponent<DarkBall_Pilar_Ctrl>().Pilar_Explosion.SetActive(false);
            }

            // 공격 중
            isAttacking = false;


            // 광역 공격 가능 여부 체크
            if (boss_hp_ctrl.isReaper_SP_ATK_1 == true && !DarkToken_END[0])
            {
                Reaper_Special_nextAct();
                yield break;
            }
            else if (boss_hp_ctrl.isReaper_SP_ATK_2 == true && !DarkToken_END[1])
            {
                Reaper_Special_nextAct();
                yield break;
            }
            else if (boss_hp_ctrl.isReaper_SP_ATK_3 == true && !DarkToken_END[2])
            {
                Reaper_Special_nextAct();
                yield break;
            }


            // 거리에 따른 다음 공격
            if (TargetDistance > Skill_Think_Range && isAttacking == false)
            {
                Reaper_Long_nextAct(3);
            }
            else if (TargetDistance <= Skill_Think_Range && isAttacking == false)
            {
                Reaper_Short_nextAct(10);
            }
        }
        // 각성 후 어둠의 구체
        else if (reaperAwakeState == Reaper_Awake.AWAKENING)
        {
            for (int i = 0; i < DarkBall_Pilar_Awakening.Length; i++)
            {
                DarkBall_Pilar_Awakening[i].GetComponent<BoxCollider>().enabled = true;
                
                // 색 초기화
                if (i == 0) // 1시 파랑
                {
                    DarkBall_Pilar_Awakening[i].GetComponent<DarkBall_Pilar_Ctrl>().Mat.SetColor("_EmissionColor",
                     new Color(0, 9, 191) * 0.01f);

                    DarkBall_Pilar_Awakening[i].GetComponent<DarkBall_Pilar_Ctrl>().pilarColor = Reaper_Pattern_Color.BLUE;
                }
                else if (i == 1) // 5시 노랑
                {
                    DarkBall_Pilar_Awakening[i].GetComponent<DarkBall_Pilar_Ctrl>().Mat.SetColor("_EmissionColor",
                    new Color(191, 157, 34) * 0.01f);

                    DarkBall_Pilar_Awakening[i].GetComponent<DarkBall_Pilar_Ctrl>().pilarColor = Reaper_Pattern_Color.YELLOW;
                }
                else if (i == 2) // 7시 초록
                {
                    DarkBall_Pilar_Awakening[i].GetComponent<DarkBall_Pilar_Ctrl>().Mat.SetColor("_EmissionColor",
                    new Color(3, 191, 0) * 0.01f);

                    DarkBall_Pilar_Awakening[i].GetComponent<DarkBall_Pilar_Ctrl>().pilarColor = Reaper_Pattern_Color.GREEN;
                }
                else // i = 3 11시 빨강
                {
                    DarkBall_Pilar_Awakening[i].GetComponent<DarkBall_Pilar_Ctrl>().Mat.SetColor("_EmissionColor",
                    new Color(191, 0, 1) * 0.01f);

                    DarkBall_Pilar_Awakening[i].GetComponent<DarkBall_Pilar_Ctrl>().pilarColor = Reaper_Pattern_Color.RED;
                }

                DarkBall_Pilar_Awakening[i].GetComponent<DarkBall_Pilar_Ctrl>().isEnter = false;
            }
            // 상태 변경
            reaperState = ReaperState.Dark_Ball;
            // 애니메이션 작동
            Reaper_animator.SetTrigger("Teleport");
            // 중앙으로 이동
            this.transform.position = Center_Tr.position;

            isAttacking = true;
            // 이동 멈춤
            isMove = false;
            Reaper_animator.SetBool("isMove", isMove);
            Slow_RotSpeed = 6.5f;

          

            yield return new WaitForSeconds(DarkBall_Delay - 1.0f);
            // 애니메이션 실행_1
            Reaper_animator.SetTrigger("Dark_Soul");

            yield return new WaitForSeconds(1.0f);
            // 가이드라인 활성화
            DarkSoul_GuideLine.SetActive(true);

            yield return new WaitForSeconds(DarkBall_Delay - 2.0f);
            DarkSoul_GuideLine.SetActive(false);
            DarkSoul_Collider.SetActive(true);

            // 빨강 구체 생성
            yield return new WaitForSeconds(2.0f);
            // 어둠의 구체 활성화
            DarkBall_Awakening[Awakening_Ball_Index].gameObject.SetActive(true);
            // 어둠의 구체 생성 위치 초기화
            DarkBall_Awakening[Awakening_Ball_Index].transform.position = DarkBall_Pos.position;
            // 인덱스 증가
            Awakening_Ball_Index++;

            // 파랑 구체 생성
            yield return new WaitForSeconds(DarkBall_Delay);
            // 어둠의 구체 활성화
            DarkBall_Awakening[Awakening_Ball_Index].gameObject.SetActive(true);
            // 어둠의 구체 생성 위치 초기화
            DarkBall_Awakening[Awakening_Ball_Index].transform.position = DarkBall_Pos.position;
            // 인덱스 증가
            Awakening_Ball_Index++;

            // 노랑 구체 생성
            yield return new WaitForSeconds(DarkBall_Delay);
            // 어둠의 구체 활성화
            DarkBall_Awakening[Awakening_Ball_Index].gameObject.SetActive(true);
            // 어둠의 구체 생성 위치 초기화
            DarkBall_Awakening[Awakening_Ball_Index].transform.position = DarkBall_Pos.position;
            // 인덱스 증가
            Awakening_Ball_Index++;

            // 초록 구체 생성
            yield return new WaitForSeconds(DarkBall_Delay);
            // 어둠의 구체 활성화
            DarkBall_Awakening[Awakening_Ball_Index].gameObject.SetActive(true);
            // 어둠의 구체 생성 위치 초기화
            DarkBall_Awakening[Awakening_Ball_Index].transform.position = DarkBall_Pos.position;
            // 인덱스 초기화
            Awakening_Ball_Index = 0;

            yield return new WaitForSeconds((DarkBall_Razer_Time - (4 * DarkBall_Delay)));

            // 기둥 폭발
            for (int i = 0; i < DarkBall_Pilar.Length; i++)
            {
                // 만약 들어가지 않았다면
                if (DarkBall_Pilar_Awakening[i].GetComponent<DarkBall_Pilar_Ctrl>().isEnter == false)
                {
                    // 들어가지않은 기둥 폭발
                    DarkBall_Pilar_Awakening[i].GetComponent<DarkBall_Pilar_Ctrl>().Awakening_Pilar_Explosion.SetActive(true);
                }
            }

            yield return new WaitForSeconds(2.0f);

            // 기둥 폭발
            for (int i = 0; i < DarkBall_Pilar.Length; i++)
            {
                // 폭발 비활성화
                DarkBall_Pilar_Awakening[i].GetComponent<DarkBall_Pilar_Ctrl>().Awakening_Pilar_Explosion.SetActive(false);
            }

            // 공격 중
            isAttacking = false;
            Slow_RotSpeed = 0.0f;

            if (boss_hp_ctrl.isReaper_SP_ATK_3 == true && !DarkToken_END[2])
            {
                Reaper_Special_nextAct();
                yield break;
            }

            // 거리에 따른 다음 공격
            if (TargetDistance > Skill_Think_Range && isAttacking == false)
            {
                Reaper_Long_nextAct(4);
            }
            else if (TargetDistance <= Skill_Think_Range && isAttacking == false)
            {
                Reaper_Short_nextAct(10);
            }
        }
    }

    public void DarkBall_Eff()
    {
        StartCoroutine(Play_DarkBall_Eff());
    }

    IEnumerator Play_DarkBall_Eff()
    {
        yield return new WaitForSeconds(0);

        // 각성 전 
        if (reaperAwakeState == Reaper_Awake.NORMAL)
        {
            // 어둠의 구체 가져오기
            GameObject DarkBall = reaper_ObjPoolRef.GetDarkBallFromPool();
            DarkBall.transform.position = DarkBall_Pos.position;
        }
        else // 각성 후
        {
            // 따로 색깔 별 구체 관리하기 때문에 사용 
        }
    }
    #endregion

    #region Dark_Token
    // TODO ## Reaper_Token
    IEnumerator Dark_Token()
    {
        if (boss_hp_ctrl.isDead == true)
        {
            yield break;
        }

        // 상태 변경
            reaperState = ReaperState.Dark_Token;
        // 애니메이션 작동
        Reaper_animator.SetTrigger("Teleport");
        // 중앙으로 이동
        this.transform.position = Center_Tr.position;
        // 공격 중
        isAttacking = true;
        this.transform.rotation = Quaternion.Euler(0.0f, -90.0f, 0.0f);
        //회전 멈춤
        isLock = true;
        // 이동 멈춤
        isMove = false;
        Reaper_animator.SetBool("isMove", isMove);

        // 첫번째
        yield return new WaitForSeconds(Token_Delay);
        // 장판 활성화
        Flooring_Effect.SetActive(true);

        // 첫번째
        yield return new WaitForSeconds(Token_Delay);
        Token_obj[0].SetActive(true);
        Token_GuideLine[0].SetActive(true);

        // 두번째
        yield return new WaitForSeconds(Token_Delay);
        Token_obj[1].SetActive(true);
        Token_GuideLine[0].SetActive(false);
        Token_GuideLine[1].SetActive(true);

        // 세번째
        yield return new WaitForSeconds(Token_Delay);
        Token_obj[2].SetActive(true);
        Token_GuideLine[1].SetActive(false);
        Token_GuideLine[2].SetActive(true);

        // 네번째
        yield return new WaitForSeconds(Token_Delay);
        Token_obj[3].SetActive(true);
        Token_GuideLine[2].SetActive(false);
        Token_GuideLine[3].SetActive(true);

        // 다섯번째
        yield return new WaitForSeconds(Token_Delay);
        Token_obj[4].SetActive(true);
        Token_GuideLine[3].SetActive(false);
        Token_GuideLine[4].SetActive(true);

        // 여섯번째
        yield return new WaitForSeconds(Token_Delay);
        Token_obj[5].SetActive(true);
        Token_GuideLine[4].SetActive(false);
        Token_GuideLine[5].SetActive(true);

        // 일곱번째
        yield return new WaitForSeconds(Token_Delay);
        Token_obj[6].SetActive(true);
        Token_GuideLine[5].SetActive(false);
        Token_GuideLine[6].SetActive(true);


        // 여덟번째
        yield return new WaitForSeconds(Token_Delay);
        Token_obj[7].SetActive(true);
        Token_GuideLine[6].SetActive(false);
        Token_GuideLine[7].SetActive(true);


        yield return new WaitForSeconds(Token_Delay);
        Token_GuideLine[7].SetActive(false);

        yield return new WaitForSeconds(2);

        // 장판 이펙트 비활성화
        Flooring_Effect.SetActive(false);

        // 공격 중
        isAttacking = false;
        //회전 멈춤
        isLock = false;

        // 거리에 따른 다음 공격
        if (TargetDistance > Skill_Think_Range && isAttacking == false)
        {
            Reaper_Long_nextAct(10);
        }
        else if (TargetDistance <= Skill_Think_Range && isAttacking == false)
        {
            Reaper_Short_nextAct(10);
        }
    }
    #endregion
}

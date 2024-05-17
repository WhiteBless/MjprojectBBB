using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using InfinityPBR;
public enum TreantType
{
    NORMAL,
    SPEED,
    POWER,
}

public enum Treant_Normal_State
{
    NONE = -1, 
  
    IDLE,
    MOVE,
    LEAFMISSALE,
    LEAFBREATH,
    NORMALATTACK,
    BARRIER,
    LEAFTURN,
    LEAFPLACE,
    FORMCHANGE,
    END,
}

public enum Treant_Speed_State
{
    NONE = -1,
   
    IDLE,
    MOVE,
    TRUNWHEEL,
    DASH,
    NORMALATTACK,
    BARRIER,
    CHOP,
    CLAP,
    FORMCHANGE,
    END,
}

public enum Treant_Power_State
{
    NONE = -1,
   
    IDLE,
    MOVE,
    NORMALATTACK,
    BARRIER,
    GOLEM_RECALL,
    THROW_STONE,
    HULK_BURST_1,
    HULK_BURST_2,
    FORMCHANGE,
    END,
}

public class Treant_Controller : Boss_BehaviorCtrl_Base
{
    [Header("----Treant_Animation_Variable---")]
    [SerializeField]
    Animator animator;
    [SerializeField]
    float startVal;
    [SerializeField]
    float endVal;
    [SerializeField]
    float lerpTime;

    [Header("----Treant_HP_Variable---")]
    [SerializeField]
    Boss_HP_Controller boss_hp_ctrl;        // HP 컨트롤러
    [SerializeField]
    int MaxHP;                              // 최대 체력


    [Header("----Treant_Target_Variable---")]
    public GameObject Target;
    [SerializeField]
    float TargetDistance;
    [SerializeField]
    float ChaseDistance;
    [SerializeField]
    GameObject TreantAtkRange;
    public bool isStartRaid;

    [Header("----Treant_State_Variable---")]
    [SerializeField]
    BlendShapesPresetManager Treant_Type_Shape;     // 모형 변경 관련 스크립
    [SerializeField]
    TreantType Treant_Type;       // 3층보스 형태
    [SerializeField]
    Treant_Normal_State TreantNormalState;     // 3층 보스 노말폼 현재 상태
    [SerializeField]
    Treant_Speed_State TreantSpeedState;
    [SerializeField]
    Treant_Power_State TreantPowerState;
    [SerializeField]
    bool isMove;                        // 움직임 체크 변수
    [SerializeField]
    public bool isAttacking;            // 공격 체크 변수
    [SerializeField]
    bool isLock;                        // 회전 가능여부 체크 변수
    [SerializeField]
    float Treant_MoveSpeed;             // 이동속도
    [SerializeField]
    float Treant_Normal_MoveSpeed;
    [SerializeField]
    float Treant_Speed_MoveSpeed;
    [SerializeField]
    float Treant_Power_MoveSpeed;
    [SerializeField]
    float Treant_RotSpeed;
    [SerializeField]
    float Treant_Slow_RotSpeed;
    [SerializeField]
    float Treant_Skill_Delay;
    [SerializeField]
    bool isThink;
    [SerializeField]
    bool isCurIdle;

    Vector3 dir;                  // Treant 각도

    [Header("----Treant_Speed_Dash_Variable---")]
    [SerializeField]
    int DashCount;
    [SerializeField]
    float DashTime;
    [SerializeField]
    float DashSpeed;
    [SerializeField]
    int ReDash_Percent;
    [SerializeField]
    bool isReDash;
    [SerializeField]
    bool isEnterCoroutine;

    [Header("----Treant_Speed_TurnWheel_Variable---")]
    [SerializeField]
    float TurnWheel_Time;
    [SerializeField]
    float TurnWheel_Speed;

    #region Rotation
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
        // 회전 계산 스킬에 따라 회전방향 느려질수 있음
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * (Treant_RotSpeed - Treant_Slow_RotSpeed));
    }
    #endregion

    #region Move
    public override void Move()
    {
        if (Target == null)
            return;

        if (isMove && isAttacking == false)
        {

            transform.Translate(Vector3.forward * Treant_MoveSpeed * Time.deltaTime);
            //animator.SetFloat("Locomotion", 1.0f);
            animator.SetBool("isMove", true);
        }
    }

    public void NotMove()
    {
        if (Target == null)
            return;

        isMove = false;

        if (!isMove && isAttacking == false)
        {
            // 노말 상태로 멈출 시 
            if (Treant_Type == TreantType.NORMAL)
            {
                TreantNormalState = Treant_Normal_State.IDLE;
                TreantPowerState = Treant_Power_State.NONE;
                TreantSpeedState = Treant_Speed_State.NONE;
            }
            else if (Treant_Type == TreantType.POWER) // 파워폼으로 멈출 시
            {
                TreantNormalState = Treant_Normal_State.NONE;
                TreantPowerState = Treant_Power_State.IDLE;
                TreantSpeedState = Treant_Speed_State.NONE;
            }
            else // 스피드 폼으로 멈출 시
            {
                TreantNormalState = Treant_Normal_State.NONE;
                TreantPowerState = Treant_Power_State.NONE;
                TreantSpeedState = Treant_Speed_State.IDLE;
            }

            transform.Translate(Vector3.forward * 0.0f * Time.deltaTime);
            //animator.SetFloat("Locomotion", 0.5f);
            animator.SetBool("isMove", false);
        }
    }
    #endregion

    #region Awake / Start / Update
    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        Treant_Init();
    }

    void Update()
    {
        // 플레이어가 null이 아니라면
        if (Target != null)
        {
            TargetDistance = Vector3.Distance(Target.transform.position, this.transform.position);
        }

        LookAtPlayer(); // 플레이어 방향 전환
    }

    void FixedUpdate()
    {
        // 사정 거리 안에 안들어 왔을 경우
        if (TargetDistance >= ChaseDistance)
        {
            Move();
        }
        else if (TargetDistance < ChaseDistance + 0.5f && !isAttacking) // 공격중이 아니고 사정거리안에 들어 왔을 시
        {
            NotMove();
        }
    }
    #endregion

    #region Init
    void Treant_Init()
    {
        Treant_Type_Shape = GetComponent<BlendShapesPresetManager>();
        animator = GetComponent<Animator>();
        boss_hp_ctrl = GetComponent<Boss_HP_Controller>();

        Target = GameObject.FindGameObjectWithTag("Player");
        // 최대 체력 전달 현재 체력 
        boss_hp_ctrl.BossMaxHP = MaxHP;
        // 현재 폼 초기화
        Treant_Type = TreantType.NORMAL;
        TreantNormalState = Treant_Normal_State.IDLE;
        TreantPowerState = Treant_Power_State.NONE;
        TreantSpeedState = Treant_Speed_State.NONE;
    }
    #endregion

    #region Treant_Form_Change
    public void Change_Normal_Form()
    {
        // TODO ## 3층 보스 노말 폼 체인지
        Treant_Type = TreantType.NORMAL;

        TreantSpeedState = Treant_Speed_State.NONE;
        TreantPowerState = Treant_Power_State.NONE;

        // 이동속도 변경
        Treant_MoveSpeed = Treant_Normal_MoveSpeed;
        // 형태 변환
        Treant_Type_Shape.StartTransitionToPreset("Reset");
        animator.SetTrigger("FormChange");
        animator.SetFloat("AnimSpeed", 1.0f);
    }

    public void Change_Speed_Form()
    {
        // TODO ## 3층 보스 스피드 폼 체인지
        Treant_Type = TreantType.SPEED;

        TreantNormalState = Treant_Normal_State.NONE;
        TreantPowerState = Treant_Power_State.NONE;


        // 이동속도 변경
        Treant_MoveSpeed = Treant_Speed_MoveSpeed;
        // 형태 변환
        Treant_Type_Shape.StartTransitionToPreset("Skinny");
        animator.SetTrigger("FormChange");
        animator.SetFloat("AnimSpeed", 1.5f);
    }
    public void Change_Power_Form()
    {
        // TODO ## 3층 보스 파워 폼 체인지
        Treant_Type = TreantType.POWER;

        TreantNormalState = Treant_Normal_State.NONE;
        TreantSpeedState = Treant_Speed_State.NONE;

        // 이동속도 변경
        Treant_MoveSpeed = Treant_Power_MoveSpeed;
        // 형태 변환
        Treant_Type_Shape.StartTransitionToPreset("Fat");
        animator.SetTrigger("FormChange");
        animator.SetFloat("AnimSpeed", 1.0f);
    }

    // 노말 폼일 떄 변신
    public void CurrentType_Normal_Form_Change()
    {
        int changePercent = Random.Range(0, 10);

        // 50 / 50 확률로 변신
        if (changePercent < 5)
        {
            Change_Speed_Form();
        }
        else
        {
            Change_Power_Form();
        }
    }

    // 스피드 폼일 떄 변신
    public void CurrentType_Speed_Form_Change()
    {
        int changePercent = Random.Range(0, 10);

        // 30 / 70 확률로 변신
        if (changePercent < 3)
        {
            Change_Normal_Form();
        }
        else
        {
            Change_Power_Form();
        }
    }

    // 파워 폼일 떄 변신
    public void CurrentType_Power_Form_Change()
    {
        int changePercent = Random.Range(0, 10);

        // 30 / 70 확률로 변신
        if (changePercent < 3)
        {
            Change_Normal_Form();
        }
        else
        {
            Change_Speed_Form();
        }
    }
    #endregion

    #region next_act

    public void Treant_NextAct()
    {
        // 타깃이 없거나 공격중이면 return;
        if (Target == null || isAttacking || isThink)
            return;

        //StartCoroutine(Next_Act());

        Next_Act();
    }

    // IEnumerator Next_Act()
    public void Next_Act()
    {
        isThink = true;

        // 다음 행동 변경
        if (Treant_Type == TreantType.NORMAL)
        {
            // 랜덤으로 다음 상태 변경
            Treant_Normal_State randomNormalState = (Treant_Normal_State)Random.Range(2, (int)Treant_Normal_State.END);
            TreantNormalState = randomNormalState;

            Debug.Log(randomNormalState);
        }
        else if (Treant_Type == TreantType.POWER)
        {
            // 랜덤으로 다음 상태 변경
            Treant_Power_State randomPowerState = (Treant_Power_State)Random.Range(2, (int)Treant_Power_State.END);
            TreantPowerState = randomPowerState;

            Debug.Log(randomPowerState);

        }
        else if (Treant_Type == TreantType.SPEED)
        {
            // 랜덤으로 다음 상태 변경
            Treant_Speed_State randomSpeedState = (Treant_Speed_State)Random.Range(2, (int)Treant_Speed_State.END);
            //Treant_Speed_State randomSpeedState = (Treant_Speed_State)7;
            TreantSpeedState = randomSpeedState;

            Debug.Log(randomSpeedState);
        }

        // 기본 상태가 아닐 시 2초간의 딜레이를 준다
        //yield return new WaitForSeconds(0.0f);

        // 대상이 사정 거리보다 멀면 이동 선택
        if (TargetDistance >= ChaseDistance + 1.0f)
        {
            if (Treant_Type == TreantType.NORMAL)
            {
                TreantNormalState = Treant_Normal_State.MOVE;
            }
            else if (Treant_Type == TreantType.POWER)
            {
                TreantPowerState = Treant_Power_State.MOVE;
            }
            else
            {
                TreantSpeedState = Treant_Speed_State.MOVE;
            }
        }

        

        switch (Treant_Type)
        {
            // 노말 폼일 때 스킬
            case TreantType.NORMAL:
                switch (TreantNormalState)
                {
                    //case Treant_Normal_State.IDLE:
                    //    Treant_Idle();
                    //    break;
                    case Treant_Normal_State.MOVE:
                        Treant_Move();
                        break;
                    case Treant_Normal_State.NORMALATTACK:
                        Normal_Attack();
                        break;
                    case Treant_Normal_State.BARRIER:
                        Treant_Barrier();
                        break;
                    case Treant_Normal_State.LEAFTURN:
                        Treant_LeafTurn();
                        break;
                    case Treant_Normal_State.LEAFBREATH:
                        Treant_LeafBreath();
                        break;
                    case Treant_Normal_State.LEAFPLACE:
                        Treant_LeafPlace();
                        break;
                    case Treant_Normal_State.LEAFMISSALE:
                        Treant_LeafMissale();
                        break;
                    case Treant_Normal_State.FORMCHANGE:
                        CurrentType_Normal_Form_Change();
                        break;
                    default:
                        break;
                }
                break;
                // 스피드 폼일때
            case TreantType.SPEED:
                switch (TreantSpeedState)
                {
                    //case Treant_Speed_State.IDLE:
                    //    Treant_Idle();
                    //    break;
                    case Treant_Speed_State.MOVE:
                        Treant_Move();
                        break;
                    case Treant_Speed_State.NORMALATTACK:
                        Normal_Attack();
                        break;
                    case Treant_Speed_State.BARRIER:
                        Treant_Barrier();
                        break;
                    case Treant_Speed_State.DASH:
                        Treant_Dash();
                        break;
                    case Treant_Speed_State.TRUNWHEEL:
                        Treant_TurnWheel();
                        break;
                    case Treant_Speed_State.CHOP:
                        Treant_Chop();
                        break;
                    case Treant_Speed_State.CLAP:
                        Treant_Clap();
                        break;
                    case Treant_Speed_State.FORMCHANGE:
                        CurrentType_Speed_Form_Change();
                        break;
                    default:
                        break;
                }
                break;
                // 파워 폼일때
            case TreantType.POWER:
                switch (TreantPowerState)
                {
                    case Treant_Power_State.IDLE:
                        Treant_Idle();
                        break;
                    case Treant_Power_State.MOVE:
                        Treant_Move();
                        break;
                    case Treant_Power_State.NORMALATTACK:
                        Normal_Attack();
                        break;
                    case Treant_Power_State.BARRIER:
                        Treant_Barrier();
                        break;
                    case Treant_Power_State.GOLEM_RECALL:
                        Treant_Golem_Recall();
                        break;
                    case Treant_Power_State.THROW_STONE:
                        Treant_Throw_Stone();
                        break;
                    case Treant_Power_State.HULK_BURST_1:
                        Treant_Hulk_Burst_1();
                        break;
                    case Treant_Power_State.HULK_BURST_2:
                        Treant_Hulk_Burst_2();
                        break;
                    case Treant_Power_State.FORMCHANGE:
                        CurrentType_Power_Form_Change();
                        break;
                    default:
                        break;
                }
                break;

            default:
                break;
        }
    }
    #endregion

    #region Treant_Idle
    public void Treant_Idle()
    {
        isMove = false;
        animator.SetFloat("Locomotion", 0.5f);
        //다시 활성화
        TreantAtkRange.GetComponent<SphereCollider>().enabled = false;
    }

    public void Treant_Idle_NextAct()
    {
        // 생각중이면
        if (isThink)
        {
            isThink = false;
            return;
        }

        if (!isThink && isStartRaid)
        {
            Treant_NextAct();
        }
    }
    #endregion

    #region Treant_Move
    // TODO ## Treant_Move
    public void Treant_Move()
    {
        isMove = true;
        isThink = false;
    }
    #endregion

    #region Treant_NormalAttack
    // TODO ## Treant_NormalAttack
    public void Normal_Attack()
    {
        isAttacking = true;
        isLock = true;
        animator.SetTrigger("PunchBig");
    }

    public void Normal_Attack_Next()
    {
        // 다음 공격 실행
        StartCoroutine(Normal_Attack_Next_Motion());
    }

    IEnumerator Normal_Attack_Next_Motion()
    {
        isLock = false;

        yield return new WaitForSeconds(1.0f);
        isLock = true;
        animator.SetTrigger("PunchSmall");
    }

    public void Normal_Attack_End()
    {
        isAttacking = false;
        isLock = false;
        isThink = false;
    }
    #endregion

    #region Treant_Barrier
    // TODO ## Treant_Barrier
    public void Treant_Barrier()
    {
        isAttacking = true;
        animator.SetBool("isBlock", true);

       
    }

    public void Treant_Barrier_End()
    {
        StartCoroutine(Treant_Barrier_Start_Event());
    }

    IEnumerator Treant_Barrier_Start_Event()
    {
        yield return new WaitForSeconds(5.0f);
        isAttacking = false;
        animator.SetTrigger("BlockStop");
    }

    public void Treant_Barrier_End_Event()
    {
        animator.SetBool("isBlock", false);
        isThink = false;
    }
    #endregion

    // Normal Form
    #region Treant_LeafTurn
    // TODO ## Treant_LeafTurn
    public void Treant_LeafTurn()
    {
        isAttacking = true;
        isLock = true;
        animator.SetTrigger("LeafTurn");
    }

    public void Treant_LeafTurn_End()
    {
        isAttacking = false;
        isThink = false;
        isLock = false;
    }
    #endregion

    #region Treant_LeafBreath
    // TODO ## Treant_LeafBreath
    public void Treant_LeafBreath()
    {
        isAttacking = true;
        animator.SetTrigger("LeafBreath");
    }

    public void Treant_LeafBreath_Start()
    {
        Treant_Slow_RotSpeed = 4.0f;
    }

    public void Treant_LeafBreath_End()
    {
        Treant_Slow_RotSpeed = 0.0f;
        isAttacking = false;
        isThink = false;
    }
    #endregion

    #region Treant_LeafPlace
    // TODO ## Treant_LeafPlace
    public void Treant_LeafPlace()
    {
        isAttacking = true;
        animator.SetTrigger("LeafPlace");
    }

    public void Treant_LeafPlace_Start()
    {
        isLock = true;
    }

    public void Treant_LeafPlace_End()
    {
        isAttacking = false;
        isLock = false;
        isThink = false;
    }
    #endregion

    #region Treant_LeafMissale
    // TODO ## Treant_LeafMissale
    public void Treant_LeafMissale()
    {
        isAttacking = true;
        animator.SetTrigger("LeafMissale");
    }

    public void Treant_LeafMissale_Start()
    {

    }

    public void Treant_LeafMissale_End()
    {
        isAttacking = false;
        isThink = false;
    }
    #endregion

    // Speed Form
    #region Treant_Dash
    // TODO ## Treant_Dash
    public void Treant_Dash()
    {
        DashCount++; // 대쉬 카운트 증가
        animator.SetTrigger("Dash");
    }

    public void Treant_Dash_Start()
    {
        // 코루틴 실행
        StartCoroutine(Treant_Dash_Start_Event());
        // 코루틴 재실행 방지
        isEnterCoroutine = true;
    }

    IEnumerator Treant_Dash_Start_Event()
    {
        if (isEnterCoroutine == true)
            yield break;

        isAttacking = true;
        isLock = true;
        float time = 0.0f;

        while (time < DashTime)
        {
            time += Time.deltaTime;
            transform.Translate(Vector3.forward * DashSpeed * Time.deltaTime);
            yield return null;
        }

        // 첫번째랑 두번째 돌진 때 계산
        if (DashCount != 0 && DashCount < 3)
        {
            // 확률
            int re_Dash = Random.Range(0, 10);

            // 재사용 확률
            if (re_Dash < ReDash_Percent)
            {
                // 재사용 한다면 true
                isReDash = true;
                // 애니메이션 재생
                animator.SetTrigger("KeepDash");
            }
            else
            {
                isReDash = false;  
            }
        }

        // 대쉬카운트가 3이거나 연속동작이 없다면 빠져나가기
        if (DashCount == 3 || !isReDash)
        {
            animator.SetTrigger("OutDash");
        }

        isEnterCoroutine = false;
    }

    public void Treant_Keep_Dash_End()
    {
        isLock = false;
        Treant_Dash();
    }

    public void Treant_Out_Dash_End()
    {
        isLock = false;
        isAttacking = false;
        DashCount = 0;
        isThink = false;
    }
    #endregion

    #region Treant_TurnWheel
    // TODO ## Treant_TurnWheel
    public void Treant_TurnWheel()
    {
        isAttacking = true;
        animator.SetTrigger("TurnWheel");
    }

    public void Treant_TurnWheel_Start()
    {
        // 코루틴 실행
        StartCoroutine(Treant_TurnWheel_Start_Event());
        // 코루틴 재실행 방지
        isEnterCoroutine = true;
    }

    IEnumerator Treant_TurnWheel_Start_Event()
    {
        if (isEnterCoroutine == true)
            yield break;

        float time = 0.0f;
        Treant_Slow_RotSpeed = 4.0f;

        while (time < TurnWheel_Time)
        {
            time += Time.deltaTime;
            transform.Translate(Vector3.forward * TurnWheel_Speed * Time.deltaTime);
            yield return null;
        }

        animator.SetTrigger("OutTurnWheel");
    }

    public void Treant_TurnWheel_End()
    {
        Treant_Slow_RotSpeed = 0.0f;
        isAttacking = false;
        isThink = false;
        isEnterCoroutine = false;
    }
    #endregion

    #region Treant_Chop
    // TODO ## Treant_Chop
    public void Treant_Chop()
    {
        isAttacking = true;
        animator.SetTrigger("Chop");
    }

    public void Treant_Chop_Start()
    {
        isLock = true;
    }

    public void Treant_Chop_End()
    {
        isAttacking = false;
        isLock = false;
        isThink = false;
    }
    #endregion

    #region Treant_Clap
    // TODO ## Treant_Clap
    public void Treant_Clap()
    {
        isAttacking = true;
        animator.SetTrigger("Clap");
    }

    public void Treant_Clap_Start()
    {
        isLock = true;
    }

    public void Treant_Clap_End()
    {
        isAttacking = false;
        isLock = false;
        isThink = false;
    }
    #endregion

    // Power Form
    #region Treant_Golem_Recall
    // TODO ## Treant_Golem_Recall
    public void Treant_Golem_Recall()
    {
        isAttacking = true;
        //isLock = true;
        animator.SetTrigger("Golem_Recall");
    }

    public void Treant_Golem_Recall_End()
    {
        isAttacking = false;
        isThink = false;
        isLock = false;
    }
    #endregion

    #region Treant_Throw_Stone
    // TODO ## Treant_Throw_Stone
    public void Treant_Throw_Stone()
    {
        isAttacking = true;
        animator.SetTrigger("Throw_Stone");
    }

    public void Treant_Throw_Stone_Start()
    {
        isLock = true;
    }

    public void Treant_Throw_Stone_Lock()
    {
        isLock = false;
    }

    public void Treant_Throw_Stone_End()
    {
        isAttacking = false;
        isThink = false;
    }
    #endregion

    #region Treant_Hulk_Burst_1
    // TODO ## Treant_Hulk_Burst_1
    public void Treant_Hulk_Burst_1()
    {
        isAttacking = true;
        isLock = true;
        animator.SetTrigger("Hulk_Burst_1");
    }

    public void Treant_Hulk_Burst_1_Start()
    {

    }

    public void Treant_Hulk_Burst_1_End()
    {
        isAttacking = false;
        isLock = false;
        isThink = false;
    }
    #endregion

    #region  Treant_Hulk_Burst_2
    // TODO ## Treant_Hulk_Burst_2
    public void Treant_Hulk_Burst_2()
    {
        isAttacking = true;
        animator.SetTrigger("Hulk_Burst_2");
    }

    // 회전 제한
    public void Treant_Hulk_Burst_2_Start()
    {
        isLock = true;
    }

    // 회전 제한 해제
    public void Treant_Hulk_Burst_2_Lock()
    {
        isLock = false;
    }

    public void Treant_Hulk_Burst_2_End()
    {
        isAttacking = false;
        isThink = false;
        isLock = false;
    }
    #endregion
}

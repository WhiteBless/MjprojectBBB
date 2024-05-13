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

public enum TreantState
{
    NONE = -1,
    IDLE,
    MOVE,
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

    [Header("----Treant_State_Variable---")]
    [SerializeField]
    BlendShapesPresetManager Treant_Type_Shape;     // 모형 변경 관련 스크립
    [SerializeField]
    TreantType Treant_Type;       // 3층보스 형태
    [SerializeField]
    TreantState Treant_State;     // 3층 보스 현재 상태
    [SerializeField]
    bool isMove;                  // 움직임 체크 변수
    [SerializeField]
    bool isAttacking;             // 공격 체크 변수
    [SerializeField]
    bool isLock;                  // 회전 가능여부 체크 변수
    [SerializeField]
    float Treant_MoveSpeed;       // 이동속도
    [SerializeField]
    float Treant_RotSpeed;
    [SerializeField]
    float Treant_Skill_Delay;


    Vector3 dir;                  // Treant 각도

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
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * Treant_RotSpeed);
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
            animator.SetFloat("Locomotion", 1.0f);
        }
    }

    public void NotMove()
    {
        if (Target == null)
            return;

        isMove = false;

        if (!isMove && isAttacking == false)
        {
            Treant_State = TreantState.IDLE;
            transform.Translate(Vector3.forward * 0.0f * Time.deltaTime);
            animator.SetFloat("Locomotion", 0.5f);
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
        else if (TargetDistance < ChaseDistance && !isAttacking) // 공격중이 아니고 사정거리안에 들어 왔을 시
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

        // 최대 체력 전달 현재 체력 
        boss_hp_ctrl.BossMaxHP = MaxHP;
        // 현재 폼 초기화
        Treant_Type = TreantType.NORMAL;
        Treant_State = TreantState.IDLE;
    }
    #endregion

    #region Treant_Form_Change
    public void Change_Normal_Form()
    {
        // TODO ## 3층 보스 노말 폼 체인지
        Treant_Type = TreantType.NORMAL;
        // 형태 변환
        Treant_Type_Shape.StartTransitionToPreset("Reset");
        animator.SetTrigger("MagicAttack2");
    }

    public void Change_Speed_Form()
    {
        // TODO ## 3층 보스 스피드 폼 체인지
        Treant_Type = TreantType.SPEED;
        // 형태 변환
        Treant_Type_Shape.StartTransitionToPreset("Skinny");
        animator.SetTrigger("MagicAttack2");
    }
    public void Change_Power_Form()
    {
        // TODO ## 3층 보스 파워 폼 체인지
        Treant_Type = TreantType.POWER;
        // 형태 변환
        Treant_Type_Shape.StartTransitionToPreset("Fat");
        animator.SetTrigger("MagicAttack2");
    }
    #endregion

    #region next_act

    public void Treant_NextAct()
    {
        // 타깃이 없다면 return;
        if (Target == null)
            return;

        StartCoroutine(Next_Act());
    }

    IEnumerator Next_Act()
    {
        // 기본 상태면 바로 상태 변경
        if (Treant_State == TreantState.IDLE)
        {
            yield return new WaitForSeconds(0.0f);
        }
        else
        {
            // 기본 상태가 아닐 시 2초간의 딜레이를 준다
            yield return new WaitForSeconds(Treant_Skill_Delay);
        }

        // 랜덤으로 다음 상태 변경
        TreantState randomState = (TreantState)Random.Range(0, (int)TreantState.MOVE + 1);
        Treant_State = randomState;

        switch (Treant_Type)
        {
            // 노말 폼일 때 스킬
            case TreantType.NORMAL:
                switch (Treant_State)
                {
                    case TreantState.IDLE:
                        Treant_Idle();
                        break;
                    case TreantState.MOVE:
                        Treant_Move();
                        break;
                    default:
                        break;
                }
                break;

            case TreantType.SPEED:
                switch (Treant_State)
                {
                    case TreantState.IDLE:
                        Treant_Idle();
                        break;
                    case TreantState.MOVE:
                        Treant_Move();
                        break;
                    default:
                        break;
                }
                break;

            case TreantType.POWER:
                switch (Treant_State)
                {
                    case TreantState.IDLE:
                        Treant_Idle();
                        break;
                    case TreantState.MOVE:
                        Treant_Move();
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
    }
    #endregion

    #region Treant_Move
    public void Treant_Move()
    {
        isMove = true;
    }
    #endregion
}

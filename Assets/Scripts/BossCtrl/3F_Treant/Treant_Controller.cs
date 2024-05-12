using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum TreantType
{
    NORMAL,
    SPEED,
    POWER,
}

public enum TreantState
{
    Move,
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
    bool isMove;                  // 움직임 체크 변수
    [SerializeField]
    bool isAttacking;             // 공격 체크 변수
    [SerializeField]
    bool isLock;                  // 회전 가능여부 체크 변수
    [SerializeField]
    float Treant_MoveSpeed;       // 이동속도
    [SerializeField]
    float Treant_RotSpeed;

    Vector3 dir;                  // Treant 각도

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

    #region Move
    public override void Move()
    {
        if (Target == null)
            return;

        isMove = true;

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
        if (TargetDistance > ChaseDistance)
        {
            Move();
        }
        else
        {
            NotMove();
        }
    }
    #endregion


    #region Init
    void Treant_Init()
    {
        animator = GetComponent<Animator>();
        boss_hp_ctrl = GetComponent<Boss_HP_Controller>();
        // 최대 체력 전달 현재 체력 
        boss_hp_ctrl.BossMaxHP = MaxHP;
    }
    #endregion
}

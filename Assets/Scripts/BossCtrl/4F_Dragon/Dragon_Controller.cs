using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IceDragon_State
{
    NONE,
    ICE_IDLE,
    ICE_MOVE,
}

public enum ThunderDragon_State
{
    NONE,
    THUNDER_IDLE,
    THUNDER_MOVE,
}

public enum FireDragon_State
{
    NONE,
    FIRE_IDLE,
    FIRE_MOVE,
}

public class Dragon_Controller : Boss_BehaviorCtrl_Base
{
    #region Variable
    [Header("-----Dragon State-----")]
    public bool isLock;               // 각도 조절 여부
    public bool isAttacking;          // 공격 중 인지 여부
    [SerializeField]
    bool isThink;

    public IceDragon_State IceDragonState;
    public ThunderDragon_State ThunderDragonState;
    public FireDragon_State FireDragonState;

    [Header("-----Dragon Reference-----")]
    public Boss_HP_Controller boss_hp_ctrl;  // HP 컨트롤러
    [SerializeField]
    Dragon_ObjPool Dragon_ObjPoolRef;

    [Header("-----Dragon Variable-----")]
    public GameObject Target;       // 플레이어
    public float TargetDistance;    // 플레이어와의 거리
    [SerializeField]
    int NextSkillNum;
    [SerializeField]
    float Slow_RotSpeed;

    [Header("-----Dragon State Variable-----")]
    public int MaxHP;   // 드래곤 체력

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

    [Header("-----Animation Var-----")]
    public Animator Dragon_animator;   // 애니메이터
    public bool isMove;         // 이동 여부
    #endregion

    #region Dragon_Rotate
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

    public void Reaper_Lock()
    {
        isLock = true;
    }

    public void Reaper_UnLock()
    {
        isLock = false;
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
            Dragon_animator.SetBool("isMove", isMove);
            // 앞으로 이동
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
        //else if (!isMove && isAttacking == false)
        //{
        //    reaperState = ReaperState.Idle;
        //    Reaper_animator.SetBool("isMove", isMove);
        //}
    }

    public void NotMove()
    {
        if (Target == null)
            return;

        isMove = false;

        if (!isMove && isAttacking == false)
        {
            transform.Translate(Vector3.forward * 0.0f * Time.deltaTime);
            //animator.SetFloat("Locomotion", 0.5f);
            Dragon_animator.SetBool("isMove", false);
        }
    }
    #endregion

    #region Start/Update
    // Start is called before the first frame update
    void Start()
    {
        boss_hp_ctrl = GetComponent<Boss_HP_Controller>();
        Dragon_ObjPoolRef = GetComponent<Dragon_ObjPool>();
        Dragon_animator = GetComponent<Animator>();

        // 최대 체력 전달 현재 체력 
        boss_hp_ctrl.BossMaxHP = MaxHP;
    }

    private void FixedUpdate()
    {
        // 현재 상태가 움직이는 Move고 대상이 멀면
        if (TargetDistance >= Skill_Think_Range)
        {
            Move();
        }
        else if (TargetDistance < Skill_Think_Range + 1.0f && !isAttacking) // 공격중이 아니고 사정거리안에 들어 왔을 시
        {
            NotMove();
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

    #region Dragon_FindTarget
    public void FindTarget()
    {

    }

    #endregion
}

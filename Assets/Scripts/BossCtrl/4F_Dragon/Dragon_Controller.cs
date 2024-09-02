using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CurentElement_State
{
    THUNDER_DRAGON,
    ICE_DRAGON,
    FIRE_DRAGON,
}


public enum IceDragon_State
{
    NONE,
    ICE_IDLE,
    ICE_MOVE,
    END
}

public enum ThunderDragon_State
{
    NONE,
    THUNDER_IDLE,
    THUNDER_MOVE,
    END
}

public enum FireDragon_State
{
    NONE,
    FIRE_IDLE,
    FIRE_MOVE,
    END
}

public class Dragon_Controller : Boss_BehaviorCtrl_Base
{
    #region Variable
    [Header("-----Dragon State-----")]
    public bool isLock;               // 각도 조절 여부
    public bool isAttacking;          // 공격 중 인지 여부
    [SerializeField]
    bool isThink;

    public CurentElement_State CurrentElement;
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
    public float ChaseDistance; // 스킬 시전 가능 범위
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

        CurrentElement = CurentElement_State.THUNDER_DRAGON;

        ThunderDragonState = ThunderDragon_State.THUNDER_IDLE;
        IceDragonState = IceDragon_State.NONE;
        FireDragonState = FireDragon_State.NONE;

        isMove = false;

        // 최대 체력 전달 현재 체력 
        boss_hp_ctrl.BossMaxHP = MaxHP;
    }

    private void FixedUpdate()
    {
        // 현재 상태가 움직이는 Move고 대상이 멀면
        if (TargetDistance >= ChaseDistance)
        {
            Move();
        }
        else if (TargetDistance < ChaseDistance + 1.0f && !isAttacking) // 공격중이 아니고 사정거리안에 들어 왔을 시
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

    #region Dragon_NextAct

    public void Dragon_NextAct()
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
        if (CurrentElement == CurentElement_State.THUNDER_DRAGON)
        {
            // 랜덤으로 다음 상태 변경
            ThunderDragon_State randomThunderState = (ThunderDragon_State)Random.Range(2, (int)ThunderDragon_State.END - 1);
            // Treant_Normal_State randomNormalState = (Treant_Normal_State)6;
            ThunderDragonState = randomThunderState;

            //Debug.Log(randomNormalState);
        }
        else if (CurrentElement == CurentElement_State.ICE_DRAGON)
        {
            // 랜덤으로 다음 상태 변경
            IceDragon_State randomIceState = (IceDragon_State)Random.Range(2, (int)IceDragon_State.END - 1);
            // Treant_Power_State randomPowerState = (Treant_Power_State)5;
            IceDragonState = randomIceState;

            //Debug.Log(randomPowerState);

        }
        else if (CurrentElement == CurentElement_State.FIRE_DRAGON)
        {
            // 랜덤으로 다음 상태 변경
            FireDragon_State randomFireState = (FireDragon_State)Random.Range(2, (int)FireDragon_State.END - 1);
            // Treant_Speed_State randomSpeedState = (Treant_Speed_State)2;
            FireDragonState = randomFireState;

            //Debug.Log(randomSpeedState);
        }

        // 기본 상태가 아닐 시 2초간의 딜레이를 준다
        //yield return new WaitForSeconds(0.0f);

        // 대상이 사정 거리보다 멀면 이동 선택
        if (TargetDistance >= ChaseDistance + 1.0f)
        {
            if (CurrentElement == CurentElement_State.THUNDER_DRAGON)
            {
                ThunderDragonState = ThunderDragon_State.THUNDER_MOVE;
            }
            else if (CurrentElement == CurentElement_State.ICE_DRAGON)
            {
                IceDragonState = IceDragon_State.ICE_MOVE;
            }
            else
            {
                FireDragonState = FireDragon_State.FIRE_MOVE;
            }
        }

        // 폼 체인지 가능할때
        //if (isStartFormChange && FormChange_Count == ChangeForm_Skill_Max_Count)
        //{
        //    if (Treant_Type == TreantType.NORMAL)
        //    {
        //        TreantNormalState = Treant_Normal_State.FORMCHANGE;
        //    }
        //    else if (Treant_Type == TreantType.POWER)
        //    {
        //        TreantPowerState = Treant_Power_State.FORMCHANGE;
        //    }
        //    else
        //    {
        //        TreantSpeedState = Treant_Speed_State.FORMCHANGE;
        //    }

        //    // 변신 했으니 0으로 초기화
        //    FormChange_Count = 0;

        switch (CurrentElement)
        {
            // 번개 폼일 때 스킬
            case CurentElement_State.THUNDER_DRAGON:
                switch (ThunderDragonState)
                {
                    case ThunderDragon_State.THUNDER_MOVE:
                        Dragon_Move();
                        break;
                    default:
                        break;
                }
                break;
            // 얼음 폼일때
            case CurentElement_State.ICE_DRAGON:
                switch (IceDragonState)
                {
                    case IceDragon_State.ICE_MOVE:
                        Dragon_Move();
                        break;
                    default:
                        break;
                }
                break;
            // 불 폼일때
            case CurentElement_State.FIRE_DRAGON:
                switch (FireDragonState)
                {
                    case FireDragon_State.FIRE_MOVE:
                        Dragon_Move();
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

    #region Dragon_Idle
    public void Dragon_Idle()
    {
        // 생각중이면
        if (isThink)
        {
            isThink = false;
            // return;
        }

        if (!isThink && GameManager.GMInstance.Get_PlaySceneManager().isRaidStart == true)
        {
            Dragon_NextAct();
        }
    }
    #endregion

    #region Dragon_Move
    public void Dragon_Move()
    {
        isMove = true;
        isThink = false;
    }
    #endregion

    #region Dragon_FindTarget
    public void FindTarget()
    {

    }

    #endregion
}

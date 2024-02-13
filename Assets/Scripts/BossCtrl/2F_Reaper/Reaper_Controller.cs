using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reaper_Controller : Boss_BehaviorCtrl_Base
{
    public GameObject Target;   // 플레이어
    Vector3 dir; // 각도

    Rigidbody rigid;

    [SerializeField]
    float Boss_RotSpeed;
    [SerializeField]
    float moveSpeed;
    public bool isLock;               // 각도 조절 여부
    public bool isAttacking;          // 공격 중 인지 여부

    [Header("-----Animation Var-----")]
    public Animator Reaper_animator;   // 애니메이터

    public bool isChase;
    public bool isMove;         // 이동 여부
    public bool isTargetFind;   // 첫 조우 여부

    public override void LookAtPlayer()
    {
        // 플레이어를 찾을 수 없다면 실행 안함
        if (Target == null)
            return;

        // 플레이어를 바라보도록
        // this.transform.LookAt(Target.transform);

        dir = Target.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * Boss_RotSpeed);
    }

    public override void Move()
    {
        // 플레이어를 찾을 수 없다면 실행 안함
        if (Target == null)
            return;

        Reaper_animator.SetBool("isMove", isMove);

        if (isMove)
        {
            // 앞으로 이동
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Reaper_animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();

        // Rigidbody의 회전을 고정
        rigid.constraints = RigidbodyConstraints.FreezeRotation;
        rigid.constraints = RigidbodyConstraints.FreezePositionY;
    }

    private void FixedUpdate()
    {
        Move();
    }

    // Update is called once per frame
    void Update()
    {
        if (isLock == false)
        {
            LookAtPlayer();
        }
    }

    #region Animation Event
    public void FindAnim_To_Idle()
    {
        // 타켓 찾았으니 false
        isTargetFind = false;
        // Idle 모션으로 변경
        Reaper_animator.SetBool("isFind", isTargetFind);

        isMove = true;
    }
    
    

    // 추격 여부
    public void Target_Chase()
    {
        if (isChase)
        {
            isMove = true;
            Reaper_animator.SetBool("isMove", isMove);
        }
    }
    #endregion

    #region Boss_BaseAtk
    // 보스 회전 멈춤
    public void AnimRotate_Lock()
    {
        isLock = true;
        isAttacking = true;

        StartCoroutine(AnimRotate_UnLock());
    }

    // 보스 회전
    IEnumerator AnimRotate_UnLock()
    {
        yield return new WaitForSeconds(2.5f);
        isLock = false;
        isAttacking = false;
    }
    #endregion
}

using UnityEngine;
using System.Collections;
using static UnityEngine.GraphicsBuffer;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

public class BossSkillP : MonoBehaviour
{
    public Boss_HP_Controller boss_hp_ctrl;

    [SerializeField]
    IronGuard_ObjPool objPool;
    public BossAnimator bossAnimator;
    public GameObject gameClearCanvas;
    public RectTransform gameClearCanvasRect; // 게임 클리어 캔버스의 RectTransform

    // public HPtest hp;
    public int IronGuard_MaxHP;

    public GameObject SpiritEffect;
    public GameObject JumpEffect;
    public GameObject DownAttackRange;

    //public GameObject SpiritEffect;
    //public GameObject SpiritEffectPrefab; // SpiritEffect 프리팹 변수 선언

    public BoxCollider boxCollider; //보스 히트 범위
    public CapsuleCollider JumpAttackRange;

    public GameObject Target;
    public Animator animator;
    public BossLookAt bossLookAt;

    public GameObject BossObj;

    public ShotRazer shotRazer_1;
    public ShotRazer shotRazer_2;
    public ShotRazer shotRazer_3;
    public ShotRazer shotRazer_4;
    public ShotRazer shotRazer_5;
    public ShotRazer shotRazer_6;
    public ShotRazer shotRazer_7;
    public ShotRazer shotRazer_8;

    public Transform SpiritPos;
    public Transform JumpPos;

    Transform bossPos;

    
    public GameObject[] razerMaker;
    // public GameObject razerMaker_2;
    [SerializeField]
    GameObject Razer;
    [SerializeField]
    GameObject[] RazerOBJ_Effect;
    // GameObject RazerOBJ_Effect_2;
    [SerializeField]
    int[] SelectRazerNum;

    [SerializeField]
    GameObject[] GuideLine;

    public bool isDead = false;
    [SerializeField]
    Transform Razer_SpawnTr;
    [SerializeField]
    Transform ThrowSword_SpawnTr;
    [SerializeField]
    Transform DownSword_SpawnTr;
    [SerializeField]
    Transform TargetTr;

    void Awake()
    {
        objPool = GetComponent<IronGuard_ObjPool>();
        animator = GetComponentInChildren<Animator>();
        boxCollider = GetComponent<BoxCollider>();
        bossPos = GetComponent<Transform>();
        gameClearCanvas.SetActive(false);
    }

    void Start()
    {
        boss_hp_ctrl = GetComponent<Boss_HP_Controller>();
        // 최대 체력 전달 현재 체력 
        boss_hp_ctrl.BossCurHP = IronGuard_MaxHP;

        StartCoroutine(Think());
    }

    void Update()
    {
        if (boss_hp_ctrl.isDead) // HPtest 스크립트의 isDead를 사용합니다.
        {
            StopAllCoroutines();
            return;
        }
    }

    public void Death()
    {
        StopAllCoroutines();
        this.GetComponent<BossLookAt>().isLook = false;
    }

    public void BossThink()
    {
        StartCoroutine(Think());
    }

    IEnumerator Think()
    {
        yield return new WaitForSeconds(0.1f);

        int ranAction = Random.Range(0, 4);

        switch (ranAction)
        {
            case 0:
                //검기 패턴
                StartCoroutine(BossSkill1());
                break;

            case 1:
                //3번 연속 내려찍기 패턴
                StartCoroutine(BossSkill2());
                break;

            case 2:
                //점프 공격 패턴
                StartCoroutine(BossSkill3());
                break;

            case 3:
                //레이저 공격(꼭지점) 패턴
                StartCoroutine(BossSkill4());
                break;

            //case 4:
            //    //레이저 공격(모서리) 패턴
            //    StartCoroutine(BossSkill5());
            //    break;
        }
    }
    // TODO ## IronGuard_Skill1 검기발사
    #region IronGuard_Skill1
    IEnumerator BossSkill1()
    {
        animator.SetTrigger("doSpirit");

        yield return new WaitForSeconds(1f);

        // Vector3 targetDirection = Target.transform.position - SpiritPos.position;

        Vector3 targetTrDirection = Target.transform.GetChild(0).transform.position - SpiritPos.position;
        // GameObject SpiritEffectPrf = Instantiate(SpiritEffect);

        GameObject SpiritEffectPrf = objPool.Get_SpiritSword_ObjectFromPool();
        SpiritEffectPrf.transform.position = SpiritPos.position + SpiritPos.forward * 5.0f;
        SpiritEffectPrf.transform.rotation = Quaternion.LookRotation(targetTrDirection);
        // Vector3 SpiritEffectPrf_Quaternion = SpiritEffectPrf.transform.rotation.eulerAngles;
        // Destroy(SpiritEffectPrf, 15.0f);
        // SpiritEffectPrf.transform.forward = targetDirection;

        yield return new WaitForSeconds(0.3f);
        StartCoroutine(GuideLineF(SpiritEffectPrf));
        SpiritEffectPrf.transform.GetChild(4).gameObject.SetActive(true);
        // SpiritEffectPrf.transform.rotation = Quaternion.Euler(SpiritEffectPrf.transform.rotation.x, SpiritEffectPrf.transform.rotation.y, 90.0f);

        yield return new WaitForSeconds(0.1f);

        bossLookAt.isLook = false;

        yield return new WaitForSeconds(1.9f);
        // SpiritEffectPrf.transform.GetChild(4).gameObject.SetActive(false);
        SpiritEffectPrf.GetComponent<SwordParticle_Eff>().isBig = true;
        SpiritEffectPrf.GetComponent<SwordParticle_Eff>().isShot = true;

        // Rigidbody SpiritEffect_1 = SpiritEffectPrf.GetComponent<Rigidbody>();
        // SpiritEffect_1.velocity = targetDirection * 5;

        bossLookAt.isLook = true;
        yield return new WaitForSeconds(2f);

        StartCoroutine(Think());
    }

    IEnumerator GuideLineF(GameObject _obj)
    {
        yield return new WaitForSeconds(0.5f);
        _obj.transform.GetChild(4).gameObject.SetActive(false);
    }
    #endregion

    // TODO ## IronGuard_Skill2 점프 어택
    #region IronGuard_Skill2
    IEnumerator BossSkill2()
    {
        animator.SetTrigger("doDownAttack");
        //
        yield return new WaitForSeconds(1.1f);

        GuideLine[1].SetActive(true);
        bossLookAt.isLook = false;
        Vector3 targetDirection_1 = Target.transform.position - JumpPos.position;

        yield return new WaitForSeconds(0.35f);

        GameObject DownAttackRange_1 = objPool.GetObjectFromPool();

        // 오브젝트 풀로 사리질 때 적용시킨 collider를 다시 킨다.
        if (DownAttackRange_1.GetComponent<BoxCollider>().enabled == false)
        {
            DownAttackRange_1.GetComponent<BoxCollider>().enabled = true;
        }

        GuideLine[1].SetActive(false);
        StartCoroutine(DownEffect_ActiveF(DownAttackRange_1));
        DownAttackRange_1.transform.position = JumpPos.transform.position + JumpPos.transform.forward * 30.0f;
        DownAttackRange_1.transform.position = new Vector3(DownAttackRange_1.transform.position.x, 1.0f, DownAttackRange_1.transform.position.z);
        // DownAttackRange_1.transform.rotation = Quaternion.LookRotation(transform.forward);
        DownAttackRange_1.transform.parent = DownSword_SpawnTr;

        //PoolEffectObj.Add(DownAttackRange_1);
        // Destroy(DownAttackRange_1, 10.0f);
        //DownAttackRange_1.transform.forward = targetDirection_1;
        //Vector3 currentEulerAngles_1 = DownAttackRange_1.transform.rotation.eulerAngles;
        //DownAttackRange_1.transform.rotation = Quaternion.Euler(currentEulerAngles_1.x, currentEulerAngles_1.y, 0);
        DownAttackRange_1.transform.rotation = Quaternion.LookRotation(transform.forward);

        yield return new WaitForSeconds(0.1f);

        bossLookAt.isLook = true;

        yield return new WaitForSeconds(1.2f);

        GuideLine[1].SetActive(true);
        bossLookAt.isLook = false;
        Vector3 targetDirection_2 = Target.transform.position - JumpPos.position;

        yield return new WaitForSeconds(0.35f);
        GuideLine[1].SetActive(false);
        GameObject DownAttackRange_2 = objPool.GetObjectFromPool();

        // 오브젝트 풀로 사리질 때 적용시킨 collider를 다시 킨다.
        if (DownAttackRange_2.GetComponent<BoxCollider>().enabled == false)
        {
            DownAttackRange_2.GetComponent<BoxCollider>().enabled = true;
        }

        StartCoroutine(DownEffect_ActiveF(DownAttackRange_2));
        DownAttackRange_2.transform.position = JumpPos.transform.position + JumpPos.transform.forward * 30.0f;
        DownAttackRange_2.transform.position = new Vector3(DownAttackRange_2.transform.position.x, 1.0f, DownAttackRange_2.transform.position.z);
        DownAttackRange_2.transform.parent = DownSword_SpawnTr;

        // PoolEffectObj.Add(DownAttackRange_2);
        //Destroy(DownAttackRange_2, 10.0f);
        //DownAttackRange_2.transform.forward = targetDirection_2;
        //Vector3 currentEulerAngles_2 = DownAttackRange_2.transform.rotation.eulerAngles;
        //DownAttackRange_2.transform.rotation = Quaternion.Euler(currentEulerAngles_2.x, currentEulerAngles_2.y, 0);
        DownAttackRange_2.transform.rotation = Quaternion.LookRotation(transform.forward);

        yield return new WaitForSeconds(0.1f);

        // GuideLine[1].SetActive(true);
        bossLookAt.isLook = true;

        yield return new WaitForSeconds(1.2f);

        GuideLine[1].SetActive(true);
        bossLookAt.isLook = false;
        Vector3 targetDirection_3 = Target.transform.position - JumpPos.position;

        yield return new WaitForSeconds(0.7f);
        GuideLine[1].SetActive(false);
        GameObject DownAttackRange_3 = objPool.GetObjectFromPool();

        // 오브젝트 풀로 사리질 때 적용시킨 collider를 다시 킨다.
        if (DownAttackRange_3.GetComponent<BoxCollider>().enabled == false)
        {
            DownAttackRange_3.GetComponent<BoxCollider>().enabled = true;
        }

        StartCoroutine(DownEffect_ActiveF(DownAttackRange_3));
        DownAttackRange_3.transform.position = JumpPos.transform.position + JumpPos.transform.forward * 30.0f;
        DownAttackRange_3.transform.position = new Vector3(DownAttackRange_3.transform.position.x, 1.0f, DownAttackRange_3.transform.position.z);
        DownAttackRange_3.transform.parent = DownSword_SpawnTr;

        // PoolEffectObj.Add(DownAttackRange_3);
        //Destroy(DownAttackRange_3, 10.0f);
        //DownAttackRange_3.transform.forward = targetDirection_3;
        //Vector3 currentEulerAngles_3 = DownAttackRange_3.transform.rotation.eulerAngles;
        //DownAttackRange_3.transform.rotation = Quaternion.Euler(currentEulerAngles_3.x, currentEulerAngles_3.y, 0);
        DownAttackRange_3.transform.rotation = Quaternion.LookRotation(transform.forward);

        yield return new WaitForSeconds(1f);

        // GuideLine[1].SetActive(true);
        bossLookAt.isLook = true;
        animator.SetTrigger("doDownAttack");

        yield return new WaitForSeconds(1.1f);

        GuideLine[1].SetActive(true);
        bossLookAt.isLook = false;
        Vector3 targetDirection_4 = Target.transform.position - JumpPos.position;

        yield return new WaitForSeconds(0.35f);
        GuideLine[1].SetActive(false);
        GameObject DownAttackRange_4 = objPool.GetObjectFromPool();

        // 오브젝트 풀로 사리질 때 적용시킨 collider를 다시 킨다.
        if (DownAttackRange_4.GetComponent<BoxCollider>().enabled == false)
        {
            DownAttackRange_4.GetComponent<BoxCollider>().enabled = true;
        }

        StartCoroutine(DownEffect_ActiveF(DownAttackRange_4));
        DownAttackRange_4.transform.position = JumpPos.transform.position + JumpPos.transform.forward * 30.0f;
        DownAttackRange_4.transform.position = new Vector3(DownAttackRange_4.transform.position.x, 1.0f, DownAttackRange_4.transform.position.z);
        DownAttackRange_4.transform.parent = DownSword_SpawnTr;

        //PoolEffectObj.Add(DownAttackRange_4);
        //Destroy(DownAttackRange_4, 10.0f);
        //DownAttackRange_4.transform.forward = targetDirection_4;
        //Vector3 currentEulerAngles_4 = DownAttackRange_4.transform.rotation.eulerAngles;
        //DownAttackRange_4.transform.rotation = Quaternion.Euler(currentEulerAngles_4.x, currentEulerAngles_4.y, 0);
        DownAttackRange_4.transform.rotation = Quaternion.LookRotation(transform.forward);

        yield return new WaitForSeconds(0.1f);

        // GuideLine[1].SetActive(true);
        bossLookAt.isLook = true;

        yield return new WaitForSeconds(1.2f);

        GuideLine[1].SetActive(true);
        bossLookAt.isLook = false;
        Vector3 targetDirection_5 = Target.transform.position - JumpPos.position;

        yield return new WaitForSeconds(0.35f);

        GuideLine[1].SetActive(false);
        GameObject DownAttackRange_5 = objPool.GetObjectFromPool();

        // 오브젝트 풀로 사리질 때 적용시킨 collider를 다시 킨다.
        if (DownAttackRange_5.GetComponent<BoxCollider>().enabled == false)
        {
            DownAttackRange_5.GetComponent<BoxCollider>().enabled = true;
        }

        StartCoroutine(DownEffect_ActiveF(DownAttackRange_5));
        DownAttackRange_5.transform.position = JumpPos.transform.position + JumpPos.transform.forward * 30.0f;
        DownAttackRange_5.transform.position = new Vector3(DownAttackRange_5.transform.position.x, 1.0f, DownAttackRange_5.transform.position.z);
        DownAttackRange_5.transform.parent = DownSword_SpawnTr;

        // PoolEffectObj.Add(DownAttackRange_5);
        // Destroy(DownAttackRange_5, 10.0f);
        //DownAttackRange_5.transform.forward = targetDirection_5;
        //Vector3 currentEulerAngles_5 = DownAttackRange_5.transform.rotation.eulerAngles;
        //DownAttackRange_5.transform.rotation = Quaternion.Euler(currentEulerAngles_5.x, currentEulerAngles_5.y, 0);
        DownAttackRange_5.transform.rotation = Quaternion.LookRotation(transform.forward);

        yield return new WaitForSeconds(0.1f);

        // GuideLine[1].SetActive(true);
        bossLookAt.isLook = true;

        yield return new WaitForSeconds(1.2f);

        GuideLine[1].SetActive(true);
        bossLookAt.isLook = false;
        Vector3 targetDirection_6 = Target.transform.position - JumpPos.position;

        yield return new WaitForSeconds(0.7f);
        GuideLine[1].SetActive(false);
        GameObject DownAttackRange_6 = objPool.GetObjectFromPool();

        // 오브젝트 풀로 사리질 때 적용시킨 collider를 다시 킨다.
        if (DownAttackRange_6.GetComponent<BoxCollider>().enabled == false)
        {
            DownAttackRange_6.GetComponent<BoxCollider>().enabled = true;
        }

        StartCoroutine(DownEffect_ActiveF(DownAttackRange_6));
        DownAttackRange_6.transform.position = JumpPos.transform.position + JumpPos.transform.forward * 30.0f;
        DownAttackRange_6.transform.position = new Vector3(DownAttackRange_6.transform.position.x, 1.0f, DownAttackRange_6.transform.position.z);
        DownAttackRange_6.transform.parent = DownSword_SpawnTr;

        // PoolEffectObj.Add(DownAttackRange_6);
        // Destroy(DownAttackRange_6, 10.0f);
        //DownAttackRange_6.transform.forward = targetDirection_6;
        //Vector3 currentEulerAngles_6 = DownAttackRange_6.transform.rotation.eulerAngles;
        //DownAttackRange_6.transform.rotation = Quaternion.Euler(currentEulerAngles_6.x, currentEulerAngles_6.y, 0);
        DownAttackRange_6.transform.rotation = Quaternion.LookRotation(transform.forward);

        yield return new WaitForSeconds(1f);
        //GuideLine[1].SetActive(true);
        bossLookAt.isLook = true;
        animator.SetTrigger("doDownAttack");

        yield return new WaitForSeconds(1.1f);

        GuideLine[1].SetActive(true);
        bossLookAt.isLook = false;
        Vector3 targetDirection_7 = Target.transform.position - JumpPos.position;

        yield return new WaitForSeconds(0.35f);
        GuideLine[1].SetActive(false);
        GameObject DownAttackRange_7 = objPool.GetObjectFromPool();

        // 오브젝트 풀로 사리질 때 적용시킨 collider를 다시 킨다.
        if (DownAttackRange_7.GetComponent<BoxCollider>().enabled == false)
        {
            DownAttackRange_7.GetComponent<BoxCollider>().enabled = true;
        }

        StartCoroutine(DownEffect_ActiveF(DownAttackRange_7));
        DownAttackRange_7.transform.position = JumpPos.transform.position + JumpPos.transform.forward * 30.0f;
        DownAttackRange_7.transform.position = new Vector3(DownAttackRange_7.transform.position.x, 1.0f, DownAttackRange_7.transform.position.z);
        DownAttackRange_7.transform.parent = DownSword_SpawnTr;

        //PoolEffectObj.Add(DownAttackRange_7);
        // Destroy(DownAttackRange_7, 10.0f);
        //DownAttackRange_7.transform.forward = targetDirection_7;
        //Vector3 currentEulerAngles_7 = DownAttackRange_7.transform.rotation.eulerAngles;
        //DownAttackRange_7.transform.rotation = Quaternion.Euler(currentEulerAngles_7.x, currentEulerAngles_7.y, 0);
        DownAttackRange_7.transform.rotation = Quaternion.LookRotation(transform.forward);

        yield return new WaitForSeconds(0.1f);
        //GuideLine[1].SetActive(true);
        bossLookAt.isLook = true;

        yield return new WaitForSeconds(1.2f);

        GuideLine[1].SetActive(true);
        bossLookAt.isLook = false;
        Vector3 targetDirection_8 = Target.transform.position - JumpPos.position;

        yield return new WaitForSeconds(0.35f);
        GuideLine[1].SetActive(false);
        GameObject DownAttackRange_8 = objPool.GetObjectFromPool();

        // 오브젝트 풀로 사리질 때 적용시킨 collider를 다시 킨다.
        if (DownAttackRange_8.GetComponent<BoxCollider>().enabled == false)
        {
            DownAttackRange_8.GetComponent<BoxCollider>().enabled = true;
        }

        StartCoroutine(DownEffect_ActiveF(DownAttackRange_8));
        DownAttackRange_8.transform.position = JumpPos.transform.position + JumpPos.transform.forward * 30.0f;
        DownAttackRange_8.transform.position = new Vector3(DownAttackRange_8.transform.position.x, 1.0f, DownAttackRange_8.transform.position.z);
        DownAttackRange_8.transform.parent = DownSword_SpawnTr;

        // PoolEffectObj.Add(DownAttackRange_8);
        // Destroy(DownAttackRange_8, 10.0f);
        //DownAttackRange_8.transform.forward = targetDirection_8;
        //Vector3 currentEulerAngles_8 = DownAttackRange_8.transform.rotation.eulerAngles;
        //DownAttackRange_8.transform.rotation = Quaternion.Euler(currentEulerAngles_8.x, currentEulerAngles_8.y, 0);
        DownAttackRange_8.transform.rotation = Quaternion.LookRotation(transform.forward);

        yield return new WaitForSeconds(0.1f);
        //GuideLine[1].SetActive(true);
        bossLookAt.isLook = true;

        yield return new WaitForSeconds(1.2f);

        GuideLine[1].SetActive(true);
        bossLookAt.isLook = false;
        Vector3 targetDirection_9 = Target.transform.position - JumpPos.position;

        yield return new WaitForSeconds(0.7f);
        GuideLine[1].SetActive(false);
        GameObject DownAttackRange_9 = objPool.GetObjectFromPool();

        // 오브젝트 풀로 사리질 때 적용시킨 collider를 다시 킨다.
        if (DownAttackRange_9.GetComponent<BoxCollider>().enabled == false)
        {
            DownAttackRange_9.GetComponent<BoxCollider>().enabled = true;
        }

        StartCoroutine(DownEffect_ActiveF(DownAttackRange_9));
        DownAttackRange_9.transform.position = JumpPos.transform.position + JumpPos.transform.forward * 30.0f;
        DownAttackRange_9.transform.position = new Vector3(DownAttackRange_9.transform.position.x, 1.0f, DownAttackRange_9.transform.position.z);
        DownAttackRange_9.transform.parent = DownSword_SpawnTr;

        //PoolEffectObj.Add(DownAttackRange_9);
        //Destroy(DownAttackRange_9, 10.0f);
        //DownAttackRange_9.transform.forward = targetDirection_9;
        //Vector3 currentEulerAngles_9 = DownAttackRange_9.transform.rotation.eulerAngles;
        //DownAttackRange_9.transform.rotation = Quaternion.Euler(currentEulerAngles_9.x, currentEulerAngles_9.y, 0);
        DownAttackRange_9.transform.rotation = Quaternion.LookRotation(transform.forward);

        yield return new WaitForSeconds(0.1f);

        bossLookAt.isLook = true;

        yield return new WaitForSeconds(2f);

        bossLookAt.isLook = true;
        StartCoroutine(Think());
    }

    public void Combo3_JumpAtk()
    {
        StartCoroutine(Combo3_Jump());
    }

    // 이펙트 안보이게
    IEnumerator DownEffect_ActiveF(GameObject effect)
    {
        yield return new WaitForSeconds(1.0f);
        effect.GetComponent<BoxCollider>().enabled = false;

        yield return new WaitForSeconds(2.0f);
        effect.SetActive(false);
    }

    IEnumerator Combo3_Jump()
    {
        bossLookAt.isLook = false;
        Vector3 targetDirection_1 = Target.transform.position - JumpPos.position;;

        yield return new WaitForSeconds(0.3f);

        GameObject DownAttackRange_1 = objPool.GetObjectFromPool();
        StartCoroutine(DownEffect_ActiveF(DownAttackRange_1));
        DownAttackRange_1.transform.position = JumpPos.position;
        DownAttackRange_1.transform.rotation = Quaternion.identity;
        DownAttackRange_1.transform.parent = DownSword_SpawnTr;


        //PoolEffectObj.Add(DownAttackRange_1);
        // Destroy(DownAttackRange_1, 10.0f);
        DownAttackRange_1.transform.forward = targetDirection_1;
        Vector3 currentEulerAngles_1 = DownAttackRange_1.transform.rotation.eulerAngles;
        DownAttackRange_1.transform.rotation = Quaternion.Euler(currentEulerAngles_1.x, currentEulerAngles_1.y - 90, 0);

        yield return new WaitForSeconds(0.1f);

        bossLookAt.isLook = true;

        yield return new WaitForSeconds(1.2f);
    }

    #endregion

    // TODO ## IronGuard_Skill3 9번 찍기
    #region IronGuard_Skill3
    IEnumerator BossSkill3()
    {  
        Vector3 jumpStartPosition = transform.position;
        Vector3 targetDirection = Target.transform.position - transform.position;

        Vector3 jumpEndAttackVec = Target.transform.GetChild(0).transform.position - targetDirection.normalized * 5.0f;

        // 가이드 라인 표시

        bossLookAt.isLook = false;
        boxCollider.enabled = false;

        GuideLine[0].SetActive(true);
        GuideLine[0].transform.position = jumpEndAttackVec + bossPos.forward * 15.0f;

        yield return new WaitForSeconds(0.3f);
        // 가이드 라인 제거
        GuideLine[0].SetActive(false);
        // 해당위치로 공격실행
        StartCoroutine(JumpDuring(jumpStartPosition, jumpEndAttackVec, 0.5f));

        // 점프 거리
        // Debug.Log(Vector3.Distance(jumpStartPosition, jumpEndAttackVec));

       

        animator.SetTrigger("doJumpAttack");

        yield return new WaitForSeconds(1.2f);

        // JumpAttackRange.enabled = true;
        Vector3 bossForward = bossPos.position + bossPos.forward * 15.0f;
        GameObject newPrefab = objPool.Get_JumpAtk_ObjectFromPool();
        newPrefab.transform.position = bossForward;
        newPrefab.transform.rotation = Quaternion.identity;
        // GameObject newPrefab = Instantiate(JumpEffect, bossForward, Quaternion.identity);
        // Destroy(newPrefab, 2.0f);

        yield return new WaitForSeconds(0.5f);

        if (Vector3.Distance(jumpStartPosition, jumpEndAttackVec) >= 0.0f && Vector3.Distance(jumpStartPosition, jumpEndAttackVec) <= 20.0f)
        {
            animator.SetTrigger("doReturn");
            animator.SetFloat("ReturnSpeed", 0.8f);
            yield return new WaitForSeconds(0.2f);
        }
        else if (Vector3.Distance(jumpStartPosition, jumpEndAttackVec) > 20.0f && Vector3.Distance(jumpStartPosition, jumpEndAttackVec) <= 40.0f)
        {
            animator.SetTrigger("doReturn");
            animator.SetFloat("ReturnSpeed", 0.9f);
            yield return new WaitForSeconds(0.35f);
        }
        else if (Vector3.Distance(jumpStartPosition, jumpEndAttackVec) > 40.0f && Vector3.Distance(jumpStartPosition, jumpEndAttackVec) <= 60.0f)
        {
            animator.SetTrigger("doReturn");
            animator.SetFloat("ReturnSpeed", 0.8f);
            yield return new WaitForSeconds(0.4f);
        }
        else if (Vector3.Distance(jumpStartPosition, jumpEndAttackVec) > 60.0f)
        {
            animator.SetTrigger("doReturn");
            animator.SetFloat("ReturnSpeed", 0.7f);
            yield return new WaitForSeconds(0.5f);
        }

        boxCollider.enabled = true;
        // animator.SetTrigger("doReturn");
        StartCoroutine(JumpDuring(jumpEndAttackVec, jumpStartPosition, 1.0f));

        yield return new WaitForSeconds(0.8f);

        newPrefab.SetActive(false);
        // JumpAttackRange.enabled = false;
        // boxCollider.enabled = false;

        yield return new WaitForSeconds(1f);

        bossLookAt.isLook = true;
        boxCollider.enabled = true;

        yield return new WaitForSeconds(2f);
        StartCoroutine(Think());
    }

    IEnumerator JumpDuring(Vector3 startPosition, Vector3 jumpAttackVec, float duration)
    {
        float startTime = Time.time;

        while (Time.time - startTime < duration)
        {
            // Dodge 동작 중에 플레이어를 움직입니다.
            float t = (Time.time - startTime) / duration; // 시간 계산
            transform.position = Vector3.Lerp(startPosition, jumpAttackVec, t); // 시작 지점에서 도착 지점까지 t만큼 시간 소요
            yield return null;
        }
        transform.position = jumpAttackVec;
    }
    #endregion

    // TODO ## IronGuard_Skill4 랜덤 레이저 
    #region IronGuard_Skill4
    IEnumerator BossSkill4()
    {
        SelectRazerNum = GenerateUniqueRandomValues(4, 0, 7);
        Vector3 bossPosition = transform.position; // 보스 위치 값을 저장

        Razer.SetActive(true);
        // 랜덤으로 뽑힌 오브젝트에 있는 RazerMaker_Ctrl1의 bool값 조정
        Razer.GetComponent<RazerMaker_Ctrl1>().isRazerAtk = false;

        // 레이저 공격 실행 아직 안함
        for (int i = 0; i < SelectRazerNum.Length; i++)
        {
            // 랜덤으로 뽑힌 오브젝트 활성화
            razerMaker[SelectRazerNum[i]].SetActive(true);
            RazerOBJ_Effect[SelectRazerNum[i]].SetActive(true);
        }

        bossLookAt.isLook = false;
        
        animator.SetTrigger("doRazer");

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < SelectRazerNum.Length; i++)
        {
            // 랜덤으로 뽑힌 오브젝트 빔 발사
            razerMaker[SelectRazerNum[i]].transform.GetChild(0).GetComponent<ShotRazer>().isShot = true;
        }

        //shotRazer_1.isShot = true;
        //shotRazer_2.isShot = true;
        //shotRazer_3.isShot = true;
        //shotRazer_4.isShot = true;

        yield return new WaitForSeconds(0.5f);
        animator.SetTrigger("doRazerReturn");

        yield return new WaitForSeconds(1.5f);
        
        // RazerOBJ_Effect.SetActive(false);

        for (int i = 0; i < SelectRazerNum.Length; i++)
        {
            // 랜덤으로 뽑힌 오브젝트 빔 발사
            razerMaker[SelectRazerNum[i]].transform.GetChild(0).GetComponent<ShotRazer>().End_Razer_Atk();
            // 소환진 이펙트 끄기
            RazerOBJ_Effect[SelectRazerNum[i]].SetActive(false);
        }

        //shotRazer_1.End_Razer_Atk();
        //shotRazer_2.End_Razer_Atk();
        //shotRazer_3.End_Razer_Atk();
        //shotRazer_4.End_Razer_Atk();

        // 레이저 공격 실행
        Razer.GetComponent<RazerMaker_Ctrl1>().isRazerAtk = true;

        //bossLookAt.isLook = true;
        yield return new WaitForSeconds(2f);

        // 레이저 공격 실행 아직 안함
        for (int i = 0; i < SelectRazerNum.Length; i++)
        {
            // 랜덤으로 뽑힌 오브젝트 활성화
            razerMaker[SelectRazerNum[i]].SetActive(false);
        }

        StartCoroutine(Think());
    }

    int[] GenerateUniqueRandomValues(int count, int minValue, int maxValue)
    {
        if (count > maxValue - minValue + 1)
        {
            Debug.LogError("Count should be less than or equal to the range of unique values.");
            return null;
        }

        int[] values = new int[maxValue - minValue + 1];

        // 초기화: minValue부터 maxValue까지의 순차적인 값으로 배열 초기화
        for (int i = 0; i < values.Length; i++)
        {
            values[i] = minValue + i;
        }

        // Fisher-Yates 셔플 알고리즘을 사용하여 배열 섞기
        for (int i = values.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            int temp = values[i];
            values[i] = values[randomIndex];
            values[randomIndex] = temp;
        }

        // count 만큼의 값만 반환
        int[] result = new int[count];
        for (int i = 0; i < count; i++)
        {
            result[i] = values[i];
        }

        return result;
    }
    #endregion

    // TODO ## IronGuard_Skill5 레이저 전
    #region IronGuard_Skill5
    //IEnumerator BossSkill5()
    //{
    //    RazerOBJ_Effect_2.SetActive(true);
    //    Vector3 bossPosition = transform.position; // 보스 위치 값을 저장
    //    // 레이저 공격 실행 아직 안함
    //    razerMaker_2.GetComponent<RazerMaker_Ctrl1>().isRazerAtk = false;

    //    bossLookAt.isLook = false;

    //    razerMaker_2.SetActive(true);

    //    animator.SetTrigger("doRazer");

    //    // yield return new WaitForSeconds(3f);
    //    yield return new WaitForSeconds(1f);

    //    //shotRazer_5.UseRazer();
    //    //shotRazer_6.UseRazer();
    //    //shotRazer_7.UseRazer();
    //    //shotRazer_8.UseRazer();

    //    shotRazer_5.isShot = true;
    //    shotRazer_6.isShot = true;
    //    shotRazer_7.isShot = true;
    //    shotRazer_8.isShot = true;

    //    yield return new WaitForSeconds(0.5f);

    //    animator.SetTrigger("doRazerReturn");

    //    // yield return new WaitForSeconds(1f);
    //    yield return new WaitForSeconds(1.5f);
    //    RazerOBJ_Effect_2.SetActive(false);
    //    // 레이저 이펙트 초기화
    //    shotRazer_5.End_Razer_Atk();
    //    shotRazer_6.End_Razer_Atk();
    //    shotRazer_7.End_Razer_Atk();
    //    shotRazer_8.End_Razer_Atk();

    //    // 레이저 공격 실행
    //    razerMaker_2.GetComponent<RazerMaker_Ctrl1>().isRazerAtk = true;
    //    // razerMaker_2.SetActive(false);

    //    bossLookAt.isLook = true;

    //    yield return new WaitForSeconds(2f);

    //    StartCoroutine(Think());
    //}
    #endregion
}
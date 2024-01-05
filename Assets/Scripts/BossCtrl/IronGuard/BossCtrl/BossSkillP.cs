using UnityEngine;
using System.Collections;
using static UnityEngine.GraphicsBuffer;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

public class BossSkillP : MonoBehaviour
{
    [SerializeField]
    IronGuard_ObjPool objPool;
    public BossAnimator bossAnimator;
    public GameObject gameClearCanvas;
    public RectTransform gameClearCanvasRect; // 게임 클리어 캔버스의 RectTransform

    public HPtest hp;

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

    public GameObject razerMaker_1;
    public GameObject razerMaker_2;

    public bool isDead = false;
    [SerializeField]
    Transform Razer_SpawnTr;
    [SerializeField]
    Transform ThrowSword_SpawnTr;
    [SerializeField]
    Transform DownSword_SpawnTr;

    [Header("----IronGuard_Skill2_Var----")]
    [SerializeField] int JumpCount;

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
        StartCoroutine(Think());
    }

    void Update()
    {
        if (hp.isDead) // HPtest 스크립트의 isDead를 사용합니다.
        {
            StopAllCoroutines();
            return;
        }
    }



    public void BossThink()
    {
        StartCoroutine(Think());
    }

    IEnumerator Think()
    {
        yield return new WaitForSeconds(0.1f);

        int ranAction = Random.Range(0, 5);

        switch (1)
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

            case 4:
                //레이저 공격(모서리) 패턴
                StartCoroutine(BossSkill5());
                break;
        }
    }
    // TODO ## IronGuard_Skill1
    #region IronGuard_Skill1
    IEnumerator BossSkill1()
    {
        animator.SetTrigger("doSpirit");

        yield return new WaitForSeconds(1f);

        Vector3 targetDirection = Target.transform.position - SpiritPos.position;

        GameObject SpiritEffectPrf = Instantiate(SpiritEffect, SpiritPos.position, Quaternion.identity);
        Vector3 SpiritEffectPrf_Quaternion = SpiritEffectPrf.transform.rotation.eulerAngles;
        SpiritEffectPrf.transform.rotation = Quaternion.Euler(SpiritEffectPrf_Quaternion.x, SpiritEffectPrf_Quaternion.y, 90);
        Destroy(SpiritEffectPrf, 15.0f);

        SpiritEffectPrf.transform.forward = targetDirection;

        //SpiritEffectPrf.transform.rotation = Quaternion.Euler(SpiritEffectPrf.transform.rotation.x, SpiritEffectPrf.transform.rotation.y, 90);
        yield return new WaitForSeconds(0.1f);

        bossLookAt.isLook = false;

        yield return new WaitForSeconds(1.9f);

        Rigidbody SpiritEffect_1 = SpiritEffectPrf.GetComponent<Rigidbody>();
        SpiritEffect_1.velocity = targetDirection * 5;

        bossLookAt.isLook = true;
        yield return new WaitForSeconds(2f);

        StartCoroutine(Think());
    }
    #endregion

    // TODO ## IronGuard_Skill2
    #region IronGuard_Skill2
    IEnumerator BossSkill2()
    {
        animator.SetTrigger("doDownAttack");
        yield return new WaitForSeconds(1.3f);

        bossLookAt.isLook = false;
        Vector3 targetDirection_1 = Target.transform.position - JumpPos.position;

        yield return new WaitForSeconds(0.3f);

        GameObject DownAttackRange_1 = objPool.GetObjectFromPool();
        StartCoroutine(DownEffect_ActiveF(DownAttackRange_1));
        DownAttackRange_1.transform.position = JumpPos.transform.position + JumpPos.transform.forward * 30.0f;
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

        bossLookAt.isLook = false;
        Vector3 targetDirection_2 = Target.transform.position - JumpPos.position;

        yield return new WaitForSeconds(0.35f);
        GameObject DownAttackRange_2 = objPool.GetObjectFromPool();
        StartCoroutine(DownEffect_ActiveF(DownAttackRange_2));
        DownAttackRange_2.transform.position = JumpPos.transform.position + JumpPos.transform.forward * 30.0f;
        //DownAttackRange_2.transform.rotation = Quaternion.LookRotation(transform.forward);
        DownAttackRange_2.transform.parent = DownSword_SpawnTr;

        // PoolEffectObj.Add(DownAttackRange_2);
        //Destroy(DownAttackRange_2, 10.0f);
        //DownAttackRange_2.transform.forward = targetDirection_2;
        //Vector3 currentEulerAngles_2 = DownAttackRange_2.transform.rotation.eulerAngles;
        //DownAttackRange_2.transform.rotation = Quaternion.Euler(currentEulerAngles_2.x, currentEulerAngles_2.y, 0);
        DownAttackRange_2.transform.rotation = Quaternion.LookRotation(transform.forward);

        yield return new WaitForSeconds(0.1f);

        bossLookAt.isLook = true;

        yield return new WaitForSeconds(1.2f);

        bossLookAt.isLook = false;
        Vector3 targetDirection_3 = Target.transform.position - JumpPos.position;

        yield return new WaitForSeconds(0.45f);

        GameObject DownAttackRange_3 = objPool.GetObjectFromPool();
        StartCoroutine(DownEffect_ActiveF(DownAttackRange_3));
        DownAttackRange_3.transform.position = JumpPos.transform.position + JumpPos.transform.forward * 30.0f;
        //DownAttackRange_3.transform.rotation = Quaternion.LookRotation(transform.forward);
        DownAttackRange_3.transform.parent = DownSword_SpawnTr;

        // PoolEffectObj.Add(DownAttackRange_3);
        //Destroy(DownAttackRange_3, 10.0f);
        //DownAttackRange_3.transform.forward = targetDirection_3;
        //Vector3 currentEulerAngles_3 = DownAttackRange_3.transform.rotation.eulerAngles;
        //DownAttackRange_3.transform.rotation = Quaternion.Euler(currentEulerAngles_3.x, currentEulerAngles_3.y, 0);
        DownAttackRange_3.transform.rotation = Quaternion.LookRotation(transform.forward);

        yield return new WaitForSeconds(1f);

        bossLookAt.isLook = true;
        animator.SetTrigger("doDownAttack");

        yield return new WaitForSeconds(1.3f);

        bossLookAt.isLook = false;
        Vector3 targetDirection_4 = Target.transform.position - JumpPos.position;

        yield return new WaitForSeconds(0.3f);

        GameObject DownAttackRange_4 = objPool.GetObjectFromPool();
        StartCoroutine(DownEffect_ActiveF(DownAttackRange_4));
        DownAttackRange_4.transform.position = JumpPos.transform.position + JumpPos.transform.forward * 30.0f;
        //DownAttackRange_4.transform.rotation = Quaternion.LookRotation(transform.forward);
        DownAttackRange_4.transform.parent = DownSword_SpawnTr;

        //PoolEffectObj.Add(DownAttackRange_4);
        //Destroy(DownAttackRange_4, 10.0f);
        //DownAttackRange_4.transform.forward = targetDirection_4;
        //Vector3 currentEulerAngles_4 = DownAttackRange_4.transform.rotation.eulerAngles;
        //DownAttackRange_4.transform.rotation = Quaternion.Euler(currentEulerAngles_4.x, currentEulerAngles_4.y, 0);
        DownAttackRange_4.transform.rotation = Quaternion.LookRotation(transform.forward);

        yield return new WaitForSeconds(0.1f);

        bossLookAt.isLook = true;

        yield return new WaitForSeconds(1.2f);

        bossLookAt.isLook = false;
        Vector3 targetDirection_5 = Target.transform.position - JumpPos.position;

        yield return new WaitForSeconds(0.35f);


        GameObject DownAttackRange_5 = objPool.GetObjectFromPool();
        StartCoroutine(DownEffect_ActiveF(DownAttackRange_5));
        DownAttackRange_5.transform.position = JumpPos.transform.position + JumpPos.transform.forward * 30.0f;
        //DownAttackRange_5.transform.rotation = Quaternion.LookRotation(transform.forward);
        DownAttackRange_5.transform.parent = DownSword_SpawnTr;

        // PoolEffectObj.Add(DownAttackRange_5);
        // Destroy(DownAttackRange_5, 10.0f);
        //DownAttackRange_5.transform.forward = targetDirection_5;
        //Vector3 currentEulerAngles_5 = DownAttackRange_5.transform.rotation.eulerAngles;
        //DownAttackRange_5.transform.rotation = Quaternion.Euler(currentEulerAngles_5.x, currentEulerAngles_5.y, 0);
        DownAttackRange_5.transform.rotation = Quaternion.LookRotation(transform.forward);

        yield return new WaitForSeconds(0.1f);

        bossLookAt.isLook = true;

        yield return new WaitForSeconds(1.2f);

        bossLookAt.isLook = false;
        Vector3 targetDirection_6 = Target.transform.position - JumpPos.position;

        yield return new WaitForSeconds(0.45f);

        GameObject DownAttackRange_6 = objPool.GetObjectFromPool();
        StartCoroutine(DownEffect_ActiveF(DownAttackRange_6));
        DownAttackRange_6.transform.position = JumpPos.transform.position + JumpPos.transform.forward * 30.0f;
        //DownAttackRange_6.transform.rotation = Quaternion.LookRotation(transform.forward);
        DownAttackRange_6.transform.parent = DownSword_SpawnTr;

        // PoolEffectObj.Add(DownAttackRange_6);
        // Destroy(DownAttackRange_6, 10.0f);
        //DownAttackRange_6.transform.forward = targetDirection_6;
        //Vector3 currentEulerAngles_6 = DownAttackRange_6.transform.rotation.eulerAngles;
        //DownAttackRange_6.transform.rotation = Quaternion.Euler(currentEulerAngles_6.x, currentEulerAngles_6.y, 0);
        DownAttackRange_6.transform.rotation = Quaternion.LookRotation(transform.forward);

        yield return new WaitForSeconds(1f);

        bossLookAt.isLook = true;
        animator.SetTrigger("doDownAttack");

        yield return new WaitForSeconds(1.3f);

        bossLookAt.isLook = false;
        Vector3 targetDirection_7 = Target.transform.position - JumpPos.position;

        yield return new WaitForSeconds(0.3f);

        GameObject DownAttackRange_7 = objPool.GetObjectFromPool();
        StartCoroutine(DownEffect_ActiveF(DownAttackRange_7));
        DownAttackRange_7.transform.position = JumpPos.transform.position + JumpPos.transform.forward * 30.0f;
        //DownAttackRange_7.transform.rotation = Quaternion.LookRotation(transform.forward);
        DownAttackRange_7.transform.parent = DownSword_SpawnTr;

        //PoolEffectObj.Add(DownAttackRange_7);
        // Destroy(DownAttackRange_7, 10.0f);
        //DownAttackRange_7.transform.forward = targetDirection_7;
        //Vector3 currentEulerAngles_7 = DownAttackRange_7.transform.rotation.eulerAngles;
        //DownAttackRange_7.transform.rotation = Quaternion.Euler(currentEulerAngles_7.x, currentEulerAngles_7.y, 0);
        DownAttackRange_7.transform.rotation = Quaternion.LookRotation(transform.forward);

        yield return new WaitForSeconds(0.1f);

        bossLookAt.isLook = true;

        yield return new WaitForSeconds(1.2f);

        bossLookAt.isLook = false;
        Vector3 targetDirection_8 = Target.transform.position - JumpPos.position;

        yield return new WaitForSeconds(0.35f);

        GameObject DownAttackRange_8 = objPool.GetObjectFromPool();
        StartCoroutine(DownEffect_ActiveF(DownAttackRange_8));
        DownAttackRange_8.transform.position = JumpPos.transform.position + JumpPos.transform.forward * 30.0f;
        //DownAttackRange_8.transform.rotation = Quaternion.identity;
        DownAttackRange_8.transform.parent = DownSword_SpawnTr;

        // PoolEffectObj.Add(DownAttackRange_8);
        // Destroy(DownAttackRange_8, 10.0f);
        //DownAttackRange_8.transform.forward = targetDirection_8;
        //Vector3 currentEulerAngles_8 = DownAttackRange_8.transform.rotation.eulerAngles;
        //DownAttackRange_8.transform.rotation = Quaternion.Euler(currentEulerAngles_8.x, currentEulerAngles_8.y, 0);
        DownAttackRange_8.transform.rotation = Quaternion.LookRotation(transform.forward);

        yield return new WaitForSeconds(0.1f);

        bossLookAt.isLook = true;

        yield return new WaitForSeconds(1.2f);

        bossLookAt.isLook = false;
        Vector3 targetDirection_9 = Target.transform.position - JumpPos.position;

        yield return new WaitForSeconds(0.45f);

        GameObject DownAttackRange_9 = objPool.GetObjectFromPool();
        StartCoroutine(DownEffect_ActiveF(DownAttackRange_9));
        DownAttackRange_9.transform.position = JumpPos.transform.position + JumpPos.transform.forward * 30.0f;
        //DownAttackRange_9.transform.rotation = Quaternion.identity;
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

    // TODO ## IronGuard_Skill3
    #region IronGuard_Skill3
    IEnumerator BossSkill3()
    {  
        Vector3 jumpStartPosition = transform.position;
        Vector3 targetDirection = Target.transform.position - transform.position;

        Vector3 jumpEndAttackVec = Target.transform.position - targetDirection.normalized * 5.0f;
        StartCoroutine(JumpDuring(jumpStartPosition, jumpEndAttackVec, 1.4f));

        bossLookAt.isLook = false;
        boxCollider.enabled = false;

        animator.SetTrigger("doJumpAttack");

        yield return new WaitForSeconds(1.2f);

        JumpAttackRange.enabled = true;
        Vector3 bossForward = bossPos.position + bossPos.forward * 0.0f;
        GameObject newPrefab = Instantiate(JumpEffect, bossForward, Quaternion.identity);
        Destroy(newPrefab, 1.0f);

        yield return new WaitForSeconds(2f);
        
        boxCollider.enabled = true;
        animator.SetTrigger("doReturn");
        StartCoroutine(JumpDuring(jumpEndAttackVec, jumpStartPosition, 1.1f));

        yield return new WaitForSeconds(1f);

        JumpAttackRange.enabled = false;
        boxCollider.enabled = false;

        yield return new WaitForSeconds(1f);

        bossLookAt.isLook = true;
        boxCollider.enabled = true;

        yield return new WaitForSeconds(2f);
        StartCoroutine(Think());
    }
    #endregion

    // TODO ## IronGuard_Skill4
    #region IronGuard_Skill4
    IEnumerator BossSkill4()
    {
        Vector3 bossPosition = transform.position; // 보스 위치 값을 저장

        bossLookAt.isLook = false;

        razerMaker_1.SetActive(true);

        animator.SetTrigger("doRazer");

        yield return new WaitForSeconds(1f);

        shotRazer_1.UseRazer();
        shotRazer_2.UseRazer();
        shotRazer_3.UseRazer();
        shotRazer_4.UseRazer();

        yield return new WaitForSeconds(0.5f);

        animator.SetTrigger("doRazerReturn");

        yield return new WaitForSeconds(1.5f);

        razerMaker_1.SetActive(false);
        bossLookAt.isLook = true;

        yield return new WaitForSeconds(2f);

        StartCoroutine(Think());
    }
    #endregion

    // TODO ## IronGuard_Skill5
    #region IronGuard_Skill5
    IEnumerator BossSkill5()
    {
        Vector3 bossPosition = transform.position; // 보스 위치 값을 저장

        bossLookAt.isLook = false;

        razerMaker_2.SetActive(true);

        animator.SetTrigger("doRazer");

        yield return new WaitForSeconds(3f);

        shotRazer_5.UseRazer();
        shotRazer_6.UseRazer();
        shotRazer_7.UseRazer();
        shotRazer_8.UseRazer();

        yield return new WaitForSeconds(0.5f);

        animator.SetTrigger("doRazerReturn");

        yield return new WaitForSeconds(1f);

        razerMaker_2.SetActive(false);
        bossLookAt.isLook = true;

        yield return new WaitForSeconds(2f);
  
        StartCoroutine(Think());
    }
    #endregion


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
}
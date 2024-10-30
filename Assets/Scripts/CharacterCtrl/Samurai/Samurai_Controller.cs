using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Samurai_Controller : Character_BehaviorCtrl_Base
{
    [SerializeField]
    Samurai_ObjPool samurai_ObjPoolRef;
    public Transform firePoint; // 발사 지점
    public GameObject skill_Look;
    [SerializeField]
    LayerMask groundLayer; // 지면의 레이어
    public Camera mainCam;
    private Animator animator;
    private Vector3 destination;
    public float moveSpeed;
    public float moveSpeed_Discount;
    public float DodgeDistance;
    public float EskillDistance;
    public float rSkillDistance;
    public float attackMoveDistance;
    public float attackDuration = 0.5f;  // 이동할 시간

    public int comboStep = 0; // 현재 콤보의 단계
    public float comboTimeLimit = 2f; // 콤보 타이머 제한 시간
    public float comboTimer = 0f; // 콤보 타이머
    public bool isComboInProgress = false; // 콤보 진행 중 여부
    public bool isComboTimeout = false; // 콤보가 끝났는지의 여부


    private bool isMove;
    public bool isDodge;
    public bool isAttack;
    public bool isSkill1;
    public bool isSkill2;
    public bool isSkill3;
    public bool isSkill4;

    public bool isHit;
    public bool isHitOut;
    public bool isDie;

    public PlaySceneManager playscenemanager;
    public InGameSetting inGameSetting;

    public bool skill1;
    public bool skill2;
    public bool skill3;
    public bool skill4;
    public bool spaceDown;

    public Skill_Test skillManager1;
    public Skill_Test skillManager2;
    public Skill_Test skillManager3;
    public Skill_Test skillManager4;
    public Skill_Test skillManager5;

    RaycastHit eSkillRayHit;

    public GameObject BaseAtk_1_Eff;
    public Transform BaseAtk_1_Point;
    public GameObject BaseAtk_2_Eff;
    public Transform BaseAtk_2_Point;
    public GameObject BaseAtk_3_Eff;
    public Transform BaseAtk_3_Point;
    public GameObject BaseAtk_4_Eff;
    public Transform BaseAtk_4_Point;
    public GameObject BaseAtk_5_Eff;
    public Transform BaseAtk_5_Point;

    public GameObject Skill1Atk_1_Eff;
    public Transform Skill1Atk_1_Point;

    public GameObject Skill2Atk_1_Eff;
    public Transform Skill2Atk_1_Point;
    public GameObject Skill2Atk_2_Eff;
    public Transform Skill2Atk_2_Point;
    public GameObject Skill2Atk_3_Eff;
    public Transform Skill2Atk_3_Point;
    public GameObject Skill2Atk_4_Eff;
    public Transform Skill2Atk_4_Point;

    public GameObject Skill3Atk_1_Eff;
    public Transform Skill3Atk_1_Point;

    public GameObject Skill4Atk_1_Eff;
    public Transform Skill4Atk_1_Point;

    [SerializeField]
    float CamShake_Time;
    [SerializeField]
    float CamShake_Intensity;

    void Awake()
    {
        mainCam = Camera.main;
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        samurai_ObjPoolRef = GetComponent<Samurai_ObjPool>();
        Init();
    }

    void Init()
    {
        playscenemanager = GameManager.GMInstance.Get_PlaySceneManager();
        inGameSetting = GameManager.GMInstance.Get_InGameSetting();

        skillManager1 = GameManager.GMInstance.Get_PlaySceneManager().Skills_Info[0];
        skillManager2 = GameManager.GMInstance.Get_PlaySceneManager().Skills_Info[1];
        skillManager3 = GameManager.GMInstance.Get_PlaySceneManager().Skills_Info[2];
        skillManager4 = GameManager.GMInstance.Get_PlaySceneManager().Skills_Info[3];
        skillManager5 = GameManager.GMInstance.Get_PlaySceneManager().Skills_Info[4];
    }

    // Update is called once per frame
    public void Update()
    {
        firePoint.transform.localRotation = Quaternion.identity;

        if ((Input.GetKey(KeySetting.Keys[KeyAction.Skill1]) && !skillManager1.isSkill1CT) || (Input.GetKey(KeySetting.Keys[KeyAction.Skill2]) && !skillManager2.isSkill2CT)
            || (Input.GetKey(KeySetting.Keys[KeyAction.Skill3]) && !skillManager3.isSkill3CT) || (Input.GetKey(KeySetting.Keys[KeyAction.Skill4]) && !skillManager4.isSkill4CT))//|| skill1 || skill2 || skill3 || skill4|| 
        {
            if (!isAttack && !isDodge && !isSkill1 && !isSkill2 && !isSkill3 && !isSkill4 && !isDie && GameManager.GMInstance.Get_PlaySceneManager().isCutScene == false)
            {
                Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
                RaycastHit rayHit;
                if (Physics.Raycast(ray, out rayHit, 100)) // ray가 100의 길이까지 쏘았을때 충돌이 있으면 rayHit에 저장
                {
                    Vector3 nextVec = rayHit.point - transform.position; // rayHit.point : ray가 충돌한 지점, transform.position : 캐릭터의 현재 위치
                    nextVec.y = 0;                                       // y값을 0으로 하여 수평선에서 바라볼 수 있도록 함
                    transform.LookAt(transform.position + nextVec); // 캐릭터가 계산된 방향 벡터를 바라보도록 회전합니다.
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 70f, groundLayer) && hit.collider.CompareTag("Ground") && !inGameSetting.isPaused)
            {
                Vector3 spawnPosition = new Vector3(hit.point.x, hit.point.y + 2, hit.point.z);
                // 프리팹을 생성하고 1초 후에 파괴
                GameObject mouseEffect = samurai_ObjPoolRef.MouseMoveEffectFromPool();
                mouseEffect.transform.position = spawnPosition; // 발사 위치 설정
                mouseEffect.SetActive(true); // 발사체 활성화

                StartCoroutine(MouseEffectFalse(mouseEffect, 1f));
            }
        }

        if (Input.GetMouseButton(1) && GameManager.GMInstance.Get_PlaySceneManager().isCutScene == false && !inGameSetting.isPaused)
        {
            RaycastHit hit;
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer) && hit.collider.CompareTag("Ground"))
            {
                SetDestination(hit.point);
            }
        }
        Move();
        Attack();
        GetInput();
        if (skill1)
        {
            Skill_1();
        }

        if (skill2)
        {
            Skill_2();
        }

        if (skill3)
        {
            Skill_3();
        }

        if (skill4)
        {
            Skill_4();
        }

        if (spaceDown)
        {
            Dodge();
        }

        UpdateComboStep();
    }
    private IEnumerator MouseEffectFalse(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
    }

    private void SetDestination(Vector3 dest)
    {
        if (!isAttack)
        {
            destination = dest;
            isMove = true;
            animator.SetBool("isMove", true);
        }
    }

    public override void Move()
    {
        if (isHitOut == false)
            return;

        if (!isSkill1 && !isSkill2 && !isSkill3 && !isSkill4 && !isAttack && !isDodge && !isDie)
        {
            if (isMove)
            {
                if (GameManager.GMInstance.cur_Scene == Define.Cur_Scene.FLOOR_4)
                {
                    Vector3 dirr = destination - transform.position;
                    transform.forward = dirr;
                    transform.position += dirr.normalized * Time.deltaTime * (moveSpeed - moveSpeed_Discount);
                    transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                }
                else
                {
                    Vector3 dir = destination - transform.position;
                    transform.forward = dir;
                    transform.position += dir.normalized * Time.deltaTime * (moveSpeed - moveSpeed_Discount);
                }
            }

            if (Vector3.Distance(transform.position, destination) <= 0.3f)
            {
                animator.SetBool("isMove", false);
                isMove = false;
            }
        }
    }

    public override void GetInput()
    {
        // 연출 중이면 실행 안함
        if (GameManager.GMInstance.Get_PlaySceneManager().isCutScene == true)
            return;

        spaceDown = Input.GetKey(KeySetting.Keys[KeyAction.Dodge]);
        skill1 = Input.GetKey(KeySetting.Keys[KeyAction.Skill1]);
        skill2 = Input.GetKey(KeySetting.Keys[KeyAction.Skill2]);
        skill3 = Input.GetKey(KeySetting.Keys[KeyAction.Skill3]);
        skill4 = Input.GetKey(KeySetting.Keys[KeyAction.Skill4]);
    }

    public override void Attack()
    {
        // 연출 중이면 실행 안함
        if (GameManager.GMInstance.Get_PlaySceneManager().isCutScene == true)
            return;

        if (Input.GetMouseButtonDown(0) && !isSkill1 && !isSkill2 && !isSkill3 && !isSkill4 && !isDie && !inGameSetting.isPaused)
        {
            CancelInvoke("AttackOut");
            isMove = false;
            isAttack = true;
            isHitOut = true;
            animator.SetBool("isMove", false);
            animator.SetTrigger("Attack");


            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;

            if (Physics.Raycast(ray, out rayHit, 100))
            {
                Vector3 dodgeDirection = rayHit.point - transform.position;
                dodgeDirection.y = 0;
                dodgeDirection.Normalize(); // 벡터를 정규화합니다.
                transform.LookAt(transform.position + dodgeDirection);
            }

            Invoke("AttackOut", 0.4f);
        }
    }

    public void AttackEvent()
    {
        StartCoroutine(PerformAttackMove());
    }

    private IEnumerator PerformAttackMove()
    {
        float elapsedTime = 0f;
        float attackSpeed = attackMoveDistance / attackDuration;

        while (elapsedTime < attackDuration)
        {
            float moveDistance = attackSpeed * Time.deltaTime;
            transform.Translate(transform.forward * moveDistance, Space.World);

            elapsedTime += Time.deltaTime;
            yield return null;  // 다음 프레임까지 대기
        }
    }

    public void AttackOut()
    {
        isAttack = false;
    }

    public void BaseAtk1_Eff()
    {
        // 보이스 랜덤 재생
        GameManager.GMInstance.SoundManagerRef.Play_Assasin_SFX((SoundManager.Assasin_SFX)Random.Range(6, 8));
        // 슬래시 효과음 재생
        GameManager.GMInstance.SoundManagerRef.Play_Assasin_SFX((SoundManager.Assasin_SFX)Random.Range(0, 3));
        StartCoroutine(Play_BaseAtk1_Eff());
    }

    IEnumerator Play_BaseAtk1_Eff()
    {
        BaseAtk_1_Eff.transform.position = BaseAtk_1_Point.transform.position;

        Vector3 d2 = BaseAtk_1_Eff.transform.position - skill_Look.transform.position;
        d2.y = 0.0f;
        Quaternion q2 = Quaternion.LookRotation(d2);
        //BaseAtk_1_Eff.transform.rotation = q2 * Quaternion.Euler(0f, 0f, 0f);
        BaseAtk_1_Eff.transform.rotation = q2 * Quaternion.Euler(180f, 0f, 0f);

        BaseAtk_1_Eff.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        BaseAtk_1_Eff.SetActive(false);
    }

    public void BaseAtk2_Eff()
    {
        // 보이스 랜덤 재생
        GameManager.GMInstance.SoundManagerRef.Play_Assasin_SFX((SoundManager.Assasin_SFX)Random.Range(6, 8));
        // 슬래시 효과음 재생
        GameManager.GMInstance.SoundManagerRef.Play_Assasin_SFX((SoundManager.Assasin_SFX)Random.Range(0, 3));
        StartCoroutine(Play_BaseAtk2_Eff());
    }

    IEnumerator Play_BaseAtk2_Eff()
    {
        BaseAtk_2_Eff.transform.position = BaseAtk_2_Point.transform.position;

        Vector3 d2 = BaseAtk_2_Eff.transform.position - skill_Look.transform.position;
        d2.y = 0.0f;
        Quaternion q2 = Quaternion.LookRotation(d2);
        //BaseAtk_2_Eff.transform.rotation = q2 * Quaternion.Euler(75f, 0f, 90f);
        BaseAtk_2_Eff.transform.rotation = q2 * Quaternion.Euler(0f, 180f, 0f);

        BaseAtk_2_Eff.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        BaseAtk_2_Eff.SetActive(false);
    }

    public void BaseAtk3_Eff()
    {
        // 보이스 랜덤 재생
        GameManager.GMInstance.SoundManagerRef.Play_Assasin_SFX((SoundManager.Assasin_SFX)Random.Range(6, 8));
        // 슬래시 효과음 재생
        GameManager.GMInstance.SoundManagerRef.Play_Assasin_SFX((SoundManager.Assasin_SFX)Random.Range(0, 3));
        StartCoroutine(Play_BaseAtk3_Eff());
    }

    IEnumerator Play_BaseAtk3_Eff()
    {
        BaseAtk_3_Eff.transform.position = BaseAtk_3_Point.transform.position;

        Vector3 d2 = BaseAtk_3_Eff.transform.position - skill_Look.transform.position;
        d2.y = 0.0f;
        Quaternion q2 = Quaternion.LookRotation(d2);
        //BaseAtk_3_Eff.transform.rotation = q2 * Quaternion.Euler(0f, 0f, 0f);
        BaseAtk_3_Eff.transform.rotation = q2 * Quaternion.Euler(0f, 180f, 0f);

        BaseAtk_3_Eff.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        BaseAtk_3_Eff.SetActive(false);
    }

    public void BaseAtk4_Eff()
    {
        // 보이스 랜덤 재생
        GameManager.GMInstance.SoundManagerRef.Play_Assasin_SFX((SoundManager.Assasin_SFX)Random.Range(6, 8));
        // 슬래시 효과음 재생
        GameManager.GMInstance.SoundManagerRef.Play_Assasin_SFX((SoundManager.Assasin_SFX)Random.Range(0, 3));
        StartCoroutine(Play_BaseAtk4_Eff());
    }

    IEnumerator Play_BaseAtk4_Eff()
    {
        BaseAtk_4_Eff.transform.position = BaseAtk_4_Point.transform.position;

        Vector3 d2 = BaseAtk_4_Eff.transform.position - skill_Look.transform.position;
        d2.y = 0.0f;
        Quaternion q2 = Quaternion.LookRotation(d2);
        //BaseAtk_4_Eff.transform.rotation = q2 * Quaternion.Euler(180f, 180f, 0f);
        BaseAtk_4_Eff.transform.rotation = q2 * Quaternion.Euler(180f, 0f, 0f);

        BaseAtk_4_Eff.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        BaseAtk_4_Eff.SetActive(false);
    }

    public void BaseAtk5_Eff()
    {
        // 보이스 랜덤 재생
        GameManager.GMInstance.SoundManagerRef.Play_Assasin_SFX(SoundManager.Assasin_SFX.ASSASIN_VOICE_2);
        // 슬래시 효과음 재생
        GameManager.GMInstance.SoundManagerRef.Play_Assasin_SFX((SoundManager.Assasin_SFX)Random.Range(0, 3));
        StartCoroutine(Play_BaseAtk5_Eff());
    }

    IEnumerator Play_BaseAtk5_Eff()
    {
        BaseAtk_5_Eff.transform.position = BaseAtk_5_Point.transform.position;

        Vector3 d2 = BaseAtk_5_Eff.transform.position - skill_Look.transform.position;
        d2.y = 0.0f;
        Quaternion q2 = Quaternion.LookRotation(d2);
        BaseAtk_5_Eff.transform.rotation = q2 * Quaternion.Euler(0f, 180f, 0f);

        BaseAtk_5_Eff.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        BaseAtk_5_Eff.SetActive(false);
    }

    public override void Dodge()
    {
        if (!isDodge && !skillManager5.isDodgeCT && !isDie && !inGameSetting.isPaused)
        {
            animator.SetBool("isMove", false);
            animator.SetTrigger("doDodge");
            isDodge = true;
            isHit = true;
            isHitOut = true;
            isMove = false;

            SkillOut();

            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;

            if (Physics.Raycast(ray, out rayHit, 100))
            {
                Vector3 dodgeDirection = rayHit.point - transform.position;
                dodgeDirection.y = 0;
                dodgeDirection.Normalize(); // 벡터를 정규화합니다.
                transform.LookAt(transform.position + dodgeDirection);

                Vector3 dodgeStartPosition = transform.position;
                Vector3 dodgeEndPosition = transform.position + dodgeDirection * DodgeDistance;

                StartCoroutine(MoveDuring(dodgeStartPosition, dodgeEndPosition, 0.7f));
                Invoke("DodgeOut", 0.7f);
            }
        }
    }

    public override void DodgeOut()
    {
        isDodge = false;
        isHit = false;
    }

    IEnumerator MoveDuring(Vector3 startPosition, Vector3 endPosition, float duration)
    {
        float startTime = Time.time;

        while (Time.time - startTime < duration)
        {
            // Dodge 동작 중에 플레이어를 움직입니다.
            float t = (Time.time - startTime) / duration; // 시간 계산
            transform.position = Vector3.Lerp(startPosition, endPosition, t); // 시작 지점에서 도착 지점까지 t만큼 시간 소요
            yield return null;
        }

        transform.position = endPosition;
    }

    public override void Skill_1()
    {
        if (!isSkill1 && !isAttack && !isDodge && !skillManager1.isSkill1CT && !isDie && !inGameSetting.isPaused)
        {
            animator.SetBool("isMove", false);

            isMove = false;
            isHitOut = true;
            isSkill1 = true;

            animator.SetTrigger("doSkill1");

            // 보이스 랜덤 재생
            GameManager.GMInstance.SoundManagerRef.Play_Assasin_SFX((SoundManager.Assasin_SFX)Random.Range(5, 8));

            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;

            if (Physics.Raycast(ray, out rayHit, 100))
            {
                Vector3 dodgeDirection = rayHit.point - transform.position;
                dodgeDirection.y = 0;
                dodgeDirection.Normalize(); // 벡터를 정규화합니다.
                transform.LookAt(transform.position + dodgeDirection);
            }
        }
    }

    public void Skill1Atk_Eff()
    {
        // 보이스 랜덤 재생
        GameManager.GMInstance.SoundManagerRef.Play_Assasin_SFX(SoundManager.Assasin_SFX.ASSASIN_VOICE_2);
        // 슬래시 효과음 재생
        GameManager.GMInstance.SoundManagerRef.Play_Assasin_SFX((SoundManager.Assasin_SFX)Random.Range(0, 3));
        StartCoroutine(Play_Skill1Atk_Eff());
    }

    IEnumerator Play_Skill1Atk_Eff()
    {
        Skill1Atk_1_Eff.transform.position = Skill1Atk_1_Point.transform.position;

        Vector3 d2 = Skill1Atk_1_Eff.transform.position - skill_Look.transform.position;
        d2.y = 0.0f;
        Quaternion q2 = Quaternion.LookRotation(d2);
        Skill1Atk_1_Eff.transform.rotation = q2 * Quaternion.Euler(0f, 180f, 90f);

        Skill1Atk_1_Eff.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        Skill1Atk_1_Eff.SetActive(false);
    }

    public override void Skill_2()
    {
        if (!isComboTimeout && !isSkill2 && !isAttack && !isDodge && !skillManager2.isSkill2CT && !isDie && !inGameSetting.isPaused)
        {
            if (comboStep == 0)
            {
                // 첫 번째 콤보 단계 시작
                StartComboStep();
                animator.SetTrigger("doSkill2");
                
            }
            else if (comboStep == 1)
            {
                // 두 번째 콤보 단계
                comboStep++;
                comboTimer = comboTimeLimit;
                animator.SetTrigger("Combo1");
            }
            else if (comboStep == 2)
            {
                // 세 번째 콤보 단계
                comboStep++;
                comboTimer = comboTimeLimit;
                animator.SetTrigger("Combo2");
            }
            else if (comboStep == 3)
            {
                comboStep++;
                animator.SetTrigger("Combo3");
                // 마지막 콤보 단계
                ResetCombo(); // 콤보 초기화
            }
            animator.SetBool("isMove", false);

            isMove = false;
            isHitOut = true;
            isSkill2 = true;

            // 보이스 랜덤 재생
            GameManager.GMInstance.SoundManagerRef.Play_Assasin_SFX((SoundManager.Assasin_SFX)Random.Range(5, 8));

           

            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;

            if (Physics.Raycast(ray, out rayHit, 100))
            {
                Vector3 dodgeDirection = rayHit.point - transform.position;
                dodgeDirection.y = 0;
                dodgeDirection.Normalize(); // 벡터를 정규화합니다.
                transform.LookAt(transform.position + dodgeDirection);
            }
        }
    }

    public void StartComboStep()
    {
        if (!isComboInProgress)
        {
            comboStep++;
            comboTimer = comboTimeLimit;
            isComboInProgress = true; // 콤보 진행 중으로 설정
        }
    }

    public void UpdateComboStep()
    {
        if (isComboInProgress)
        {
            comboTimer -= Time.deltaTime;

            if (comboTimer <= 0f)
            {
                // 콤보 타이머 종료
                comboStep = 0;
                isComboInProgress = false; // 콤보 진행 중이 아님
                isComboTimeout = true;
            }
        }
    }

    public void ResetCombo()
    {
        StartCoroutine(Play_ResetCombo());
    }

    IEnumerator Play_ResetCombo()
    {
        isComboTimeout = true;
        isComboInProgress = false;
        yield return new WaitForSeconds(1f);
        comboStep = 0;
        comboTimer = 0f;
    }

    public void Skill2_Envet()
    {
        comboStep++;
    }

    public void Skill2Atk1_Eff()
    {
        // 사운드
        GameManager.GMInstance.SoundManagerRef.Play_Assasin_SFX(SoundManager.Assasin_SFX.SWING_1);
        StartCoroutine(Play_Skill2Atk_1_Eff());
    }


    IEnumerator Play_Skill2Atk_1_Eff()
    {
        Skill2Atk_1_Eff.transform.position = Skill2Atk_1_Point.transform.position;

        Vector3 d2 = Skill2Atk_1_Eff.transform.position - skill_Look.transform.position;
        d2.y = 0.0f;
        Quaternion q2 = Quaternion.LookRotation(d2);
        Skill2Atk_1_Eff.transform.rotation = q2 * Quaternion.Euler(0f, 180f, 225f);

        Skill2Atk_1_Eff.SetActive(true);

        yield return new WaitForSeconds(0.2f);

        Skill2Atk_1_Eff.SetActive(false);
    }

    public void Skill2Atk2_Eff()
    {
        GameManager.GMInstance.SoundManagerRef.Play_Assasin_SFX(SoundManager.Assasin_SFX.SWING_2);
        StartCoroutine(Play_Skill2Atk_2_Eff());
    }


    IEnumerator Play_Skill2Atk_2_Eff()
    {
        Skill2Atk_2_Eff.transform.position = Skill2Atk_2_Point.transform.position;

        Vector3 d2 = Skill2Atk_2_Eff.transform.position - skill_Look.transform.position;
        d2.y = 0.0f;
        Quaternion q2 = Quaternion.LookRotation(d2);
        Skill2Atk_2_Eff.transform.rotation = q2 * Quaternion.Euler(0f, 180f, 0f);

        Skill2Atk_2_Eff.SetActive(true);

        yield return new WaitForSeconds(0.3f);

        Skill2Atk_2_Eff.SetActive(false);
    }

    public void Skill2Atk3_Eff()
    {
        // 사운드
        GameManager.GMInstance.SoundManagerRef.Play_Assasin_SFX(SoundManager.Assasin_SFX.SWING_3);
        StartCoroutine(Play_Skill2Atk_3_Eff());
    }


    IEnumerator Play_Skill2Atk_3_Eff()
    {
        Skill2Atk_3_Eff.transform.position = Skill2Atk_3_Point.transform.position;

        Vector3 d2 = Skill2Atk_3_Eff.transform.position - skill_Look.transform.position;
        d2.y = 0.0f;
        Quaternion q2 = Quaternion.LookRotation(d2);
        Skill2Atk_3_Eff.transform.rotation = q2 * Quaternion.Euler(0f, 180f, 90f);

        Skill2Atk_3_Eff.SetActive(true);

        yield return new WaitForSeconds(0.3f);

        Skill2Atk_3_Eff.SetActive(false);
    }

    public void Skill2Atk4_Eff()
    {
        // 사운드
        GameManager.GMInstance.SoundManagerRef.Play_Assasin_SFX(SoundManager.Assasin_SFX.SWING_3);
        StartCoroutine(Play_Skill2Atk_4_Eff());
    }


    IEnumerator Play_Skill2Atk_4_Eff()
    {
        Skill2Atk_4_Eff.transform.position = Skill2Atk_4_Point.transform.position;

        Vector3 d2 = Skill2Atk_4_Eff.transform.position - skill_Look.transform.position;
        d2.y = 0.0f;
        Quaternion q2 = Quaternion.LookRotation(d2);
        Skill2Atk_4_Eff.transform.rotation = q2 * Quaternion.Euler(0f, 180f, 90f);

        Skill2Atk_4_Eff.SetActive(true);

        yield return new WaitForSeconds(0.3f);

        Skill2Atk_4_Eff.SetActive(false);
    }

    public override void Skill_3()
    {
        if (!isSkill3 && !isAttack && !isDodge && !skillManager3.isSkill3CT && !isDie && !inGameSetting.isPaused)
        {
            animator.SetBool("isMove", false);
            // 보이스 랜덤 재생
            GameManager.GMInstance.SoundManagerRef.Play_Assasin_SFX((SoundManager.Assasin_SFX)Random.Range(5, 8));
            isMove = false;

            isHitOut = true;
            isSkill3 = true;
            animator.SetTrigger("doSkill3");

            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;

            if (Physics.Raycast(ray, out rayHit, 100))
            {
                this.eSkillRayHit = rayHit;
                Invoke("Skill03_Event01", 0.3f);
            }
        }
    }

    public void Skill03_Event01()
    {
        Vector3 dodgeDirection = eSkillRayHit.point - transform.position;
        dodgeDirection.y = 0;
        dodgeDirection.Normalize(); // 벡터를 정규화합니다.
        transform.LookAt(transform.position + dodgeDirection);

        Vector3 dodgeStartPosition = transform.position;
        dodgeStartPosition.y = 0;
        //Vector3 dodgeEndPosition = transform.position + dodgeDirection * EskillDistance;
        Vector3 dodgeEndPosition;
        if (Vector3.Distance(transform.position, eSkillRayHit.point) < EskillDistance)
        {
            dodgeEndPosition = eSkillRayHit.point;
        }
        else
        {
            dodgeEndPosition = transform.position + dodgeDirection * EskillDistance;
        }

        StartCoroutine(MoveDuring(dodgeStartPosition, dodgeEndPosition, 0.2f));
    }

    public void Skill3Atk1_Eff()
    {
        StartCoroutine(Play_Skill3Atk_1_Eff());
    }


    IEnumerator Play_Skill3Atk_1_Eff()
    {
        Skill3Atk_1_Eff.transform.position = Skill3Atk_1_Point.transform.position;

        Vector3 d2 = Skill3Atk_1_Eff.transform.position - skill_Look.transform.position;
        d2.y = 0.0f;
        Quaternion q2 = Quaternion.LookRotation(d2);
        Skill3Atk_1_Eff.transform.rotation = q2 * Quaternion.Euler(0f, 180f, 0f);

        Skill3Atk_1_Eff.SetActive(true);
        yield return new WaitForSeconds(1.3f);
        Skill3Atk_1_Eff.SetActive(false);
    }

    public override void Skill_4()
    {
        if (!isSkill4 && !isAttack && !isDodge && !skillManager4.isSkill4CT && !isDie && !inGameSetting.isPaused)
        {
            GameManager.GMInstance.SoundManagerRef.Play_Assasin_SFX(SoundManager.Assasin_SFX.ASSASIN_VOICE_1);
            animator.SetBool("isMove", false);

            isHitOut = true;
            isMove = false;
            isSkill4 = true;

            // 카메라 Y값 잠금
            Camera.main.GetComponent<Follow>().LockCameraYPosition();

            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;

            if (Physics.Raycast(ray, out rayHit, 100))
            {
                Vector3 dodgeDirection = rayHit.point - transform.position;
                dodgeDirection.y = 0;
                dodgeDirection.Normalize(); // 벡터를 정규화합니다.
                transform.LookAt(transform.position + dodgeDirection);

                Vector3 dodgeStartPosition = transform.position;
                Vector3 dodgeEndPosition;
                if (Vector3.Distance(transform.position, rayHit.point) < 25f)
                {
                    dodgeEndPosition = rayHit.point;
                }
                else
                {
                    dodgeEndPosition = transform.position + dodgeDirection * 25f;
                }

                StartCoroutine(DodgeAndHover(dodgeStartPosition, dodgeEndPosition));
                animator.SetTrigger("doSkill4");
            }

            // 스킬이 끝난 후 카메라 Y값 해제
            StartCoroutine(UnlockCameraAfterDelay(2.0f)); // 예시로 2초 후에 Y값 해제
        }
    }

    private IEnumerator UnlockCameraAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Camera.main.GetComponent<Follow>().UnlockCameraYPosition();
    }

    //private IEnumerator DodgeAndHover(Vector3 start, Vector3 end)
    //{
    //    // 0.1초 동안 상승
    //    Vector3 upPosition = start + Vector3.up * rSkillDistance;
    //    float elapsedTime = 0f;
    //    while (elapsedTime < 0.2f)
    //    {
    //        transform.position = Vector3.Lerp(start, upPosition, elapsedTime / 0.1f);
    //        elapsedTime += Time.deltaTime;
    //        yield return null;
    //    }
    //    transform.position = upPosition; // 정확한 위치 설정

    //    // 0.2초 동안 체공
    //    yield return new WaitForSeconds(0.3f);

    //    // 0.2초 동안 내려가면서 이동
    //    elapsedTime = 0f;
    //    while (elapsedTime < 0.2f)
    //    {
    //        transform.position = Vector3.Lerp(upPosition, end, elapsedTime / 0.2f);
    //        elapsedTime += Time.deltaTime;
    //        yield return null;
    //    }
    //    transform.position = end; // 정확한 위치 설정
    //}

    private IEnumerator DodgeAndHover(Vector3 start, Vector3 end)
    {
        float duration = 0.5f; // 전체 시간 (상승 + 하강)
        float elapsedTime = 0f;

        Vector3 direction = (end - start); // 시작과 끝 사이의 벡터
        float horizontalDistance = direction.magnitude; // 수평 거리 (X, Z)
        direction.y = 0; // Y값을 0으로 만들어 수평 방향만 계산
        Vector3 moveDirection = direction.normalized; // 이동 방향 (수평)

        // 최고점은 중간에 오도록 계산
        float maxHeight = rSkillDistance; // 상승할 최대 높이

        // 처음부터 끝까지 시간을 설정하여 위치 계산
        while (elapsedTime < duration)
        {
            float normalizedTime = elapsedTime / duration; // 0 to 1 사이로 변환

            // Y값 (포물선 형태로 상승 후 하강)
            float height = Mathf.Sin(Mathf.PI * normalizedTime) * maxHeight;

            // X, Z값 (수평 이동)
            Vector3 horizontalMove = moveDirection * normalizedTime * horizontalDistance;

            // 최종 위치 계산
            Vector3 newPosition = start + horizontalMove + Vector3.up * height;

            transform.position = newPosition;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 마지막 위치 설정 (end 지점에 정확히 맞추기)
        transform.position = end + Vector3.up * Mathf.Sin(Mathf.PI) * maxHeight;
    }

    public void Skill4Atk1_Eff()
    {
        StartCoroutine(Play_Skill4Atk_1_Eff());
    }


    IEnumerator Play_Skill4Atk_1_Eff()
    {
        Skill4Atk_1_Point.transform.position = transform.position;
        Skill4Atk_1_Eff.SetActive(true);
        yield return new WaitForSeconds(2f);
        Skill4Atk_1_Eff.SetActive(false);
    }

    public void SkillOut()
    {
        isSkill1 = false;
        isSkill2 = false;
        isSkill3 = false;
        isSkill4 = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyAttack" && !isHit)
        {

            Debug.Log(other.gameObject.name);
            // Debug.Break();
            if (playscenemanager.health > 1)
            {
                GameManager.GMInstance.CamShakeRef.ShakeCam(CamShake_Intensity, CamShake_Time);
                playscenemanager.HealthDown();
                // Debug.Log(other.gameObject.name);
                int randomIndex = Random.Range(0, 4);
                string triggerName = "Hit_" + randomIndex.ToString();
                animator.SetTrigger(triggerName);
                isHit = true;

                Invoke("HitOut", 3f);
            }
            else
            {
                GameManager.GMInstance.CamShakeRef.ShakeCam(CamShake_Intensity, CamShake_Time);
                // Debug.Log("1");
                playscenemanager.HealthDown();
                animator.SetBool("isMove", false);
                animator.SetTrigger("doDie");
                SkillOut();
                isMove = false;
                isAttack = false;
                isDodge = false;
                isDie = true;
                transform.GetComponent<CapsuleCollider>().enabled = false;

                Invoke("CharacterDie", 0.5f);
            }

        }
    }

    // 콜라이더에 계속 들어가 있을 시
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "EnemyAttack" && !isHit)
        {
            // Debug.Log(other.gameObject.name + "@");

            if (playscenemanager.health > 1)
            {
                GameManager.GMInstance.CamShakeRef.ShakeCam(CamShake_Intensity, CamShake_Time);
                playscenemanager.HealthDown();
                // Debug.Log(other.gameObject.name);
                int randomIndex = Random.Range(0, 4);
                string triggerName = "Hit_" + randomIndex.ToString();
                animator.SetTrigger(triggerName);
                isHit = true;

                Invoke("HitOut", 3f);
            }
            else
            {
                GameManager.GMInstance.CamShakeRef.ShakeCam(CamShake_Intensity, CamShake_Time);
                // Debug.Log("1");
                playscenemanager.HealthDown();
                animator.SetBool("isMove", false);
                animator.SetTrigger("doDie");
                SkillOut();
                isMove = false;
                isAttack = false;
                isDodge = false;
                isDie = true;
                transform.GetComponent<CapsuleCollider>().enabled = false;

                Invoke("CharacterDie", 0.5f);
            }
        }
    }


    void OnParticleCollision(GameObject other)
    {
        // 파티클에 닿을 시 데미지 
        if (other.gameObject.CompareTag("Particle_EnemyAtk") && !isHit)
        {

            if (playscenemanager.health > 1)
            {
                GameManager.GMInstance.CamShakeRef.ShakeCam(CamShake_Intensity, CamShake_Time);
                int randomIndex = Random.Range(0, 4);
                string triggerName = "Hit_" + randomIndex.ToString();
                animator.SetTrigger(triggerName);
                playscenemanager.HealthDown();
                // Debug.Log(other.gameObject.name);

                isHit = true;

                Invoke("HitOut", 3.0f);
            }
        }
    }

    void CharacterDie()
    {
        playscenemanager.CharacterDie();
    }


    // 3초간 무적
    public void HitOut()
    {
        isHit = false;
    }

    public void CantMove()
    {
        isMove = false;
        isDodge = false;
        isAttack = false;
        isSkill1 = false;
        isSkill2 = false;
        isSkill3 = false;
        isSkill4 = false;
        isHitOut = false;
        // moveSpeed_Discount = moveSpeed;
    }

    public void CanMove()
    {
        // Debug.Log(0);
        isMove = true;
        isHitOut = true;
        // moveSpeed_Discount = 0;
    }

    //public void Move_Error()
    //{
    //    if (moveSpeed_Discount != 0)
    //    {
    //        moveSpeed_Discount = 0;
    //    }
    //}
}


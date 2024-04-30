using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assassin_Controller : Character_BehaviorCtrl_Base
{
    [SerializeField]
    Assassin_ObjPool assassin_ObjPoolRef;
    public Transform firePoint; // 발사 지점
    public GameObject skill_Look;
    [SerializeField]
    LayerMask groundLayer; // 지면의 레이어
    public GameObject mouseMoveEffect; // 이동시 마우스 클릭 이펙트
    public Camera mainCam;
    private Animator animator;
    private Vector3 destination;
    public float moveSpeed;
    public float DodgeDistance;
    public float EskillDistance;

    private bool isMove;
    public bool isDodge;
    public bool isAttack;
    public bool isSkill1;
    public bool isSkill2;
    public bool isSkill3;
    public bool isSkill4;

    public bool isHit;
    public bool isDie;

    public PlaySceneManager playscenemanager;

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

    public GameObject Skill2Atk_1_Eff;
    public Transform Skill2Atk_1_Point;
    public GameObject Skill2Atk_2_Eff;
    public Transform Skill2Atk_2_Point;
    public GameObject Skill2Atk_3_Eff;
    public Transform Skill2Atk_3_Point;

    public GameObject Skill3Atk_1_Eff;
    public GameObject Skill3Atk_2_Eff;
    public Transform Skill3Atk_3_Point;
    public GameObject Skill3Atk_3_Eff;

    public GameObject Skill4Atk_1_Eff;
    public Transform Skill4Atk_1_Point;
    public GameObject Skill4Atk_2_Eff;
    public Transform Skill4Atk_2_Point;
    public GameObject Skill4Atk_3_Eff;
    public Transform Skill4Atk_3_Point;

    void Awake()
    {
        mainCam = Camera.main;
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        assassin_ObjPoolRef = GetComponent<Assassin_ObjPool>();
        
    }

    // Update is called once per frame
    public void Update()
    {
        firePoint.transform.localRotation = Quaternion.identity;

        if (Input.GetMouseButton(0) || (Input.GetKey(KeySetting.Keys[KeyAction.Skill1]) && !skillManager1.isSkill1CT) || (Input.GetKey(KeySetting.Keys[KeyAction.Skill2]) && !skillManager2.isSkill2CT)
            || (Input.GetKey(KeySetting.Keys[KeyAction.Skill3]) && !skillManager3.isSkill3CT) || (Input.GetKey(KeySetting.Keys[KeyAction.Skill4]) && !skillManager4.isSkill4CT))//|| skill1 || skill2 || skill3 || skill4|| 
        {
            if (!isAttack && !isDodge && !isSkill1 && !isSkill2 && !isSkill3 && !isSkill4 && !isDie)
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

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer) && hit.collider.CompareTag("Ground"))
            {
                Vector3 spawnPosition = new Vector3(hit.point.x, hit.point.y + 2f, hit.point.z);
                // 프리팹을 생성하고 1초 후에 파괴
                GameObject newPrefab = Instantiate(mouseMoveEffect, spawnPosition, Quaternion.identity);

                // 회전 값을 변경합니다. x 값을 90으로 설정합니다.
                newPrefab.transform.rotation = Quaternion.Euler(90f, 0f, 0f);

                Destroy(newPrefab, 1.0f);
            }
        }

        if (Input.GetMouseButton(1))
        {
            RaycastHit hit;
            if (Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out hit) && hit.collider.CompareTag("Ground"))
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
    }

    public override void GetInput()
    {
        spaceDown = Input.GetKey(KeySetting.Keys[KeyAction.Dodge]);
        skill1 = Input.GetKey(KeySetting.Keys[KeyAction.Skill1]);
        skill2 = Input.GetKey(KeySetting.Keys[KeyAction.Skill2]);
        skill3 = Input.GetKey(KeySetting.Keys[KeyAction.Skill3]);
        skill4 = Input.GetKey(KeySetting.Keys[KeyAction.Skill4]);
    }

    public override void Attack()
    {

        if (Input.GetMouseButton(0) && !isSkill1 && !isSkill2 && !isSkill3 && !isSkill4 && !isDie)
        {
            if (!isAttack)
            {
                animator.SetBool("isMove", false);

                isMove = false;
                isAttack = true;

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

                    Invoke("AttackOut", 0.3f);
            }
        }
    }

    public void AttackOut()
    {
        isAttack = false;
    }

    public void BaseAtk1_Eff()
    {
        StartCoroutine(Play_BaseAtk1_Eff());
    }

    IEnumerator Play_BaseAtk1_Eff()
    {
        BaseAtk_1_Eff.transform.position = BaseAtk_1_Point.transform.position;

        Vector3 d2 = BaseAtk_1_Eff.transform.position - skill_Look.transform.position;
        d2.y = 0.0f;
        Quaternion q2 = Quaternion.LookRotation(d2);
        BaseAtk_1_Eff.transform.rotation = q2 * Quaternion.Euler(0f, 0f, 0f);

        BaseAtk_1_Eff.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        BaseAtk_1_Eff.SetActive(false);
    }

    public void BaseAtk2_Eff()
    {
        StartCoroutine(Play_BaseAtk2_Eff());
    }

    IEnumerator Play_BaseAtk2_Eff()
    {
        BaseAtk_2_Eff.transform.position = BaseAtk_2_Point.transform.position;

        Vector3 d2 = BaseAtk_2_Eff.transform.position - skill_Look.transform.position;
        d2.y = 0.0f;
        Quaternion q2 = Quaternion.LookRotation(d2);
        BaseAtk_2_Eff.transform.rotation = q2 * Quaternion.Euler(75f, 0f, 90f);

        BaseAtk_2_Eff.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        BaseAtk_2_Eff.SetActive(false);
    }

    public void BaseAtk3_Eff()
    {
        StartCoroutine(Play_BaseAtk3_Eff());
    }

    IEnumerator Play_BaseAtk3_Eff()
    {
        BaseAtk_3_Eff.transform.position = BaseAtk_3_Point.transform.position;

        Vector3 d2 = BaseAtk_3_Eff.transform.position - skill_Look.transform.position;
        d2.y = 0.0f;
        Quaternion q2 = Quaternion.LookRotation(d2);
        BaseAtk_3_Eff.transform.rotation = q2 * Quaternion.Euler(0f, 0f, 0f);

        BaseAtk_3_Eff.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        BaseAtk_3_Eff.SetActive(false);
    }

    public void BaseAtk4_Eff()
    {
        StartCoroutine(Play_BaseAtk4_Eff());
    }

    IEnumerator Play_BaseAtk4_Eff()
    {
        BaseAtk_4_Eff.transform.position = BaseAtk_4_Point.transform.position;

        Vector3 d2 = BaseAtk_4_Eff.transform.position - skill_Look.transform.position;
        d2.y = 0.0f;
        Quaternion q2 = Quaternion.LookRotation(d2);
        BaseAtk_4_Eff.transform.rotation = q2 * Quaternion.Euler(180f, 180f, 0f);

        BaseAtk_4_Eff.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        BaseAtk_4_Eff.SetActive(false);
    }

    public override void Dodge()
    {
        if (!isDodge && !skillManager5.isDodgeCT && !isDie)
        {
            animator.SetBool("isMove", false);
            animator.SetTrigger("doDodge");
            isDodge = true;
            isHit = true;
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
        if (!isSkill1 && !isSkill2 && !isSkill3 && !isSkill4 && !isAttack && !isDodge && !isDie)
        {
            if (isMove)
            {
                Vector3 dir = destination - transform.position;
                transform.forward = dir;
                transform.position += dir.normalized * Time.deltaTime * moveSpeed;
            }

            if (Vector3.Distance(transform.position, destination) <= 0.1f)
            {
                isMove = false;
                animator.SetBool("isMove", false);
            }
        }
    }

    public override void Skill_1()
    {
        if (!isSkill1 && !isSkill2 && !isSkill3 && !isSkill4 && !isAttack && !isDodge && !skillManager1.isSkill1CT && !isDie) 
        {
            animator.SetBool("isMove", false);

            isMove = false;
            isSkill1 = true;

            animator.SetTrigger("doSkill1");

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

    public void Skill01_Event01()
    {
        StartCoroutine("ShurikenShot");
    }

    
    IEnumerator ShurikenShot()
    {
        GameObject shuriken = assassin_ObjPoolRef.ShurikenFromPool_Q();
        shuriken.transform.position = firePoint.transform.position; // 발사 위치 설정
        shuriken.transform.Rotate(0, 0, 90);
        shuriken.SetActive(true); // 발사체 활성화


        Vector3 d2 = shuriken.transform.position - skill_Look.transform.position;
        d2.y = 0.0f;
        Quaternion q2 = Quaternion.LookRotation(d2);
        shuriken.transform.rotation = q2 * Quaternion.Euler(90f, 180f, 0f);

        yield return new WaitForSeconds(0f);
    }

    public override void Skill_2()
    {
        if (!isSkill1 && !isSkill2 && !isSkill3 && !isSkill4 && !isAttack && !isDodge && !skillManager2.isSkill2CT && !isDie)
        {
            animator.SetBool("isMove", false);

            isMove = false;
            isSkill2 = true;

            animator.SetTrigger("doSkill2");

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

    public void Skill2Atk1_Eff()
    {
        StartCoroutine(Play_Skill2Atk_1_Eff());
    }


    IEnumerator Play_Skill2Atk_1_Eff()
    {
        Skill2Atk_1_Eff.transform.position = Skill2Atk_1_Point.transform.position;

        Vector3 d2 = Skill2Atk_1_Eff.transform.position - skill_Look.transform.position;
        d2.y = 0.0f;
        Quaternion q2 = Quaternion.LookRotation(d2);
        Skill2Atk_1_Eff.transform.rotation = q2 * Quaternion.Euler(0f, 0f, -35f);

        Skill2Atk_1_Eff.SetActive(true);

        yield return new WaitForSeconds(0.2f);

        Skill2Atk_1_Eff.SetActive(false);
    }

    public void Skill2Atk2_Eff()
    {
        StartCoroutine(Play_Skill2Atk_2_Eff());
    }


    IEnumerator Play_Skill2Atk_2_Eff()
    {
        Skill2Atk_2_Eff.transform.position = Skill2Atk_2_Point.transform.position;

        Vector3 d2 = Skill2Atk_2_Eff.transform.position - skill_Look.transform.position;
        d2.y = 0.0f;
        Quaternion q2 = Quaternion.LookRotation(d2);
        Skill2Atk_2_Eff.transform.rotation = q2 * Quaternion.Euler(0f, 0f, 0f);

        Skill2Atk_2_Eff.SetActive(true);

        yield return new WaitForSeconds(0.3f);

        Skill2Atk_2_Eff.SetActive(false);
    }

    public void Skill2Atk3_Eff()
    {
        StartCoroutine(Play_Skill2Atk_3_Eff());
    }


    IEnumerator Play_Skill2Atk_3_Eff()
    {
        Skill2Atk_3_Eff.transform.position = Skill2Atk_3_Point.transform.position;

        Vector3 d2 = Skill2Atk_3_Eff.transform.position - skill_Look.transform.position;
        d2.y = 0.0f;
        Quaternion q2 = Quaternion.LookRotation(d2);
        Skill2Atk_3_Eff.transform.rotation = q2 * Quaternion.Euler(75f, 0f, 90f);

        Skill2Atk_3_Eff.SetActive(true);

        yield return new WaitForSeconds(0.3f);

        Skill2Atk_3_Eff.SetActive(false);
    }

    public void Skill02_Event01()
    {
        StartCoroutine("ShurikenShot2");
    }


    IEnumerator ShurikenShot2()
    {
        GameObject shuriken = assassin_ObjPoolRef.ShurikenFromPool_W();
        shuriken.transform.position = firePoint.transform.position; // 발사 위치 설정
        shuriken.transform.Rotate(0, 0, 90);
        shuriken.SetActive(true); // 발사체 활성화

        Vector3 d2 = shuriken.transform.position - skill_Look.transform.position;
        d2.y = 0.0f;
        Quaternion q2 = Quaternion.LookRotation(d2);
        shuriken.transform.rotation = q2 * Quaternion.Euler(90f, 180f, 0f);


        Transform[] children = new Transform[shuriken.transform.childCount];
        for (int i = 0; i < shuriken.transform.childCount; i++)
        {
            children[i] = shuriken.transform.GetChild(i);
        }

        // 각 자식 오브젝트를 순차적으로 활성화합니다.
        foreach (Transform child in children)
        {
            child.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.04f);
        }
    }



    public override void Skill_3()
    {
        if (!isSkill1 && !isSkill2 && !isSkill3 && !isSkill4 && !isAttack && !isDodge && !skillManager3.isSkill3CT && !isDie)
        {
            animator.SetBool("isMove", false);

            isMove = false;
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
        Vector3 dodgeEndPosition = transform.position + dodgeDirection * EskillDistance;

        StartCoroutine(MoveDuring(dodgeStartPosition, dodgeEndPosition, 0.2f));
    }

    public void Skill3Atk1_Eff()
    {
        StartCoroutine(Play_Skill3Atk_1_Eff());
    }


    IEnumerator Play_Skill3Atk_1_Eff()
    {
        Skill3Atk_1_Eff.SetActive(true);
        yield return new WaitForSeconds(2f);
        Skill3Atk_1_Eff.SetActive(false);
    }

    public void Skill3Atk2_Eff()
    {
        StartCoroutine(Play_Skill3Atk_2_Eff());
    }


    IEnumerator Play_Skill3Atk_2_Eff()
    {
        //Vector3 skillEff_Position = transform.position;
        Skill3Atk_3_Point.transform.position = transform.position;
        Skill3Atk_3_Eff.SetActive(true);
        yield return new WaitForSeconds(2f);
        Skill3Atk_3_Eff.SetActive(false);
    }

    public override void Skill_4()
    {
        if (!isSkill1 && !isSkill2 && !isSkill3 && !isSkill4 && !isAttack && !isDodge && !skillManager4.isSkill4CT && !isDie)
        {
            animator.SetBool("isMove", false);

            isMove = false;
            isSkill4 = true;

            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;

            if (Physics.Raycast(ray, out rayHit, 100))
            {
                Vector3 dodgeDirection = rayHit.point - transform.position;
                dodgeDirection.y = 0;
                dodgeDirection.Normalize(); // 벡터를 정규화합니다.
                transform.LookAt(transform.position + dodgeDirection);

                Vector3 dodgeStartPosition = transform.position;
                Vector3 dodgeEndPosition = transform.position + dodgeDirection * 25f;

                StartCoroutine(MoveDuring(dodgeStartPosition, dodgeEndPosition, 0.1f));
                animator.SetTrigger("doSkill4");
            }
        }
    }

    public void Skill4Atk1_Eff()
    {
        StartCoroutine(Play_Skill4Atk_1_Eff());
    }

    IEnumerator Play_Skill4Atk_1_Eff()
    {
        // 이펙트 초기 위치 설정
        Skill4Atk_1_Eff.transform.position = Skill4Atk_1_Point.transform.position;
        Skill4Atk_2_Eff.transform.position = Skill4Atk_2_Point.transform.position;
        Skill4Atk_3_Eff.transform.position = Skill4Atk_3_Point.transform.position;

        // 이펙트 초기 회전 설정
        Vector3 d2 = Skill4Atk_1_Eff.transform.position - skill_Look.transform.position;
        d2.y = 0.0f;
        Quaternion q2 = Quaternion.LookRotation(d2);
        Skill4Atk_1_Eff.transform.rotation = q2 * Quaternion.Euler(0f, 0f, 0f);
        Skill4Atk_2_Eff.transform.rotation = q2 * Quaternion.Euler(0f, 0f, 0f);
        Skill4Atk_3_Eff.transform.rotation = q2 * Quaternion.Euler(0f, 0f, 0f);

        // 이펙트 활성화
        Skill4Atk_1_Eff.SetActive(true);
        Skill4Atk_2_Eff.SetActive(true);
        Skill4Atk_3_Eff.SetActive(true);

        // 부모 오브젝트의 스케일 저장
        Vector3 parentScale = Skill4Atk_1_Eff.transform.localScale;

        // 자식 오브젝트의 스케일 변화 애니메이션 - 1에서 2로
        float elapsedTime = 0f;
        while (elapsedTime < 0.3f)
        {
            float scale = Mathf.Lerp(1f, 2f, elapsedTime / 0.3f);
            SetScaleRecursive(Skill4Atk_1_Eff.transform, scale, parentScale);
            SetScaleRecursive(Skill4Atk_2_Eff.transform, scale, parentScale);
            SetScaleRecursive(Skill4Atk_3_Eff.transform, scale, parentScale);

            elapsedTime += Time.deltaTime;
            yield return null;
        }


        // 자식 오브젝트의 스케일 변화 애니메이션 - 2에서 1로
        elapsedTime = 0f;
        while (elapsedTime < 0.1f)
        {
            float scale = Mathf.Lerp(2f, 1f, elapsedTime / 0.1f);
            SetScaleRecursive(Skill4Atk_1_Eff.transform, scale, parentScale);
            SetScaleRecursive(Skill4Atk_2_Eff.transform, scale, parentScale);
            SetScaleRecursive(Skill4Atk_3_Eff.transform, scale, parentScale);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 이펙트 비활성화
        yield return new WaitForSeconds(0.1f);

        // 이펙트 비활성화
        Skill4Atk_1_Eff.SetActive(false);
        Skill4Atk_2_Eff.SetActive(false);
        Skill4Atk_3_Eff.SetActive(false);
    }

    void SetScaleRecursive(Transform parent, float scale, Vector3 parentScale)
    {
        // 부모 오브젝트의 스케일과 역 스케일을 곱하여 자식 오브젝트의 스케일을 설정
        Vector3 childScale = new Vector3(scale / parentScale.x, scale / parentScale.y, scale / parentScale.z);
        parent.localScale = childScale;
        foreach (Transform child in parent)
        {
            SetScaleRecursive(child, scale, parentScale);
        }
    }



    //IEnumerator Play_Skill4Atk_1_Eff()
    //{
    //    Skill4Atk_1_Eff.transform.position = Skill4Atk_1_Point.transform.position;
    //    Skill4Atk_2_Eff.transform.position = Skill4Atk_2_Point.transform.position;
    //    Skill4Atk_3_Eff.transform.position = Skill4Atk_3_Point.transform.position;

    //    Vector3 d2 = Skill4Atk_1_Eff.transform.position - skill_Look.transform.position;
    //    d2.y = 0.0f;
    //    Quaternion q2 = Quaternion.LookRotation(d2);
    //    Skill4Atk_1_Eff.transform.rotation = q2 * Quaternion.Euler(0f, 0f, 0f);
    //    Skill4Atk_2_Eff.transform.rotation = q2 * Quaternion.Euler(0f, 0f, 0f);
    //    Skill4Atk_3_Eff.transform.rotation = q2 * Quaternion.Euler(0f, 0f, 0f);

    //    Skill4Atk_1_Eff.SetActive(true);
    //    Skill4Atk_2_Eff.SetActive(true);
    //    Skill4Atk_3_Eff.SetActive(true);

    //    yield return new WaitForSeconds(0.3f);

    //    Skill4Atk_1_Eff.SetActive(false);
    //    Skill4Atk_2_Eff.SetActive(false);
    //    Skill4Atk_3_Eff.SetActive(false);
    //}

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
            if (playscenemanager.health > 1)
            {
                playscenemanager.HealthDown();
                Debug.Log(other.gameObject.name);

                isHit = true;

                Invoke("HitOut", 3f);
            }
            else
            {
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
                
                
            }
            
        }
    }


    // 3초간 무적
    public void HitOut()
    {
        isHit = false;
    }
}

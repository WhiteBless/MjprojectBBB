using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player 이동속도")]
    public float moveSpeed;

    Rigidbody rigid;
    public Camera cam;
    private Animator animator;

    public bool isMove;
    public bool isDodge;
    public bool isAttack;
    public bool isSkill1;
    public bool isSkill2;
    public bool isSkill3;
    public bool isSkill4;

    public bool spaceDown;

    public bool skill1;
    public bool skill2;
    public bool skill3;
    public bool skill4;

    bool isBorder;

    bool isFireReady = true;

    float fireDelay;

    private Vector3 movement;

    public Skill skillManager1;
    public Skill skillManager2;
    public Skill skillManager3;
    public Skill skillManager4;
    public Skill skillManager5;

    public SkinnedMeshRenderer effect1;
    public SkinnedMeshRenderer effect2;
    public SkinnedMeshRenderer effect3;
    public SkinnedMeshRenderer effect4;

    public MeshCollider Slash_R;
    public MeshCollider Shash_FX;
    public MeshCollider Shash_FX001;
    public MeshCollider Treak;

    public GameObject MouseMoveEffect;

    public Attack attack;

    void Awake()
    {
        cam = Camera.main;
        rigid = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {

    }

    void Update()
    {
        GetInput();

        Dodge();

        Attack();

        Skill1();
        Skill2();
        Skill3();
        Skill4();

        if (isDodge)
        {
            isMove = false;
            isAttack = false;
            isSkill1 = false;
            isSkill2 = false;
            isSkill3 = false;
            isSkill4 = false;
        }

        if (Input.GetMouseButtonDown(0) || skill1 || skill2 || skill3 || skill4)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;
            if (Physics.Raycast(ray, out rayHit, 100))
            {
                Vector3 nextVec = rayHit.point - transform.position;
                nextVec.y = 0;
                transform.LookAt(transform.position + nextVec);
            }

            foreach (Transform child in transform)
            {
                child.localRotation = Quaternion.identity;
            }

            // Enemy 스크립트의 isAttackable 변수 설정
            Enemy[] enemies = FindObjectsOfType<Enemy>();
            foreach (Enemy enemy in enemies)
            {
                enemy.attack.isAttackable = true;
            }
        }

        if (isBorder)
        {
            animator.SetBool("isMove", false);
        }

        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("Ground"))
            {
                Vector3 spawnPosition = new Vector3(hit.point.x, hit.point.y + 2f, hit.point.z);
                // 프리팹을 생성하고 1초 후에 파괴
                GameObject newPrefab = Instantiate(MouseMoveEffect, spawnPosition, Quaternion.identity);
                Destroy(newPrefab, 1.0f);
            }
        }
    }

    void GetInput()
    {
        spaceDown = Input.GetButtonDown("Dodge");

        skill1 = Input.GetButtonDown("Skill1");
        skill2 = Input.GetButtonDown("Skill2");
        skill3 = Input.GetButtonDown("Skill3");
        skill4 = Input.GetButtonDown("Skill4");
    }

    void Move()
    {
        if (Input.GetMouseButton(1))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("Ground"))
            {
                SetMove(hit.point);
            }
            else if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("Wall"))
            {
                NotMove();
            }
        }

        if (isMove)
        {
            Vector3 dir = movement - transform.position;
            animator.transform.forward = dir;

            if (!isBorder)
            {
                transform.position += dir.normalized * Time.deltaTime * moveSpeed;
            }
        }

        if (Vector3.Distance(transform.position, movement) <= 0.1f)
        {
            isMove = false;
            animator.SetBool("isMove", false);
        }
    }

    void SetMove(Vector3 move)
    {
        movement = move;
        isMove = true;
        animator.SetBool("isMove", true);
    }

    void NotMove()
    {
        isMove = false;
        animator.SetBool("isMove", false);
    }

    void StopToWall()
    {
        //DrawRay(): Scene내에서 Ray를 보여주는 함수
        Debug.DrawRay(transform.position, animator.transform.forward * 5, Color.green);

        isBorder = Physics.Raycast(transform.position, animator.transform.forward, 5, LayerMask.GetMask("Wall"));
    }

    void FixedUpdate()
    {
        StopToWall();

        if (!isDodge && !isAttack && !isSkill1 && !isSkill2 && !isSkill3 && !isSkill4)
        {
            Move();
        }
    }

    void Dodge()
    {
        if (spaceDown && !isDodge && skillManager5.getSkillTimes <= 0)
        {
            animator.SetBool("isMove", false);
            animator.SetTrigger("doDodge");
            isDodge = true;

            SkillOut();

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;

            if (Physics.Raycast(ray, out rayHit, 100))
            {
                Vector3 dodgeDirection = rayHit.point - transform.position;
                dodgeDirection.y = 0;
                dodgeDirection.Normalize(); // 벡터를 정규화합니다.
                transform.LookAt(transform.position + dodgeDirection);

                Vector3 dodgeStartPosition = transform.position;
                Vector3 dodgeEndPosition = transform.position + dodgeDirection * 25f;

                StartCoroutine(MoveDuring(dodgeStartPosition, dodgeEndPosition, 0.7f));
                Invoke("DodgeOut", 0.7f);
            }

            foreach (Transform child in transform)
            {
                child.localRotation = Quaternion.identity;
            }
        }
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

    void DodgeOut()
    {
        isDodge = false;
    }

    void Attack()
    {
        if (Input.GetMouseButtonDown(0) && !isSkill1 && !isSkill2 && !isSkill3 && !isSkill4)
        {
            animator.SetBool("isMove", false);

            isMove = false;
            isAttack = true;
            StartCoroutine("Effect");

            animator.SetTrigger("Attack");

            Invoke("AttackOut", 0.8f);

            Enemy[] enemies = FindObjectsOfType<Enemy>();
            foreach (Enemy enemy in enemies)
            {
                enemy.attack.isAttackable = false;
            }
        }

    }

    void AttackOut()
    {
        isAttack = false;

        effect1.enabled = false;
        effect2.enabled = false;
        effect3.enabled = false;
        effect4.enabled = false;

        Slash_R.enabled = false;
        Shash_FX.enabled = false;
        Shash_FX001.enabled = false;
        Treak.enabled = false;
    }

    void Skill1()
    {
        if (skill1 && !isAttack && !isSkill1 && !isSkill2 && !isSkill3 && !isSkill4 && skillManager1.getSkillTimes <= 0)
        {
            animator.SetBool("isMove", false);

            isMove = false;
            isSkill1 = true;
            StartCoroutine("Effect");

            animator.SetTrigger("doSkill1");

            Invoke("SkillOut", 2f);

            // Enemy 스크립트의 isAttackable 변수 설정
            Enemy[] enemies = FindObjectsOfType<Enemy>();
            foreach (Enemy enemy in enemies)
            {
                enemy.attack.isAttackable = false;
            }
        }
    }

    void Skill2()
    {
        if (skill2 && !isAttack && !isSkill1 && !isSkill2 && !isSkill3 && !isSkill4 && skillManager2.getSkillTimes <= 0)
        {
            animator.SetBool("isMove", false);

            isMove = false;
            isSkill2 = true;
            StartCoroutine("Effect");

            animator.SetTrigger("doSkill2");

            Invoke("SkillOut", 2.3f);

            // Enemy 스크립트의 isAttackable 변수 설정
            Enemy[] enemies = FindObjectsOfType<Enemy>();
            foreach (Enemy enemy in enemies)
            {
                enemy.attack.isAttackable = false;
            }
        }
    }

    void Skill3()
    {
        if (skill3 && !isAttack && !isSkill1 && !isSkill2 && !isSkill3 && !isSkill4 && skillManager3.getSkillTimes <= 0)
        {
            animator.SetBool("isMove", false);

            isMove = false;
            isSkill3 = true;
            StartCoroutine("Effect");

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;

            if (Physics.Raycast(ray, out rayHit, 100))
            {
                Vector3 dodgeDirection = rayHit.point - transform.position;
                dodgeDirection.y = 0;
                dodgeDirection.Normalize(); // 벡터를 정규화합니다.
                transform.LookAt(transform.position + dodgeDirection);

                Vector3 dodgeStartPosition = transform.position;
                Vector3 dodgeEndPosition = transform.position + dodgeDirection * 25f;

                StartCoroutine(MoveDuring(dodgeStartPosition, dodgeEndPosition, 1f));
                Invoke("SkillOut", 1f);
                animator.SetTrigger("doSkill3");

                // Enemy 스크립트의 isAttackable 변수 설정
                Enemy[] enemies = FindObjectsOfType<Enemy>();
                foreach (Enemy enemy in enemies)
                {
                    enemy.attack.isAttackable = false;
                }
            }

            foreach (Transform child in transform)
            {
                child.localRotation = Quaternion.identity;
            }
        }
    }

    void Skill4()
    {
        if (skill4 && !isAttack && !isSkill1 && !isSkill2 && !isSkill3 && !isSkill4 && skillManager4.getSkillTimes <= 0)
        {
            animator.SetBool("isMove", false);

            isMove = false;
            isSkill4 = true;
            StartCoroutine("Effect");

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
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
                Invoke("SkillOut", 1f);

                // Enemy 스크립트의 isAttackable 변수 설정
                // Enemy 스크립트의 isAttackable 변수 설정
                Enemy[] enemies = FindObjectsOfType<Enemy>();
                foreach (Enemy enemy in enemies)
                {
                    enemy.attack.isAttackable = false;
                }
            }

            foreach (Transform child in transform)
            {
                child.localRotation = Quaternion.identity;
            }
        }
    }

    IEnumerator Effect()
    {
        yield return new WaitForSeconds(0.1f);
        effect1.enabled = true;
        effect2.enabled = true;
        effect3.enabled = true;
        effect4.enabled = true;

        Slash_R.enabled = true;
        Shash_FX.enabled = true;
        Shash_FX001.enabled = true;
        Treak.enabled = true;
    }

    void SkillOut()
    {
        isSkill1 = false;
        isSkill2 = false;
        isSkill3 = false;
        isSkill4 = false;

        effect1.enabled = false;
        effect2.enabled = false;
        effect3.enabled = false;
        effect4.enabled = false;

        Slash_R.enabled = false;
        Shash_FX.enabled = false;
        Shash_FX001.enabled = false;
        Treak.enabled = false;
    }
}

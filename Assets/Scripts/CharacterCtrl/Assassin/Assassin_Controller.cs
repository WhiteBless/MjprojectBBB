using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assassin_Controller : Character_BehaviorCtrl_Base
{
    [SerializeField]
    Assassin_ObjPool assassin_ObjPoolRef;
    public Transform firePoint; // �߻� ����
    public GameObject skill_Look;
    [SerializeField]
    LayerMask groundLayer; // ������ ���̾�
    public GameObject mouseMoveEffect; // �̵��� ���콺 Ŭ�� ����Ʈ
    public Camera mainCam;
    private Animator animator;
    private Vector3 destination;
    public float moveSpeed;
    public float DodgeDistance;
    public float EskillDistance;

    private bool isMove;
    private bool isDodge;
    private bool isAttack;
    public bool isSkill1;
    private bool isSkill2;
    private bool isSkill3;
    private bool isSkill4;

    RaycastHit eSkillRayHit;

    public GameObject BaseAtk_1_Eff;
    public GameObject BaseAtk_2_Eff;

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

        if (Input.GetMouseButtonDown(0) || Input.GetKey(KeySetting.Keys[KeyAction.Skill1]) || Input.GetKey(KeySetting.Keys[KeyAction.Skill2])
             || Input.GetKey(KeySetting.Keys[KeyAction.Skill3]) || Input.GetKey(KeySetting.Keys[KeyAction.Skill4])) //|| skill1 || skill2 || skill3 || skill4|| 
        {
            if (!isAttack && !isDodge && !isSkill1 && !isSkill2 && !isSkill3 && !isSkill4)
            {
                Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
                RaycastHit rayHit;
                if (Physics.Raycast(ray, out rayHit, 100)) // ray�� 100�� ���̱��� ������� �浹�� ������ rayHit�� ����
                {
                    Vector3 nextVec = rayHit.point - transform.position; // rayHit.point : ray�� �浹�� ����, transform.position : ĳ������ ���� ��ġ
                    nextVec.y = 0;                                       // y���� 0���� �Ͽ� ���򼱿��� �ٶ� �� �ֵ��� ��
                    transform.LookAt(transform.position + nextVec); // ĳ���Ͱ� ���� ���� ���͸� �ٶ󺸵��� ȸ���մϴ�.
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
                // �������� �����ϰ� 1�� �Ŀ� �ı�
                GameObject newPrefab = Instantiate(mouseMoveEffect, spawnPosition, Quaternion.identity);

                // ȸ�� ���� �����մϴ�. x ���� 90���� �����մϴ�.
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

        if (Input.GetKey(KeySetting.Keys[KeyAction.Skill1]))
        {
            Skill_1();
        }

        if (Input.GetKey(KeySetting.Keys[KeyAction.Skill2]))
        {
            Skill_2();
        }

        if (Input.GetKey(KeySetting.Keys[KeyAction.Skill3]))
        {
            Skill_3();
        }

        if (Input.GetKey(KeySetting.Keys[KeyAction.Skill4]))
        {
            Skill_4();
        }

        if (Input.GetKey(KeySetting.Keys[KeyAction.Dodge]))
        {
            Dodge();
        }

    }

    public override void Attack()
    {

        if (Input.GetMouseButtonDown(0) && !isSkill1 && !isSkill2 && !isSkill3 && !isSkill4)
        {
            if (!isAttack)
            {
                animator.SetBool("isMove", false);

                isMove = false;
                isAttack = true;

                animator.SetTrigger("Attack");

                Invoke("AttackOut", 0.2f);
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
        BaseAtk_2_Eff.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        BaseAtk_2_Eff.SetActive(false);
    }

    public override void Dodge()
    {
        if (!isDodge)
        {
            animator.SetBool("isMove", false);
            animator.SetTrigger("doDodge");
            isDodge = true;
            isMove = false;

            SkillOut();

            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;

            if (Physics.Raycast(ray, out rayHit, 100))
            {
                Vector3 dodgeDirection = rayHit.point - transform.position;
                dodgeDirection.y = 0;
                dodgeDirection.Normalize(); // ���͸� ����ȭ�մϴ�.
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
    }

    IEnumerator MoveDuring(Vector3 startPosition, Vector3 endPosition, float duration)
    {
        float startTime = Time.time;

        while (Time.time - startTime < duration)
        {
            // Dodge ���� �߿� �÷��̾ �����Դϴ�.
            float t = (Time.time - startTime) / duration; // �ð� ���
            transform.position = Vector3.Lerp(startPosition, endPosition, t); // ���� �������� ���� �������� t��ŭ �ð� �ҿ�
            yield return null;
        }

        transform.position = endPosition;
    }

    private void SetDestination(Vector3 dest)
    {
        destination = dest;
        isMove = true;
        animator.SetBool("isMove", true);
    }

    public override void Move()
    {
        if (!isSkill1 && !isSkill2 && !isSkill3 && !isSkill4 && !isAttack && !isDodge)
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
        if (!isSkill1)
        {
            animator.SetBool("isMove", false);

            isMove = false;
            isSkill1 = true;

            animator.SetTrigger("doSkill1");

            //attack_Collider.Q_Use();

            Invoke("SkillOut", 2f);
        }
    }

    public void Skill01_Event01()
    {
        StartCoroutine("ShurikenShot");
    }

    public void Skill01_Event02()
    {
        StartCoroutine("ShurikenShot");
    }

    public void Skill01_Event03()
    {
        StartCoroutine("ShurikenShot");
    }
    IEnumerator ShurikenShot()
    {
        GameObject shuriken = assassin_ObjPoolRef.ShurikenFromPool();
        shuriken.transform.position = firePoint.transform.position; // �߻� ��ġ ����
        shuriken.transform.Rotate(0, 0, 90);
        shuriken.SetActive(true); // �߻�ü Ȱ��ȭ


        Vector3 d2 = shuriken.transform.position - skill_Look.transform.position;
        d2.y = 0.0f;
        Quaternion q2 = Quaternion.LookRotation(d2);
        shuriken.transform.rotation = q2 * Quaternion.Euler(90f, 180f, 0f);

        yield return new WaitForSeconds(0f);
    }

    public override void Skill_2()
    {
        if (!isSkill2)
        {
            animator.SetBool("isMove", false);

            isMove = false;
            isSkill2 = true;

            animator.SetTrigger("doSkill2");

            Invoke("SkillOut", 2.3f);
        }
    }

    public override void Skill_3()
    {
        if (!isSkill3)
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
        dodgeDirection.Normalize(); // ���͸� ����ȭ�մϴ�.
        transform.LookAt(transform.position + dodgeDirection);

        Vector3 dodgeStartPosition = transform.position;
        dodgeStartPosition.y = 0;
        Vector3 dodgeEndPosition = transform.position + dodgeDirection * EskillDistance;

        StartCoroutine(MoveDuring(dodgeStartPosition, dodgeEndPosition, 0.2f));

        Invoke("SkillOut", 0.7f);
    }

    public override void Skill_4()
    {
        if (!isSkill4)
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
                dodgeDirection.Normalize(); // ���͸� ����ȭ�մϴ�.
                transform.LookAt(transform.position + dodgeDirection);

                Vector3 dodgeStartPosition = transform.position;
                Vector3 dodgeEndPosition = transform.position + dodgeDirection * 25f;

                StartCoroutine(MoveDuring(dodgeStartPosition, dodgeEndPosition, 0.1f));
                animator.SetTrigger("doSkill4");
                Invoke("SkillOut", 1f);
            }
        }
    }

    public void SkillOut()
    {
        isSkill1 = false;
        isSkill2 = false;
        isSkill3 = false;
        isSkill4 = false;
    }
}

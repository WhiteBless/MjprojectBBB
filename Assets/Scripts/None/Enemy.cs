using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum Type { A, B, C, D };
    

    public Attack attack;

    [Header("적 유형")]
    public Type enemyType;

    [Header("적 체력")]
    public int maxHealth;
    public int curHealth;

    [Header("적 추적")]
    public Transform target;

    [Header("공격 범위")]
    public BoxCollider meleeArea;
    public GameObject bullet;

    public bool isChase;

    public bool isAttack;

    public bool isDead;

    public Rigidbody rigid;
    public BoxCollider boxCollider;
    public MeshRenderer[] meshs;

    public Animator anim;

    //NavMesh: NavAgent가 경로를 그리기 위한 바탕(Mesh)
    //NavMeshsh는 Static 오브젝트만 Bake 가능
    public NavMeshAgent nav;

    public float attackInterval = 2f; // 공격 간격
    private Coroutine attackCoroutine; // 공격 코루틴 참조

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        meshs = GetComponentsInChildren<MeshRenderer>();

        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

        Invoke("ChaseStart", 2);
    }

    void ChaseStart()
    {
        isChase = true;
        anim.SetBool("isWalk", true);
    }

    void Update()
    {
        if (nav.enabled && enemyType != Type.D)
        {
            nav.SetDestination(target.position);

            nav.isStopped = !isChase;
        }

        if (Input.GetMouseButtonDown(0) && attack.isAttackable)
        {
            attackCoroutine = StartCoroutine(AttackWithDelay());
        }
    }

    void FreezeVelocity()
    {
        if (isChase)
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
        }
    }

    void Targeting()
    {
        if (!isDead && enemyType != Type.C)
        {
            float targetRadius = 0;
            float targetRange = 0;

            switch (enemyType)
            {
                case Type.A:
                    targetRadius = 1.5f;
                    targetRange = 3f;
                    break;

                case Type.B:
                    targetRadius = 1f;
                    targetRange = 12f;
                    break;

                case Type.C:
                    targetRadius = 0.5f;
                    targetRange = 25f;
                    break;
            }

            RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange, LayerMask.GetMask("Player"));

            if (rayHits.Length > 0 && !isAttack)
            {
                StartCoroutine(Attack());
            }
        }
    }

    IEnumerator Attack()
    {
        if (attack.isAttackable)
        {
            isChase = false;
            isAttack = true;
            anim.SetBool("isAttack", true);

            switch (enemyType)
            {
                case Type.A:
                    yield return new WaitForSeconds(0.2f);

                    meleeArea.enabled = true;

                    yield return new WaitForSeconds(1f);

                    meleeArea.enabled = false;

                    yield return new WaitForSeconds(1f);
                    break;

                case Type.B:
                    yield return new WaitForSeconds(0.1f);

                    rigid.AddForce(transform.forward * 20, ForceMode.Impulse);
                    meleeArea.enabled = true;

                    yield return new WaitForSeconds(0.5f);

                    rigid.velocity = Vector3.zero;
                    meleeArea.enabled = false;

                    yield return new WaitForSeconds(2f);
                    break;

                case Type.C:
                    yield return new WaitForSeconds(0.5f);

                    GameObject instantBullet = Instantiate(bullet, transform.position, transform.rotation);
                    Rigidbody rigidBullet = instantBullet.GetComponent<Rigidbody>();
                    rigidBullet.velocity = transform.forward * 20;

                    yield return new WaitForSeconds(2f);
                    break;
            }

            isChase = true;
            isAttack = false;
            anim.SetBool("isAttack", false);
        }
    }
    IEnumerator OnDamage(Vector3 reactVec, bool isGrenade)
    {
        foreach (MeshRenderer mesh in meshs)
            mesh.material.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        if (curHealth > 0)
        {
            foreach (MeshRenderer mesh in meshs)
                mesh.material.color = Color.white;
        }
        else
        {
            foreach (MeshRenderer mesh in meshs)
                mesh.material.color = Color.gray;

            gameObject.layer = 14;

            isDead = true;

            isChase = false;
            nav.enabled = false;
            anim.SetTrigger("doDie");

            Destroy(gameObject, 4);
        }
    }

    IEnumerator AttackWithDelay()
    {
        attack.isAttackable = false; // 공격 불가능 상태로 설정

        yield return new WaitForSeconds(attackInterval); // 공격 간격만큼 대기

        // 공격 애니메이션 재생 및 공격 로직 수행

        attack.isAttackable = true; // 공격 가능 상태로 다시 설정
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Melee") && other.gameObject.CompareTag("Enemy") && attack.isAttackable)
        {
            Attack weapon = other.GetComponent<Attack>();
            if (weapon != null)
            {
                int weaponDamage = weapon.damage; // 무기의 공격력 값을 가져옵니다.
                curHealth -= weaponDamage; // 체력을 무기의 공격력만큼 감소시킵니다.
                curHealth = Mathf.Max(curHealth, 0); // 체력을 0 미만으로 떨어지지 않도록 고정합니다.

                Vector3 reactVec = transform.position - other.transform.position;

                StartCoroutine(OnDamage(reactVec, false));

                attack.isAttackable = false; // 일정 시간 동안 공격 불가능 상태로 설정
                StartCoroutine(ResetAttackableState());
            }
        }
    }

    IEnumerator ResetAttackableState()
    {
        yield return new WaitForSeconds(attackInterval); // 공격 간격만큼 대기

        attack.isAttackable = true; // 공격 가능 상태로 다시 설정
    }

    void FixedUpdate()
    {
        Targeting();
        FreezeVelocity();
    }
}

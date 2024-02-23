using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assassin_Controller : Character_BehaviorCtrl_Base
{
    [SerializeField]
    LayerMask groundLayer; // 지면의 레이어
    public GameObject mouseMoveEffect; // 이동시 마우스 클릭 이펙트
    public Camera mainCam;
    private Animator animator;
    private Vector3 destination;
    public float moveSpeed;

    private bool isMove;

    void Awake()
    {
        mainCam = Camera.main;
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Update()
    {
        if (Input.GetMouseButtonDown(0)) //|| skill1 || skill2 || skill3 || skill4
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
    }

    private void SetDestination(Vector3 dest)
    {
        destination = dest;
        isMove = true;
        animator.SetBool("isMove", true);
    }

    public override void Move()
    {
        if(isMove)
        {
            Vector3 dir = destination - transform.position;
            transform.forward = dir;
            transform.position += dir.normalized * Time.deltaTime * moveSpeed;
        }

        if(Vector3.Distance(transform.position, destination) <= 0.1f)
        {
            isMove = false;
            animator.SetBool("isMove", false);
        }
    }
}
 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenController : MonoBehaviour
{
    public float moveSpeed = 10f; // 이동 속도
    public float maxDistance = 30f; // 최대 이동 거리

    public bool wSkill;
    public bool wSkillDis;

    [SerializeField]
    Assassin_ObjPool assassin_ObjPoolRef;

    void Start()
    {
        
    }

    
    void Update()
    {
        Move();
    }

    private void Move()
    {
        if (!wSkill)
        {
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

            // 발사체 이동 거리 체크
            if (Vector3.Distance(transform.position, transform.parent.parent.parent.GetChild(1).position) > maxDistance)
            {
                gameObject.SetActive(false); // 일정 거리 이동 후 비활성화
            }
        }
        else
        {
            if (!wSkillDis)
            {
                transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

                // 발사체 이동 거리 체크
                if (Vector3.Distance(transform.position, transform.parent.parent.parent.parent.GetChild(1).position) > maxDistance)
                {
                    gameObject.SetActive(false);
                }
            }
            else
            {
                transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

                // 발사체 이동 거리 체크
                if (Vector3.Distance(transform.position, transform.parent.parent.parent.parent.GetChild(1).position) > maxDistance)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
}

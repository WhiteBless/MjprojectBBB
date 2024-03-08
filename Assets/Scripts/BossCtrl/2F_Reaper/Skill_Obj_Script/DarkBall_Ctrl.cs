using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkBall_Ctrl : MonoBehaviour
{
    public GameObject Target;
    [SerializeField]
    float DarkBall_RotSpeed;
    [SerializeField]
    float DarkBall_Speed;

    // Update is called once per frame
    void Update()
    {
        LookAtPlayer();
        DarkBall_Move();
    }

    public void LookAtPlayer()
    {
        // 플레이어를 바라보도록
        // this.transform.LookAt(Target.transform);

        Vector3 dir;

        dir = Target.transform.position - transform.position;
        // y축 정보 제거

        Quaternion rotation = Quaternion.LookRotation(dir, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * DarkBall_RotSpeed);
    }

    public void DarkBall_Move()
    {
        // 앞으로 이동
        transform.Translate(Vector3.forward * DarkBall_Speed * Time.deltaTime);
    }
}

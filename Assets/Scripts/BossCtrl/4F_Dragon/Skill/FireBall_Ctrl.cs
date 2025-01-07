using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall_Ctrl : MonoBehaviour
{
    [SerializeField]
    GameObject Target;
    [SerializeField]
    float FireBall_Speed;
    [SerializeField]
    float FireBall_Rot_Speed;
    [SerializeField]
    bool isMove;
    [SerializeField]
    bool isLock;

    [SerializeField]
    GameObject FireBall_VFX;
    [SerializeField]
    GameObject Explosion_VFX;

    Vector3 dir;

    [SerializeField]
    float Spawn_MaxTime;
    [SerializeField]
    float Current_SpawnTime;

    [SerializeField]
    float Rot_MaxTime;
    [SerializeField]
    float Current_RotTime;

    private void OnEnable()
    {
        isMove = true;
        StartCoroutine(VisibleTime());
        StartCoroutine(Rotate_Time());
    }

    private void OnDisable()
    {
        Current_RotTime = 0.0f;
        Current_SpawnTime = 0.0f;
        FireBall_VFX.SetActive(true);
        Explosion_VFX.SetActive(false);
        isLock = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        isMove = false;
        Target = GameObject.FindWithTag("TargetPoint");
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isMove)
        {
            transform.Translate(Vector3.forward * FireBall_Speed * Time.deltaTime);
            LookAtPlayer();
        }
    }

    public void LookAtPlayer()
    {
        if (!isLock)
        {
            dir = Target.transform.position - transform.position;
            // y축 정보 제거
            // dir.y = 0.0f;

            Quaternion rotation = Quaternion.LookRotation(dir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * FireBall_Rot_Speed);
        }
    }

    IEnumerator Rotate_Time()
    {
        // Spawn_MaxTime 까지 스킬 지속, 이후 스킬 비활성화
        while (Current_RotTime <= Rot_MaxTime)
        {
            Current_RotTime += Time.deltaTime;
            yield return null;
        }

        isLock = true;
    }

    IEnumerator VisibleTime()
    {
        // Spawn_MaxTime 까지 스킬 지속, 이후 스킬 비활성화
        while (Current_SpawnTime <= Spawn_MaxTime)
        {
            Current_SpawnTime += Time.deltaTime;
            yield return null;
        }

        isMove = false;
        FireBall_VFX.SetActive(false);
        Explosion_VFX.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        this.gameObject.SetActive(false);
    }
}

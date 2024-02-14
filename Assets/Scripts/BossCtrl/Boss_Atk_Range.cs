using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Atk_Range : MonoBehaviour
{
    public Transform obj;
    public Reaper_Controller reaperCtrl; 

    // Update is called once per frame
    void Update()
    {
        this.transform.position = obj.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            reaperCtrl.isChase = false;
            reaperCtrl.isMove = false;
            //reaperCtrl.Reaper_animator.SetBool("isMove", reaperCtrl.isMove);

            // TODO ## 2층 보스 패턴 작동 구현은 여기다가 기입
            // 공격중이 아니라면
            if (reaperCtrl.isAttacking == false && reaperCtrl.isTargetFind == false)
            {
                reaperCtrl.Reaper_animator.SetTrigger("BaseAtk");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("3");
            reaperCtrl.isChase = true;
        }
    }
}

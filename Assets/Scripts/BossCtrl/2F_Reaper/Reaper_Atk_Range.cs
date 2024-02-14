using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reaper_Atk_Range : MonoBehaviour
{
    public Transform obj;
    public Reaper_Controller reaperCtrl;

    public GameObject player;

    // Update is called once per frame
    void Update()
    {
        this.transform.position = obj.position;
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        player = other.gameObject;

    //        reaperCtrl.isChase = false;
    //        reaperCtrl.isMove = false;

    //        if (player != null && reaperCtrl.isAttacking == false)
    //        {
    //            reaperCtrl.Skill_Think();
    //        }
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        // 범위를 벗어나면 플레이어 추격
    //        player = null;
    //        reaperCtrl.isChase = true;
    //        reaperCtrl.isMove = true;
    //    }
    //}
}

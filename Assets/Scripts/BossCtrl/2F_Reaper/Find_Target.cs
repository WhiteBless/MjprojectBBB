using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Find_Target : MonoBehaviour
{
    [SerializeField]
    Reaper_Controller ReaperCtrl;

    [SerializeField]
    GameObject HP_Canvas;
    [SerializeField]
    Boss_HP_Controller boss_hp_ctrl;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            this.GetComponent<SphereCollider>().enabled = false;

            // 범위 안에 들었을 때 타켓을 지정한다
            ReaperCtrl.Target = other.gameObject;
            ReaperCtrl.reaperState = ReaperState.RaidStart;

            // HP바 활성화
            ReaperCtrl.boss_hp_ctrl.Boss_HP_Canvas.SetActive(true);

            // HP바 UI 컨트롤러에 현재 보스의 HP를 준다
            HP_Canvas.GetComponent<BossHP_UI_Ctrl>().BossMax_HP = boss_hp_ctrl.BossMaxHP;
            HP_Canvas.GetComponent<BossHP_UI_Ctrl>().BossCur_HP = boss_hp_ctrl.BossMaxHP;
            HP_Canvas.GetComponent<BossHP_UI_Ctrl>().Refresh_BossHP();

            // 레이드 시작 시 현재 체력 최대 체력으로 설정
            boss_hp_ctrl.BossCurHP = boss_hp_ctrl.BossMaxHP;
            
            StartCoroutine(SeePlayer());
        }
    }

    IEnumerator SeePlayer()
    {
        ReaperCtrl.Reaper_animator.SetTrigger("IsFindPlayer");

        yield return new WaitForSeconds(ReaperCtrl.Reaper_animator.GetCurrentAnimatorStateInfo(0).length + 3.0f);

        // 거리에 따라 원거리 공격 근거리 공격
        if (ReaperCtrl.TargetDistance > ReaperCtrl.Skill_Think_Range)
        {
            ReaperCtrl.Reaper_Long_nextAct();
        }
        else if (ReaperCtrl.TargetDistance <= ReaperCtrl.Skill_Think_Range)
        {
            ReaperCtrl.Reaper_Short_nextAct();
        }
    }
}

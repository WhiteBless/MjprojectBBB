using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class Find_Target : MonoBehaviour
{
    #region Reaper
    [SerializeField]
    Reaper_Controller ReaperCtrl;
    #endregion

    #region
    [SerializeField]
    Treant_Controller TreantCtrl;
    #endregion Treant

    [SerializeField]
    GameObject HP_Canvas;
    [SerializeField]
    Boss_HP_Controller boss_hp_ctrl;

    [SerializeField]
    PlayableDirector PD;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            this.GetComponent<SphereCollider>().enabled = false;

            #region Reaper
            // TODO ## 리퍼 타켓 초기화
            if (this.gameObject.name == "2F_Raid_Start_Collision")
            {
                PD.Play();

                // 범위 안에 들었을 때 타켓을 지정한다
                ReaperCtrl.Target = other.gameObject;
                ReaperCtrl.reaperState = ReaperState.RaidStart;

                StartCoroutine(SeePlayer());
            }
            #endregion

            // TODO ## 나무정령 타켓 초기화
            if (this.gameObject.name == "3F_Raid_Start_Collision")
            {
                // 범위 안에 들었을 때 타켓을 지정한다
                TreantCtrl.isStartRaid = true;
                StartCoroutine(SeePlayer());
            }

            // HP바 활성화
            HP_Canvas.transform.localScale = new Vector3(0.0f, 1.0f, 0.0f);

            // HP바 UI 컨트롤러에 현재 보스의 HP를 준다
            HP_Canvas.GetComponent<BossHP_UI_Ctrl>().BossMax_HP = boss_hp_ctrl.BossMaxHP;
            HP_Canvas.GetComponent<BossHP_UI_Ctrl>().BossCur_HP = boss_hp_ctrl.BossMaxHP;
            HP_Canvas.GetComponent<BossHP_UI_Ctrl>().Refresh_BossHP();

            // 레이드 시작 시 현재 체력 최대 체력으로 설정
            boss_hp_ctrl.BossCurHP = boss_hp_ctrl.BossMaxHP; 
        }
    }


    IEnumerator SeePlayer()
    {
        #region Reaper
        if (this.gameObject.name == "2F_Raid_Start_Collision")
        {
            ReaperCtrl.Reaper_animator.SetTrigger("IsFindPlayer");

            ReaperCtrl.Reaper_animator.SetFloat("FirstSeeAniSpeed", 0.4f);

            yield return new WaitForSeconds(9.0f);

            ReaperCtrl.Reaper_animator.SetFloat("FirstSeeAniSpeed", 1.0f);

            // 거리에 따라 원거리 공격 근거리 공격
            if (ReaperCtrl.TargetDistance > ReaperCtrl.Skill_Think_Range)
            {
                ReaperCtrl.Reaper_Long_nextAct(0);
            }
            else if (ReaperCtrl.TargetDistance <= ReaperCtrl.Skill_Think_Range)
            {
                ReaperCtrl.Reaper_Short_nextAct(0);
            }
        }
        #endregion

        #region Treant
        if (this.gameObject.name == "3F_Raid_Start_Collision")
        {
            TreantCtrl.Treant_NextAct();
            //yield return new WaitForSeconds(9.0f); 
        }
        #endregion
    }
}

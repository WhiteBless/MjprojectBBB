using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class BossAttRange : MonoBehaviour
{
    public GameObject bossHPCanvas;
    public GameObject IronGuard_obj;
    [SerializeField]
    BossSkillP BossSkill;

    BossAnimator bossAnimator; // BossAnimator 스크립트를 참조하는 변수

    PlayableDirector PD;
    public TimelineAsset[] TimeAssets;

    // Start is called before the first frame update
    void Start()
    {
        bossAnimator = FindObjectOfType<BossAnimator>(); // 부모 오브젝트의 BossAnimator 컴포넌트를 찾아서 bossAnimator 변수에 할당
        PD = GetComponent<PlayableDirector>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // 1층 보스 hp ui연결
        if (other.gameObject.CompareTag("P"))
        {
            PD.Play(TimeAssets[0]);

            GameManager.GMInstance.SoundManagerRef.PlayBGM(SoundManager.BGM.BOSS_1FLOOR);

            bossHPCanvas.transform.localScale = Vector3.one;

            bossAnimator.AttRadyState = true; // AttRadyState를 true로 설정

            BossSkill.Target = other.gameObject.transform.parent.gameObject;
            BossSkill.boss_hp_ctrl.BossMaxHP = BossSkill.IronGuard_MaxHP;
            BossSkill.boss_hp_ctrl.BossCurHP = BossSkill.IronGuard_MaxHP;

            bossHPCanvas.GetComponent<BossHP_UI_Ctrl>().BossMax_HP = BossSkill.IronGuard_MaxHP;
            bossHPCanvas.GetComponent<BossHP_UI_Ctrl>().BossCur_HP = BossSkill.IronGuard_MaxHP;

            bossHPCanvas.GetComponent<BossHP_UI_Ctrl>().Refresh_BossHP();
            this.gameObject.GetComponent<SphereCollider>().enabled = false;

        }
    }

    // 플레이어가 공격 범위 안에 들어왔을 때
    //void OnTriggerStay(Collider other)
    //{
    //    if (other.gameObject.CompareTag("P")) // 충돌한 오브젝트의 태그가 P라면
    //    {
    //        bossAnimator.AttRadyState = true; // AttRadyState를 true로 설정

    //        //bossHPCanvas.SetActive(true);
    //        //bossHPCanvas.GetComponent<BossHP_Ctrl>().BossMax_HP = IronGuard_obj.GetComponent<HPtest>().maxHealth;
    //        //bossHPCanvas.GetComponent<BossHP_Ctrl>().BossCur_HP = IronGuard_obj.GetComponent<HPtest>().maxHealth;
    //    }
    //}

    // 플레이어가 공격 범위를 벗어났을 때
    //void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.CompareTag("P")) // 충돌한 오브젝트의 태그가 Player라면
    //    {
    //        bossAnimator.AttRadyState = false; // AttRadyState를 false로 설정
    //    }
    //}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class Boss_HP_Controller : MonoBehaviour
{
    public GameObject Boss_HP_Canvas;
    public int BossMaxHP;
    public int BossCurHP;
    public bool isAwakening;
    public bool isDead;
    public bool isWSkillChek;

    [SerializeField]
    PlayableDirector PD;

    [SerializeField]
    GameObject[] Boss_Skill_Objects;

    [Header("-----ShakeCam-----")] // 카메라 흔들림 변수
    [SerializeField]
    float CamShake_Intensity;
    [SerializeField]
    float CamShake_Time;


    [Header("-----Reaper-----")] // 2층 보스의 경우 
    [SerializeField]
    int Reaper_SP_1_HP; // 첫번째 광역기 체력
    [SerializeField]
    int Reaper_SP_2_HP;
    [SerializeField]
    int Reaper_SP_3_HP;
    public bool isReaper_SP_ATK_1;
    public bool isReaper_SP_ATK_2;
    public bool isReaper_SP_ATK_3;

    [Header("-----Treant-----")] // 3층 보스의 경우 
    [SerializeField]
    int Treant_Possible_FormChange_HP; //폼 체인지 체력


    [Header("-----Dragon-----")] // 4층 보스의 경우
    [SerializeField]
    bool isTriggerHandled = false;
    const int ChangeThunder_HP = 70;
    public bool isChange_Thunder;

    [SerializeField]
    const int ChangeFire_HP = 40;
    public bool isChange_Fire;


    public PlaySceneManager playSceneManager;



    // Start is called before the first frame update
    void Start()
    {
        Boss_HP_Canvas.transform.localScale = Vector3.zero;
        // PD.GetComponent<PlayableDirector>();
    }

    void Update()
    {
        #region IronGuard2_n_Death
        if (this.gameObject.name == "IronGuard2_n" && BossCurHP <= 0 && !isDead)
        {
            BossCurHP = 0;
            isDead = true;

            // 보스 스킬 비활성화
            this.gameObject.transform.parent.GetChild(1).gameObject.SetActive(false);

            // 콜라이더 비활성화
            this.GetComponent<BoxCollider>().enabled = false;
            // 죽는 모션 재생
            Animator reaperanimator = GetComponent<Animator>();
            reaperanimator.SetTrigger("doDie2");
            Boss_HP_Canvas.transform.localScale = Vector3.zero;
            playSceneManager.BossClear();

            for (int i = 0; i < Boss_Skill_Objects.Length; i++)
            {
                Boss_Skill_Objects[i].SetActive(false);
            }

            PD.Play();
        }
        #endregion

        #region Reaper_Death
        if (this.gameObject.name == "Reaper" && BossCurHP <= 0 && !isDead)
        {
            BossCurHP = 0;
            isDead = true;
            this.GetComponent<CapsuleCollider>().enabled = false;

            Animator reaperanimator = GetComponent<Animator>();
            reaperanimator.SetTrigger("isDeath");
            Boss_HP_Canvas.transform.localScale = Vector3.zero;
            playSceneManager.BossClear();


            for (int i = 0; i < Boss_Skill_Objects.Length; i++)
            {
                Boss_Skill_Objects[i].SetActive(false);
            }

            PD.Play();
        }
        #endregion

        #region Treant_Death
        if (this.gameObject.name == "Treant" && BossCurHP <= 0 && !isDead)
        {
            BossCurHP = 0;
            isDead = true;
            this.GetComponent<CapsuleCollider>().enabled = false;

            Animator reaperanimator = GetComponent<Animator>();
            reaperanimator.SetTrigger("isDeath");
            Boss_HP_Canvas.transform.localScale = Vector3.zero;
            playSceneManager.BossClear();
            PD.Play();
        }
        #endregion
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            Debug.Log(this.name);

            // 보스 피격 소리 재생
            GameManager.GMInstance.SoundManagerRef.PlaySFX(((SoundManager.SFX)Random.Range((int)SoundManager.SFX.BOSS_HIT_1, (int)SoundManager.SFX.CLEAR_SOUND)));
            GameManager.GMInstance.CamShakeRef.ShakeCam(CamShake_Intensity, CamShake_Time);

            // HP가 0이상일 시 작동
            if (BossCurHP > 0)
            {
                #region Reaper 
                if (this.gameObject.name == "Reaper")
                {
                    // 보스 체력이 50퍼 보다 작아지면
                    if (BossCurHP <= (BossMaxHP / 100) * Reaper_SP_1_HP && isReaper_SP_ATK_1 == false)
                    {
                        isReaper_SP_ATK_1 = true;
                    }
                    // 보스 체력이 50퍼 보다 작아지면
                    else if (BossCurHP <= (BossMaxHP / 100) * Reaper_SP_2_HP && isReaper_SP_ATK_2 == false)
                    {
                        isReaper_SP_ATK_2 = true;
                    }
                    // 보스 체력이 50퍼 보다 작아지면
                    else if (BossCurHP <= (BossMaxHP / 100) * Reaper_SP_3_HP && isReaper_SP_ATK_3 == false)
                    {
                        isReaper_SP_ATK_3 = true;
                    }

                    // 보스 체력이 50퍼 보다 작아지면
                    if (BossCurHP <= (BossMaxHP / 100) * 50 && isAwakening == false)
                    {
                        isAwakening = true;
                    }
                }

                #endregion // 리퍼의 hp에 따른 변수 조정

                #region Treant
                // 3층 보스일 경우
                if (this.gameObject.name == "Treant")
                {
                    // 설정한 체력 보다 작아질 경우
                    if (BossCurHP <= (BossMaxHP / 100) * Treant_Possible_FormChange_HP)
                    {
                        this.GetComponent<Treant_Controller>().isStartFormChange = true;
                    }

                    // 방어 상태면 return
                    if (this.GetComponent<Treant_Controller>().isBarrier)
                    {
                        return;
                    }
                }
                #endregion

                #region Dragon
                if (this.gameObject.name == "Dragon")
                {
                    // 보스 체력이 70퍼 보다 작고 번개폼으로 변신하지 않았다면
                    if (BossCurHP <= (BossMaxHP / 100) * ChangeThunder_HP && isChange_Thunder == false)
                    {
                        isChange_Thunder = true;
                    }
                    // 보스 체력이 40퍼 보다 작아지면
                    else if (BossCurHP <= (BossMaxHP / 100) * ChangeFire_HP && isChange_Fire == false)
                    {
                        isChange_Fire = true;
                    }
                }
                #endregion

                //if (!isWSkillChek)
                //{
                //    Attack weapon = other.GetComponent<Attack>();
                //    // Debug.Log("Damage: " + weapon.damage);

                //    BossCurHP -= weapon.damage;
                //}

                Attack weapon = other.GetComponent<Attack>();
                BossCurHP -= weapon.damage;

                Boss_HP_Canvas.GetComponent<BossHP_UI_Ctrl>().BossCur_HP = BossCurHP;
                Boss_HP_Canvas.GetComponent<BossHP_UI_Ctrl>().Refresh_BossHP();
            }
        }
        else if (other.gameObject.name == "DarkBallCounter_Eff")
        {
            // 어둠의 구체 반격
            other.gameObject.SetActive(false);

            BossCurHP -= 1000;

            Boss_HP_Canvas.GetComponent<BossHP_UI_Ctrl>().BossCur_HP = BossCurHP;
            // this.GetComponent<Reaper_Controller>().CurHP = BossCurHP;
            Boss_HP_Canvas.GetComponent<BossHP_UI_Ctrl>().Refresh_BossHP();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isTriggerHandled = false; // 트리거 종료 시 플래그 초기화
    }
}

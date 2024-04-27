using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_HP_Controller : MonoBehaviour
{
    public GameObject Boss_HP_Canvas;
    public int BossMaxHP;
    public int BossCurHP;
    public bool isAwakening;
    public bool isDead;
    public bool isWSkillChek;

    [Header("-----Reaper-----")] // 리퍼의 경우 
    [SerializeField]
    int Reaper_SP_1_HP; // 첫번째 광역기 체력
    [SerializeField]
    int Reaper_SP_2_HP;
    [SerializeField]
    int Reaper_SP_3_HP;
    public bool isReaper_SP_ATK_1;
    public bool isReaper_SP_ATK_2;
    public bool isReaper_SP_ATK_3;



    // Start is called before the first frame update
    void Start()
    {
        Boss_HP_Canvas.transform.localScale = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
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
                }

                #endregion // 리퍼의 hp에 따른 변수 조정

                // 보스 체력이 50퍼 보다 작아지면
                if (BossCurHP <= (BossMaxHP / 100) * 50 && isAwakening == false)
                {
                    isAwakening = true;
                }

                if (!isWSkillChek)
                {
                    Attack weapon = other.GetComponent<Attack>();
                    Debug.Log("Damage: " + weapon.damage);

                    BossCurHP -= weapon.damage;
                }

                Boss_HP_Canvas.GetComponent<BossHP_UI_Ctrl>().BossCur_HP = BossCurHP;
                // this.GetComponent<Reaper_Controller>().CurHP = BossCurHP;
                Boss_HP_Canvas.GetComponent<BossHP_UI_Ctrl>().Refresh_BossHP();
            }
            else
            {
                isDead = true;
                this.GetComponent<CapsuleCollider>().enabled = false;

                #region ReaperDeath
                if (this.gameObject.name == "Reaper")
                {
                    Animator reaperanimator = GetComponent<Animator>();                  
                    reaperanimator.SetTrigger("isDeath");
                }

                #endregion          
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
}

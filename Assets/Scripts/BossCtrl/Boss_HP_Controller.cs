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

    [Header("-----Reaper-----")] // ������ ��� 
    [SerializeField]
    int Reaper_SP_1_HP; // ù��° ������ ü��
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
            // HP�� 0�̻��� �� �۵�
            if (BossCurHP > 0)
            {
                #region Reaper 
                if (this.gameObject.name == "Reaper")
                {
                    // ���� ü���� 50�� ���� �۾�����
                    if (BossCurHP <= (BossMaxHP / 100) * Reaper_SP_1_HP && isReaper_SP_ATK_1 == false)
                    {
                        isReaper_SP_ATK_1 = true;
                    }
                    // ���� ü���� 50�� ���� �۾�����
                    else if (BossCurHP <= (BossMaxHP / 100) * Reaper_SP_2_HP && isReaper_SP_ATK_2 == false)
                    {
                        isReaper_SP_ATK_2 = true;
                    }
                    // ���� ü���� 50�� ���� �۾�����
                    else if (BossCurHP <= (BossMaxHP / 100) * Reaper_SP_3_HP && isReaper_SP_ATK_3 == false)
                    {
                        isReaper_SP_ATK_3 = true;
                    }
                }

                #endregion // ������ hp�� ���� ���� ���� 

                // ���� ü���� 50�� ���� �۾�����
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
            // ����� ��ü �ݰ�
            other.gameObject.SetActive(false);

            BossCurHP -= 1000;

            Boss_HP_Canvas.GetComponent<BossHP_UI_Ctrl>().BossCur_HP = BossCurHP;
            // this.GetComponent<Reaper_Controller>().CurHP = BossCurHP;
            Boss_HP_Canvas.GetComponent<BossHP_UI_Ctrl>().Refresh_BossHP();
        }
    }
}

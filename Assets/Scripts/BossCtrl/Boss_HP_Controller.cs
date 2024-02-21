using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_HP_Controller : MonoBehaviour
{
    public GameObject Boss_HP_Canvas;
    public int BossMaxHP;
    public int BossCurHP;

    public bool isAwakening;

    // Start is called before the first frame update
    void Start()
    {
        Boss_HP_Canvas.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            // HP가 0이상일 시 작동
            if (BossCurHP > 0)
            {
                // 보스 체력이 30퍼 보다 작아지면
                if (BossCurHP <= (BossMaxHP / 100) * 30 && isAwakening == false)
                {
                    isAwakening = true;
                }

                Attack weapon = other.GetComponent<Attack>();
                Debug.Log("Damage: " + weapon.damage);

                BossCurHP -= weapon.damage;

                Boss_HP_Canvas.GetComponent<BossHP_UI_Ctrl>().BossCur_HP = BossCurHP;
                // this.GetComponent<Reaper_Controller>().CurHP = BossCurHP;
                Boss_HP_Canvas.GetComponent<BossHP_UI_Ctrl>().Refresh_BossHP();
            }
        }
    }
}

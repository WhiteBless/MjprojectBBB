using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_HP_Controller : MonoBehaviour
{
    public GameObject Boss_HP_Canvas;
    public int BossMaxHP;
    public int BossCurHP;

    // Start is called before the first frame update
    void Start()
    {
        Boss_HP_Canvas.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            // HP 0이하면 작동 금지
            if (BossCurHP > 0)
            {
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

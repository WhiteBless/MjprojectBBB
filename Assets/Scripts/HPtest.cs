using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPtest : MonoBehaviour
{
    [Header("적 체력")]
    public int maxHealth;
    public int curHealth;

    public MeshRenderer[] meshs;

    public Slider[] healthBars; // 체력 바들을 참조하는 변수
    public int[] virtualHealths; // 가상 체력들을 저장하는 배열

    public BossSkillP bossSkillP; // BossSkillP 스크립트를 참조하는 변수
    public Canvas healthBarCanvas; // 체력 바의 부모 캔버스를 참조하는 변수

    // Start is called before the first frame update
    void Awake()
    {
        meshs = GetComponentsInChildren<MeshRenderer>();
        healthBarCanvas = healthBars[0].transform.parent.GetComponent<Canvas>();

        // 가상 체력들을 초기화
        virtualHealths = new int[healthBars.Length - 1];
        for (int i = 0; i < virtualHealths.Length; i++)
        {
            virtualHealths[i] = 8000;
            healthBars[i].maxValue = virtualHealths[i]; // 슬라이더의 최대 값을 가상 체력의 최대 값으로 설정
        }

        // 오리지널 체력 바의 최대 값을 maxHealth로 설정
        healthBars[healthBars.Length - 1].maxValue = maxHealth;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon"))
        {
            if (this.gameObject.CompareTag("Enemy"))
            {
                Attack weapon = other.GetComponent<Attack>();
                Debug.Log("Damage: " + weapon.damage);

                int remainingDamage = weapon.damage; // 남은 데미지량을 계산하기 위한 변수

                // 가상 체력 감소
                for (int i = 0; i < virtualHealths.Length; i++)
                {
                    if (virtualHealths[i] > 0)
                    {
                        int actualDamage = Mathf.Min(virtualHealths[i], remainingDamage); // 실제로 가상 체력에서 감소시킬 데미지 계산
                        virtualHealths[i] = Mathf.Max(virtualHealths[i] - actualDamage, 0);
                        remainingDamage = Mathf.Max(remainingDamage - actualDamage, 0); // 남은 데미지량 갱신

                        if (remainingDamage <= 0) // 모든 데미지를 적용했다면 루프 종료
                        {
                            break;
                        }
                    }
                }

                // 가상 체력이 모두 소진되었을 때만 체력 감소
                if (remainingDamage > 0)
                {
                    curHealth = Mathf.Max(curHealth - remainingDamage, 0);
                }

                // 체력이 감소한 후에 체력바를 갱신
                UpdateHealthBars();

                foreach (MeshRenderer mesh in meshs)
                    mesh.material.color = Color.red;

                StartCoroutine(ResetColorAfterDelay(0.5f));

                if (curHealth <= 0)
                {
                    curHealth = 0; // curHealth 값을 0으로 고정
                    bossSkillP.isDead = true; // isDead 값을 true로 설정
                }
            }
        }
    }

    IEnumerator ResetColorAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (curHealth > 0)
        {
            foreach (MeshRenderer mesh in meshs)
                mesh.material.color = Color.white;
        }
    }

    void UpdateHealthBars()
    {
        // 오리지널 체력 바 업데이트
        healthBars[healthBars.Length - 1].value = curHealth;

        // 가상 체력 바들 업데이트
        for (int i = 0; i < healthBars.Length - 1; i++)
        {
            healthBars[i].value = virtualHealths[i];
            if (virtualHealths[i] <= 0)
            {
                healthBars[i].gameObject.SetActive(false); // 가상 체력이 0이 되면 해당 체력 바 비활성화
            }
        }

        if (curHealth <= 0)
        {
            healthBarCanvas.gameObject.SetActive(false);
        }
    }
}
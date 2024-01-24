using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPtest : MonoBehaviour
{
    [Header("적 체력")]
    public int maxHealth;
    public int curHealth;

    public AudioSource deathSoundSource; // 보스 사망 사운드를 재생할 AudioSource 컴포넌트를 참조하는 변수

    public MeshRenderer[] meshs;

    public Slider[] healthBars; // 체력 바들을 참조하는 변수
    public int[] virtualHealths; // 가상 체력들을 저장하는 배열

    public BossSkillP bossSkillP; // BossSkillP 스크립트를 참조하는 변수
    public Canvas healthBarCanvas; // 체력 바의 부모 캔버스를 참조하는 변수

    public Animator bossAnimator; // 보스의 애니메이터를 참조하는 변수
    public GameObject deathPopup; // 사망 시 띄울 팝업을 참조하는 변수

    public bool isDead; // 보스가 죽었는지 여부를 나타내는 변수
    // Start is called before the first frame update
    void Awake()
    {
        meshs = GetComponentsInChildren<MeshRenderer>();
        healthBarCanvas = healthBars[healthBars.Length - 1].transform.parent.GetComponent<Canvas>();

        // 가상 체력들을 초기화
        virtualHealths = new int[healthBars.Length - 1];
        for (int i = 0; i < virtualHealths.Length; i++)
        {
            virtualHealths[i] = 1000;
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
                    
                    // TODO ## 보스 현재 체력 HP바에 전달하는 부분
                    this.GetComponent<BossLookAt>().bossHPCanvas.GetComponent<BossHP_Ctrl>().BossCur_HP = curHealth;
                }

                // 체력이 감소한 후에 체력바를 갱신
                UpdateHealthBars();

                foreach (MeshRenderer mesh in meshs)
                    mesh.material.color = Color.red;

                StartCoroutine(ResetColorAfterDelay(0.5f));

                if (curHealth <= 0)
                {
                    curHealth = 0; // curHealth 값을 0으로 고정
                    isDead = true; // isDead 값을 true로 설정

                    // 랜덤으로 사망 애니메이션 선택
                    int randomDieAnimation = Random.Range(1, 3); // 1 또는 2의 값이 랜덤으로 선택됩니다.
                    bossAnimator.SetTrigger("doDie" + randomDieAnimation); // 선택된 애니메이션을 재생합니다.

                    // 팝업을 활성화하고 애니메이션 후에 비활성화하는 코루틴을 시작합니다.
                    StartCoroutine(ShowPopupAndDisableBossAfterDelay(5f));
                }
            }
        }
    }

    IEnumerator ShowPopupAndDisableBossAfterDelay(float delay)
    {

        bossSkillP.isDead = true;
        yield return new WaitForSeconds(0.5f);

        // 보스가 죽었을 때 사운드 재생
        if (deathSoundSource != null)
        {
            deathSoundSource.Play();
        }

        deathPopup.SetActive(true);
        StartCoroutine(ExpandPopupWidth(3f, 650f)); // 팝업의 너비를 1초 동안 650으로 확장하는 코루틴을 시작합니다.

        yield return new WaitForSeconds(delay);

        // 팝업을 천천히 사라지게 만드는 코루틴을 시작합니다.
        StartCoroutine(FadeOutPopup(1f));

        // 체력바를 비활성화하는 경우
        GameObject healthBar = GameObject.Find("BossHealthO"); // 체력바의 이름을 사용하여 찾습니다.
        if (healthBar != null)
        {
            healthBar.SetActive(false); // 체력바를 비활성화합니다.
        }
        // 체력아이콘를 비활성화하는 경우
        GameObject healthIcon = GameObject.Find("hpicon"); // 체력바의 이름을 사용하여 찾습니다.
        if (healthIcon != null)
        {
            healthIcon.SetActive(false); // 체력바를 비활성화합니다.
        }


        yield return new WaitForSeconds(3f); // 체력바 캔버스가 완전히 사라진 후에 보스를 비활성화하기 위해 0.5초 기다립니다.

        // 보스를 비활성화합니다.
        this.gameObject.SetActive(false);
    }

    IEnumerator FadeOutPopup(float duration)
    {
        CanvasGroup canvasGroup = deathPopup.GetComponent<CanvasGroup>();
        float startTime = Time.time;

        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;
            canvasGroup.alpha = Mathf.Lerp(1, 0, t);
            yield return null;
        }

        canvasGroup.alpha = 0; // 팝업을 완전히 투명하게 만듭니다.

        yield return new WaitForSeconds(2f);
        deathPopup.SetActive(false); // 팝업을 비활성화합니다.
    }

    IEnumerator ExpandPopupWidth(float duration, float targetWidth)
    {
        RectTransform rectTransform = deathPopup.GetComponent<RectTransform>();
        float startWidth = rectTransform.rect.width;
        float startTime = Time.time;

        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;
            rectTransform.sizeDelta = new Vector2(Mathf.Lerp(startWidth, targetWidth, t), rectTransform.sizeDelta.y);
            yield return null;
        }

        rectTransform.sizeDelta = new Vector2(targetWidth, rectTransform.sizeDelta.y); // 팝업의 너비를 목표치로 고정합니다.
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

        if (isDead) return; // 보스가 죽었다면 이 메소드를 더 이상 실행하지 않습니다.

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
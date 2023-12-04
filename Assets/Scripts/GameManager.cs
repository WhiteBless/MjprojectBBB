using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject deathMenu; // 사망 메뉴 UI 오브젝트
    public RectTransform deathMenuRectTransform; // 사망 메뉴의 RectTransform
    private float currentWidth = 0f; // 현재 Width 값을 저장할 변수

    public AudioClip deathSound; // 사망 효과음
    public List<AudioSource> allAudioSources; // 모든 오디오 소스

    public int health;
    //public Player player;

    public Image[] UIhealth;

    public AudioSource hitAudioSource; // 피격음을 재생할 AudioSource
    public AudioSource deathAudioSource; // 사망음을 재생할 AudioSource

    public AudioClip hitSound; // 피격음

    public BossAnimator bossAnimator; // BossAnimator의 참조

    private void Awake()
    {
        deathMenu.SetActive(false);
        AudioSource[] sources = FindObjectsOfType<AudioSource>(); // 모든 오디오 소스를 찾아 배열에 추가
        allAudioSources = new List<AudioSource>(); // 리스트 초기화

        foreach (AudioSource src in sources) // 배열에 있는 오디오 소스를 순회
        {
            if (src != deathAudioSource && src != hitAudioSource) // 사망음과 피격음을 재생하는 오디오 소스가 아닌 경우
            {
                allAudioSources.Add(src); // 리스트에 추가
            }
        }
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1; // 타임스케일을 원래대로 복구
        SceneManager.LoadScene("Main"); // "MainMenu"라는 이름의 Scene을 로드
    }

    void Update()
    {
        if (deathMenu.activeSelf) // 사망 메뉴가 활성화된 경우
        {
            currentWidth = Mathf.Lerp(currentWidth, 650, Time.unscaledDeltaTime * 0.02f); // 선형보간 함수를 이용해 Width 값을 천천히 증가시킴
            deathMenuRectTransform.sizeDelta = new Vector2(currentWidth, deathMenuRectTransform.sizeDelta.y); // 새로 계산된 Width 값으로 사망 메뉴의 Width를 업데이트
        }
    }

    public void HealthDown()
    {
        if (health > 1)
        {
            health--;

            UIhealth[health].color = new Color(1, 0, 0, 0.01f);

            // 피격음 재생
            hitAudioSource.PlayOneShot(hitSound, 1.0f); // 피격음 재생 (볼륨은 1.0)
        }


        else
        {
            //bossAnimator.AttRadyState = false; // 플레이어가 사망하였으므로 AttReadyState를 false로 설정

            //All Health UI Off
            UIhealth[0].color = new Color(1, 0, 0, 0.01f);

            //Player Die Effect
            Time.timeScale = 0; // 타임스케일을 0으로 설정
            deathMenu.SetActive(true); // 사망 메뉴를 활성화

            AudioSource[] sources = FindObjectsOfType<AudioSource>(); // 모든 오디오 소스를 찾아 배열에 추가
            allAudioSources.Clear(); // 리스트를 비웁니다.

            foreach (AudioSource src in sources) // 배열에 있는 오디오 소스를 순회
            {
                if (src != deathAudioSource && src != hitAudioSource) // 사망음과 피격음을 재생하는 오디오 소스가 아닌 경우
                {
                    allAudioSources.Add(src); // 리스트에 추가
                }
            }

            foreach (AudioSource audioSource in allAudioSources) // 모든 오디오 소스를 순회
            {
                audioSource.volume = 0; // 오디오 소스의 볼륨을 0으로 설정
                audioSource.clip = null; // 오디오 소스의 클립을 비활성화
                audioSource.Stop();
            }

            GameObject pObject = GameObject.FindGameObjectWithTag("P"); // P태그를 가진 오브젝트를 찾습니다.
            if (pObject != null) // P태그를 가진 오브젝트가 존재하면
            {
                BoxCollider boxCollider = pObject.GetComponent<BoxCollider>(); // 해당 오브젝트의 BoxCollider 컴포넌트를 찾습니다.
                if (boxCollider != null) // BoxCollider 컴포넌트가 존재하면
                {
                    boxCollider.enabled = false; // 해당 컴포넌트를 비활성화합니다.
                }
            }

            deathAudioSource.PlayOneShot(deathSound, 1.0f); // 사망 효과음 재생

            //Result UI
            Debug.Log("죽었습니다!");
        }

    }
}

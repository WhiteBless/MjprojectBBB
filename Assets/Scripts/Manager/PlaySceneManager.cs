using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlaySceneManager : MonoBehaviour
{
    public GameObject deathMenu; // 사망 메뉴 UI 오브젝트
    public RectTransform deathMenuRectTransform; // 사망 메뉴의 RectTransform
    private float currentWidth = 0f; // 현재 Width 값을 저장할 변수

    // public AudioClip deathSound; // 사망 효과음
    // public List<AudioSource> allAudioSources; // 모든 오디오 소스

    public int health;
    //public Player player;

    public Image[] UIhealth;

    [SerializeField]
    Slider SFX_Slider;
    [SerializeField]
    Slider BGM_Slider;

    public TextMeshProUGUI[] keyCodeName;

    public GameObject keySettingImage;
    public GameObject keySettingFailImage;

    public Image[] floorLevelIMG; // 이미지 배열, 순서대로 활성화할 이미지들

    private int floorLevelIndex = 0;

    [Header("----Stage----")]
    public GameObject[] Stages;

    // public AudioSource hitAudioSource; // 피격음을 재생할 AudioSource
    // public AudioSource deathAudioSource; // 사망음을 재생할 AudioSource

    // public AudioClip hitSound; // 피격음

    public BossAnimator bossAnimator; // BossAnimator의 참조

    private void Awake()
    {
        deathMenu.SetActive(false);

        GameManager.GMInstance.Set_PlaySceneManager(this);
        //AudioSource[] sources = FindObjectsOfType<AudioSource>(); // 모든 오디오 소스를 찾아 배열에 추가
        //allAudioSources = new List<AudioSource>(); // 리스트 초기화

        //foreach (AudioSource src in sources) // 배열에 있는 오디오 소스를 순회
        //{
        //    if (src != deathAudioSource && src != hitAudioSource) // 사망음과 피격음을 재생하는 오디오 소스가 아닌 경우
        //    {
        //        allAudioSources.Add(src); // 리스트에 추가
        //    }
        //}
    }

    private void Start()
    {
        Init();

        for (int i = 0; i < keyCodeName.Length; i++)
        {
            keyCodeName[i].text = KeySetting.Keys[(KeyAction)i].ToString();
        }

        // 모든 이미지를 비활성화하고 첫 번째 이미지를 활성화
        foreach (Image img in floorLevelIMG)
        {
            img.gameObject.SetActive(false);
        }

        if (floorLevelIMG.Length > 0)
        {
            floorLevelIMG[0].gameObject.SetActive(true);
        }
    }

    public void ActivateNextImage()
    {
        if (floorLevelIndex < floorLevelIMG.Length - 1)
        {
            floorLevelIMG[floorLevelIndex].gameObject.SetActive(false);
            floorLevelIndex++;
            floorLevelIMG[floorLevelIndex].gameObject.SetActive(true);
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

        for (int i = 0; i < keyCodeName.Length; i++)
        {
            keyCodeName[i].text = KeySetting.Keys[(KeyAction)i].ToString();
        }

        if (keySettingImage.activeSelf)
        {
            // 키보드의 모든 키에 대해 확인합니다.
            foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
            {
                // 마우스 버튼이 아닌 경우에만 처리합니다.
                if (!IsMouseButton(keyCode) && Input.GetKeyDown(keyCode))
                {
                    // 게임 오브젝트를 비활성화합니다.
                    keySettingImage.SetActive(false);
                    break; // 키 입력이 감지되었으므로 루프를 종료합니다.
                }
            }
        }

        if (keySettingFailImage.activeSelf && Input.GetMouseButtonDown(0))
        {
            // 게임 오브젝트를 비활성화합니다.
            keySettingFailImage.SetActive(false);
        }
    }

    // 입력된 키가 마우스 버튼인지 확인합니다.
    private bool IsMouseButton(KeyCode keyCode)
    {
        return keyCode >= KeyCode.Mouse0 && keyCode <= KeyCode.Mouse6;
    }

    public void HealthDown()
    {
        if (health > 0)
        {
            health--;

            UIhealth[health].gameObject.SetActive(false);

            // 피격음 재생
            //GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.HitSound);
            // hitAudioSource.PlayOneShot(hitSound, 1.0f); // 피격음 재생 (볼륨은 1.0)
        }
    }

    public void HealthActivateAll()
    {
        foreach (Image img in UIhealth)
        {
            if (img != null)
            {
                img.gameObject.SetActive(true);
            }
        }
    }
    public void CharacterDie()
    {
        //bossAnimator.AttRadyState = false; // 플레이어가 사망하였으므로 AttReadyState를 false로 설정

        //All Health UI Off


        //Player Die Effect
        Time.timeScale = 0; // 타임스케일을 0으로 설정
        deathMenu.SetActive(true); // 사망 메뉴를 활성화

        GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.DeadSong);

        //AudioSource[] sources = FindObjectsOfType<AudioSource>(); // 모든 오디오 소스를 찾아 배열에 추가
        //allAudioSources.Clear(); // 리스트를 비웁니다.

        //foreach (AudioSource src in sources) // 배열에 있는 오디오 소스를 순회
        //{
        //    if (src != deathAudioSource && src != hitAudioSource) // 사망음과 피격음을 재생하는 오디오 소스가 아닌 경우
        //    {
        //        allAudioSources.Add(src); // 리스트에 추가
        //    }
        //}

        //foreach (AudioSource audioSource in allAudioSources) // 모든 오디오 소스를 순회
        //{
        //    audioSource.volume = 0; // 오디오 소스의 볼륨을 0으로 설정
        //    audioSource.clip = null; // 오디오 소스의 클립을 비활성화
        //    audioSource.Stop();
        //}

        //GameObject pObject = GameObject.FindGameObjectWithTag("P"); // P태그를 가진 오브젝트를 찾습니다.
        //if (pObject != null) // P태그를 가진 오브젝트가 존재하면
        //{
        //    BoxCollider boxCollider = pObject.GetComponent<BoxCollider>(); // 해당 오브젝트의 BoxCollider 컴포넌트를 찾습니다.
        //    if (boxCollider != null) // BoxCollider 컴포넌트가 존재하면
        //    {
        //        boxCollider.enabled = false; // 해당 컴포넌트를 비활성화합니다.
        //    }
        //}

        //deathAudioSource.PlayOneShot(deathSound, 1.0f); // 사망 효과음 재생

        //Result UI
        Debug.Log("죽었습니다!");
    }


    private void Init()
    {
        // 배경음 변경
        GameManager.GMInstance.SoundManagerRef.PlayBGM(SoundManager.BGM.Floor_1);

        // 사운드 관련 초기화
        for (int i = 0; i < GameManager.GMInstance.SoundManagerRef.SFXPlayers.Length; i++)
        {
            SFX_Slider.value = GameManager.GMInstance.SoundManagerRef.SFXPlayers[i].volume;
        }

        for (int i = 0; i < GameManager.GMInstance.SoundManagerRef.BGMPlayers.Length; i++)
        {
            BGM_Slider.value = GameManager.GMInstance.SoundManagerRef.BGMPlayers[i].volume;
        }
    }

    // TODO ## 로비화면 환경설정 사운드 조절 함수
    #region Sound BGM / SFX
    public void SetSFXVolume(float volume)
    {
        // 배열에 존재하는 이펙트 음들의 크기를 조절한다.
        for (int i = 0; i < GameManager.GMInstance.SoundManagerRef.SFXPlayers.Length; i++)
        {
            // 효과음 음소거 시 실행 취소
            //if (SFXToggle.GetComponent<Toggle>().isOn == false)
            //{
            //    return;
            //}

            GameManager.GMInstance.SoundManagerRef.SFXPlayers[i].volume = volume;
        }
    }

    public void SetBGMVolume(float volume)
    {
        // 배열안에 존재하는 배경음의 크기를 조절한다.
        for (int i = 0; i < GameManager.GMInstance.SoundManagerRef.BGMPlayers.Length; i++)
        {
            // 배경음 음소거 시 실행 취소
            //if (BGMToggle.GetComponent<Toggle>().isOn == false)
            //{
            //    return;
            //}

            GameManager.GMInstance.SoundManagerRef.BGMPlayers[i].volume = volume;
        }
    }

    #endregion

    public void OpenToConfig(GameObject obj)
    {
        obj.gameObject.SetActive(true);
    }
    public void CloseToConfig(GameObject obj)
    {
        obj.gameObject.SetActive(false);
    }

    public void OpenToKeySetting(GameObject obj)
    {
        obj.gameObject.SetActive(true);
    }
}

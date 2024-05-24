using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlaySceneManager : MonoBehaviour
{
    public GameObject deathMenu; // ��� �޴� UI ������Ʈ
    public RectTransform deathMenuRectTransform; // ��� �޴��� RectTransform
    private float currentWidth = 0f; // ���� Width ���� ������ ����

    // public AudioClip deathSound; // ��� ȿ����
    // public List<AudioSource> allAudioSources; // ��� ����� �ҽ�

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

    public Image[] floorLevelIMG; // �̹��� �迭, ������� Ȱ��ȭ�� �̹�����

    private int floorLevelIndex = 0;

    [Header("----Stage----")]
    public GameObject[] Stages;

    // public AudioSource hitAudioSource; // �ǰ����� ����� AudioSource
    // public AudioSource deathAudioSource; // ������� ����� AudioSource

    // public AudioClip hitSound; // �ǰ���

    public BossAnimator bossAnimator; // BossAnimator�� ����

    private void Awake()
    {
        deathMenu.SetActive(false);

        GameManager.GMInstance.Set_PlaySceneManager(this);
        //AudioSource[] sources = FindObjectsOfType<AudioSource>(); // ��� ����� �ҽ��� ã�� �迭�� �߰�
        //allAudioSources = new List<AudioSource>(); // ����Ʈ �ʱ�ȭ

        //foreach (AudioSource src in sources) // �迭�� �ִ� ����� �ҽ��� ��ȸ
        //{
        //    if (src != deathAudioSource && src != hitAudioSource) // ������� �ǰ����� ����ϴ� ����� �ҽ��� �ƴ� ���
        //    {
        //        allAudioSources.Add(src); // ����Ʈ�� �߰�
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

        // ��� �̹����� ��Ȱ��ȭ�ϰ� ù ��° �̹����� Ȱ��ȭ
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
        Time.timeScale = 1; // Ÿ�ӽ������� ������� ����
        SceneManager.LoadScene("Main"); // "MainMenu"��� �̸��� Scene�� �ε�
    }

    void Update()
    {
        if (deathMenu.activeSelf) // ��� �޴��� Ȱ��ȭ�� ���
        {
            currentWidth = Mathf.Lerp(currentWidth, 650, Time.unscaledDeltaTime * 0.02f); // �������� �Լ��� �̿��� Width ���� õõ�� ������Ŵ
            deathMenuRectTransform.sizeDelta = new Vector2(currentWidth, deathMenuRectTransform.sizeDelta.y); // ���� ���� Width ������ ��� �޴��� Width�� ������Ʈ
        }

        for (int i = 0; i < keyCodeName.Length; i++)
        {
            keyCodeName[i].text = KeySetting.Keys[(KeyAction)i].ToString();
        }

        if (keySettingImage.activeSelf)
        {
            // Ű������ ��� Ű�� ���� Ȯ���մϴ�.
            foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
            {
                // ���콺 ��ư�� �ƴ� ��쿡�� ó���մϴ�.
                if (!IsMouseButton(keyCode) && Input.GetKeyDown(keyCode))
                {
                    // ���� ������Ʈ�� ��Ȱ��ȭ�մϴ�.
                    keySettingImage.SetActive(false);
                    break; // Ű �Է��� �����Ǿ����Ƿ� ������ �����մϴ�.
                }
            }
        }

        if (keySettingFailImage.activeSelf && Input.GetMouseButtonDown(0))
        {
            // ���� ������Ʈ�� ��Ȱ��ȭ�մϴ�.
            keySettingFailImage.SetActive(false);
        }
    }

    // �Էµ� Ű�� ���콺 ��ư���� Ȯ���մϴ�.
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

            // �ǰ��� ���
            //GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.HitSound);
            // hitAudioSource.PlayOneShot(hitSound, 1.0f); // �ǰ��� ��� (������ 1.0)
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
        //bossAnimator.AttRadyState = false; // �÷��̾ ����Ͽ����Ƿ� AttReadyState�� false�� ����

        //All Health UI Off


        //Player Die Effect
        Time.timeScale = 0; // Ÿ�ӽ������� 0���� ����
        deathMenu.SetActive(true); // ��� �޴��� Ȱ��ȭ

        GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.DeadSong);

        //AudioSource[] sources = FindObjectsOfType<AudioSource>(); // ��� ����� �ҽ��� ã�� �迭�� �߰�
        //allAudioSources.Clear(); // ����Ʈ�� ���ϴ�.

        //foreach (AudioSource src in sources) // �迭�� �ִ� ����� �ҽ��� ��ȸ
        //{
        //    if (src != deathAudioSource && src != hitAudioSource) // ������� �ǰ����� ����ϴ� ����� �ҽ��� �ƴ� ���
        //    {
        //        allAudioSources.Add(src); // ����Ʈ�� �߰�
        //    }
        //}

        //foreach (AudioSource audioSource in allAudioSources) // ��� ����� �ҽ��� ��ȸ
        //{
        //    audioSource.volume = 0; // ����� �ҽ��� ������ 0���� ����
        //    audioSource.clip = null; // ����� �ҽ��� Ŭ���� ��Ȱ��ȭ
        //    audioSource.Stop();
        //}

        //GameObject pObject = GameObject.FindGameObjectWithTag("P"); // P�±׸� ���� ������Ʈ�� ã���ϴ�.
        //if (pObject != null) // P�±׸� ���� ������Ʈ�� �����ϸ�
        //{
        //    BoxCollider boxCollider = pObject.GetComponent<BoxCollider>(); // �ش� ������Ʈ�� BoxCollider ������Ʈ�� ã���ϴ�.
        //    if (boxCollider != null) // BoxCollider ������Ʈ�� �����ϸ�
        //    {
        //        boxCollider.enabled = false; // �ش� ������Ʈ�� ��Ȱ��ȭ�մϴ�.
        //    }
        //}

        //deathAudioSource.PlayOneShot(deathSound, 1.0f); // ��� ȿ���� ���

        //Result UI
        Debug.Log("�׾����ϴ�!");
    }


    private void Init()
    {
        // ����� ����
        GameManager.GMInstance.SoundManagerRef.PlayBGM(SoundManager.BGM.Floor_1);

        // ���� ���� �ʱ�ȭ
        for (int i = 0; i < GameManager.GMInstance.SoundManagerRef.SFXPlayers.Length; i++)
        {
            SFX_Slider.value = GameManager.GMInstance.SoundManagerRef.SFXPlayers[i].volume;
        }

        for (int i = 0; i < GameManager.GMInstance.SoundManagerRef.BGMPlayers.Length; i++)
        {
            BGM_Slider.value = GameManager.GMInstance.SoundManagerRef.BGMPlayers[i].volume;
        }
    }

    // TODO ## �κ�ȭ�� ȯ�漳�� ���� ���� �Լ�
    #region Sound BGM / SFX
    public void SetSFXVolume(float volume)
    {
        // �迭�� �����ϴ� ����Ʈ ������ ũ�⸦ �����Ѵ�.
        for (int i = 0; i < GameManager.GMInstance.SoundManagerRef.SFXPlayers.Length; i++)
        {
            // ȿ���� ���Ұ� �� ���� ���
            //if (SFXToggle.GetComponent<Toggle>().isOn == false)
            //{
            //    return;
            //}

            GameManager.GMInstance.SoundManagerRef.SFXPlayers[i].volume = volume;
        }
    }

    public void SetBGMVolume(float volume)
    {
        // �迭�ȿ� �����ϴ� ������� ũ�⸦ �����Ѵ�.
        for (int i = 0; i < GameManager.GMInstance.SoundManagerRef.BGMPlayers.Length; i++)
        {
            // ����� ���Ұ� �� ���� ���
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

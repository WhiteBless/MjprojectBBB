using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using UnityEngine.UI;


public class LobbyManager : MonoBehaviour
{
    PlayableDirector PD;

    [SerializeField]
    Slider SFX_Slider;
    [SerializeField]
    Slider BGM_Slider;
    public float SFX_Volume;

    public GameObject settingUI;
    public GameObject ExitPop; // ExitPop 이미지 오브젝트를 Inspector에서 연결합니다.
    public bool isPaused;
    public bool doSetUI;



    // Start is called before the first frame update
    void Start()
    {
        GameManager.GMInstance.SoundManagerRef.PlayBGM(SoundManager.BGM.Lobby);
        GameManager.GMInstance.cur_Scene = Define.Cur_Scene.MAIN;

        PD = GetComponent<PlayableDirector>();

        PD.Play();

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


        SFX_Volume = volume;
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // ESC키가 눌렸는지 확인
        {
            if (isPaused )
            {
                OnClickXb(); // ExitPop을 비활성화하여 팝업을 닫습니다.
            }
            else
            {
                OnClickExitOpBtn(); // ExitPop을 활성화
            }
        }
    }



    // Ob 버튼이 눌렸을 때의 동작을 정의한 함수입니다.
    public void OnClickOb()
    {
        Application.Quit(); // 게임을 종료합니다.
    }

    // Xb 버튼이 눌렸을 때의 동작을 정의한 함수입니다.
    public void OnClickXb()
    {
        ExitPop.SetActive(false); // ExitPop을 비활성화하여 팝업을 닫습니다.
        isPaused = false;
    }

    public void OnClickExitOpBtn()
    {
        if (!doSetUI)
        {
            ExitPop.SetActive(true); // ExitPop을 활성화
            isPaused = true;
        }
    }

    public void OpenSettingUI()
    {
        settingUI.SetActive(true);
        doSetUI = true;
    }

    public void CloseSettingUI()
    {
        settingUI.SetActive(false);
        doSetUI = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 씬 관리를 위한 네임스페이스
using UnityEngine.UI; // UI를 사용하기 위한 네임스페이스

public class InGameSetting : MonoBehaviour
{
    public GameObject settingsUI; // Setting이라는 태그를 가진 UI 오브젝트를 Inspector에서 드래그로 연결해줍니다.
    public Button mainSceneButton; // Main 씬으로 이동하기 위한 버튼. Inspector에서 연결해줍니다.
    public bool isPaused = false; // 게임이 일시 정지되었는지 확인하는 플래그

    public void Start()
    {
        settingsUI.SetActive(false); // 시작 시 설정 UI 비활성화
        //Time.timeScale = 1f; // 게임 시간을 1로 설정해 게임 시작

        //mainSceneButton.onClick.AddListener(GoToMainScene); // 버튼 클릭 이벤트에 리스너 추가
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // ESC키가 눌렸는지 확인
        {
            if (isPaused)
            {
                ResumeGame(); // 게임이 이미 일시 정지 상태라면 재개
            }
            else
            {
                PauseGame(); // 아니라면 일시 정지
            }
        }
    }

    public void PauseGame()
    {
        settingsUI.SetActive(true); // 설정 UI 활성화
        //Time.timeScale = 0f; // 게임 시간을 0으로 설정해 게임 일시 정지
        isPaused = true; // 일시 정지 플래그 설정
    }

    public void ResumeGame()
    {
        settingsUI.SetActive(false); // 설정 UI 비활성화
        //Time.timeScale = 1f; // 게임 시간을 1로 설정해 게임 재개
        isPaused = false; // 일시 정지 플래그 해제
    }

    public void GoToMainScene()
    {
        SceneManager.LoadScene("Main"); // Main 씬 로드
    }

    public void OnClickRePlay_Btn()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
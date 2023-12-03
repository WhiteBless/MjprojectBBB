using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitPopup : MonoBehaviour
{
    public GameObject ExitPop; // ExitPop 이미지 오브젝트를 Inspector에서 연결합니다.

    // Update 함수에서 'Esc' 키 입력을 체크합니다.
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitPop.SetActive(true); // 'Esc' 키가 눌리면 ExitPop을 활성화합니다.
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
    }
}
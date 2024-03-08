using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdatePopup : MonoBehaviour
{
    public GameObject updateUI; // Update 태그를 가진 UI 오브젝트를 Inspector에서 드래그로 연결해줍니다.
    public Button[] openUpdateButtons; // Update UI를 활성화하기 위한 버튼들. Inspector에서 연결해줍니다.
    public Button[] closeUpdateButtons; // Update UI를 비활성화하기 위한 X 버튼들. Inspector에서 연결해줍니다.

    void Start()
    {
        updateUI.SetActive(false); // 시작 시 Update UI 비활성화

        foreach (Button button in openUpdateButtons)
        {
            button.onClick.AddListener(OpenUpdateUI); // 각 버튼 클릭 이벤트에 리스너 추가
        }

        foreach (Button button in closeUpdateButtons)
        {
            button.onClick.AddListener(CloseUpdateUI); // 각 X 버튼 클릭 이벤트에 리스너 추가
        }
    }

    void OpenUpdateUI()
    {
        updateUI.SetActive(true); // Update UI 활성화
    }

    void CloseUpdateUI()
    {
        updateUI.SetActive(false); // Update UI 비활성화
    }
}
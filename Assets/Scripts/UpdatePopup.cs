using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatePopup : MonoBehaviour
{
    public GameObject Updatepopup; // ExitPop 이미지 오브젝트를 Inspector에서 연결합니다.
    public GameObject updateObject; // Update 오브젝트를 Inspector에서 연결합니다.

    // Update 함수에서 'Esc' 키 입력을 체크합니다.
    void Update()
    {

    }

    // Ob 버튼이 눌렸을 때의 동작을 정의한 함수입니다.

    public void OnClickUpdatePop()
    {
        if (updateObject != null) // updateObject가 존재하면
        {
            updateObject.SetActive(true); // 해당 오브젝트를 활성화합니다.
        }
    }
}
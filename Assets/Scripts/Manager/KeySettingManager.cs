using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeySettingManager : MonoBehaviour
{
    public TextMeshProUGUI[] keyCodeName;

    public GameObject keySettingImage;
    public GameObject keySettingFailImage;

    private void Start()
    {
        for (int i = 0; i < keyCodeName.Length; i++)
        {
            keyCodeName[i].text = KeySetting.Keys[(KeyAction)i].ToString();
        }
    }

    void Update()
    {
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

    public void OpenToKeySetting(GameObject obj)
    {
        obj.gameObject.SetActive(true);
    }

}

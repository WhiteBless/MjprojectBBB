using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayResolution : MonoBehaviour
{
    FullScreenMode screenMode;
    // 드랍다운 ui
    [SerializeField]
    Dropdown resolutionDropdown;
    // 해상도 리스트 Index
    [SerializeField]
    int DropboxIndex;
    [SerializeField]
    CanvasScaler CanvasScale;
    [SerializeField]
    Toggle FullScreenBtn;

    // 해상도 값 저장하기 위한 변수
    List<Resolution> resolutions = new List<Resolution>();
    [SerializeField]
    RawImage BackImage;

    // Start is called before the first frame update
    void Start()
    {
        // 초기화
        InitUI();
    }

    public void InitUI()
    {
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            if(Screen.resolutions[i].width >= 1200)
            {
                resolutions.Add(Screen.resolutions[i]);
            }
        }

        // 해상도 저장
        resolutions.AddRange(Screen.resolutions);
        resolutionDropdown.options.Clear();

        int optionIndex = 0;

        foreach(Resolution item in resolutions)
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            // text 저장
            option.text = item.width + " X " + item.height + " " + item.refreshRate + "hz";
            resolutionDropdown.options.Add(option);

            // 현재 적용 중인 해상도에 맞게 텍스트가 나올 수 있도록
            if (item.width == Screen.width && item.height == Screen.height)
            {
                // 현재 적용중인 해상도가 맞다면 value값을 바꿔 표시하도록
                resolutionDropdown.value = optionIndex;
                GameManager.GMInstance.DisplayWidth = Screen.width;
                GameManager.GMInstance.DisplayHeight = Screen.height;
            }

            // 
            optionIndex++;
        }

        // 재설정
        resolutionDropdown.RefreshShownValue();

        FullScreenBtn.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;
    }

    public void DropboxOptionChange(int _x)
    {
        DropboxIndex = _x;
    }

    public void On_FullScreenBtn(bool _isFull)
    {
        // 참이면 전체화면 아니면 창모드
        screenMode = _isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }

    public void ChangeDisplay()
    {
        GameManager.GMInstance.DisplayWidth = resolutions[DropboxIndex].width;
        GameManager.GMInstance.DisplayHeight = resolutions[DropboxIndex].height;

        // Get_SetResolution null이 아니라면
        if (GameManager.GMInstance.Get_SetResolution() != null)
        {
            GameManager.GMInstance.Get_SetResolution().ResolutionSet();
        }

        // 배경화면이 null이 아니라면
        if (BackImage != null)
        {
            BackImage.rectTransform.sizeDelta =
                new Vector2(GameManager.GMInstance.DisplayWidth, GameManager.GMInstance.DisplayHeight);
        }

        if (CanvasScale != null)
        {
            CanvasScale.referenceResolution =
                new Vector2(GameManager.GMInstance.DisplayWidth, GameManager.GMInstance.DisplayHeight);
        }

        Screen.SetResolution(resolutions[DropboxIndex].width,
          resolutions[DropboxIndex].height,
          screenMode);
    }
}

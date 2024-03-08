using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour
{
    public Button[] buttons; // 사운드를 재생할 버튼들
    public AudioSource audioSource; // 사운드를 재생할 AudioSource
    public AudioClip[] clickSounds; // 각 버튼에 대한 클릭 사운드들
    public AudioClip[] releaseSounds; // 각 버튼에 대한 릴리즈 사운드들

    private void Start()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int buttonIndex = i; // 버튼 인덱스를 저장하여 클로저로 사용

            // 버튼 PointerDown 이벤트에 해당 버튼의 사운드 재생 함수를 연결합니다.
            EventTrigger.Entry pointerDownEntry = new EventTrigger.Entry();
            pointerDownEntry.eventID = EventTriggerType.PointerDown;
            pointerDownEntry.callback.AddListener((eventData) => PlayClickSound(buttonIndex));
            buttons[i].gameObject.AddComponent<EventTrigger>().triggers.Add(pointerDownEntry);

            // 버튼 PointerUp 이벤트에 해당 버튼의 사운드 재생 함수를 연결합니다.
            EventTrigger.Entry pointerUpEntry = new EventTrigger.Entry();
            pointerUpEntry.eventID = EventTriggerType.PointerUp;
            pointerUpEntry.callback.AddListener((eventData) => PlayReleaseSound(buttonIndex));
            buttons[i].gameObject.GetComponent<EventTrigger>().triggers.Add(pointerUpEntry);
        }
    }

    // 해당 버튼 클릭 시 사운드를 재생하는 함수
    private void PlayClickSound(int buttonIndex)
    {
        if (buttonIndex < 0 || buttonIndex >= clickSounds.Length)
            return;

        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(clickSounds[buttonIndex]);
        }
    }

    // 해당 버튼 릴리즈 시 사운드를 재생하는 함수
    private void PlayReleaseSound(int buttonIndex)
    {
        if (buttonIndex < 0 || buttonIndex >= releaseSounds.Length)
            return;

        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(releaseSounds[buttonIndex]);
        }
    }
}
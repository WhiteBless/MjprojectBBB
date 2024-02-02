using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KeyAction { Skill1, Skill2, Skill3, Skill4, Dodge, KeyCount }

public static class KeySetting
{
    public static Dictionary<KeyAction, KeyCode> Keys = new Dictionary<KeyAction, KeyCode>();
}

public class KeyManager : MonoBehaviour
{
    KeyCode[] defaultKeys = new KeyCode[] { KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R, KeyCode.Space };

    private void Awake()
    {
        for (int i = 0; i < (int)KeyAction.KeyCount; i++)
        {
            if (!KeySetting.Keys.ContainsKey((KeyAction)i)) // 중복된 키가 아니라면 추가
            {
                KeySetting.Keys.Add((KeyAction)i, defaultKeys[i]);
            }
        }
    }

    private void OnGUI()
    {
        Event keyEvent = Event.current;

        if (keyEvent.isKey && key != -1)
        {
            KeyAction keyAction = (KeyAction)key;

            // 중복된 키가 없으면 추가
            if (!KeySetting.Keys.ContainsValue(keyEvent.keyCode))
            {
                KeySetting.Keys[keyAction] = keyEvent.keyCode;
            }
            else
            {
                Debug.LogWarning("이미 사용 중인 키입니다.");
            }

            key = -1;
        }
    }

    int key = -1;

    public void ChangeKey(int num)
    {
        key = num;
    }
}
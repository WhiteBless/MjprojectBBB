using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KeyAction { Skill1, Skill2, Skill3, Skill4, Dodge, KeyCount }

public static class KeySetting { public static Dictionary<KeyAction, KeyCode> Keys = new Dictionary<KeyAction, KeyCode>(); }
public class KeyManager : MonoBehaviour
{
    int key = -1;

    KeyCode[] defaultKeys = new KeyCode[] { KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R, KeyCode.Space };
    private void Awake()
    {
        for(int i= 0; i<(int)KeyAction.KeyCount; i++)
        {
            KeySetting.Keys.Add((KeyAction)i, defaultKeys[i]);
        } 
    }

    private void OnGUI()
    {
        Event keyEvent = Event.current;

        if(keyEvent.isKey)
        {
            KeySetting.Keys[(KeyAction) key] = keyEvent.keyCode;
            key = -1;
        }
    }

    public void ChangeKey(int num)
    {
        key = num;
    }
}

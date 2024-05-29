using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.GMInstance.SoundManagerRef.PlayBGM(SoundManager.BGM.Lobby);
        GameManager.GMInstance.cur_Scene = Define.Cur_Scene.MAIN;
    }
}

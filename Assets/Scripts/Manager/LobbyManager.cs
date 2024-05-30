using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class LobbyManager : MonoBehaviour
{
    PlayableDirector PD;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.GMInstance.SoundManagerRef.PlayBGM(SoundManager.BGM.Lobby);
        GameManager.GMInstance.cur_Scene = Define.Cur_Scene.MAIN;

        PD = GetComponent<PlayableDirector>();

        PD.Play();
    }
}

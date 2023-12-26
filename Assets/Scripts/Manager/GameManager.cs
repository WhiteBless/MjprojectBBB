using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //// ManagerRef
    //[SerializeField]
    //TitleSceneManager TitleSceneManagerRef;
    //public TitleSceneManager titleManagerRef { get { return TitleSceneManagerRef; } set { TitleSceneManagerRef = value; } }

    //[SerializeField]
    //LobbyManager LobbyManagerRef;
    //public LobbyManager lobbyManagerRef { get { return LobbyManagerRef; } set { LobbyManagerRef = value; } }

    // 싱글톤
    public static GameManager GMInstance;

    // Managers Reference
    public SoundManager SoundManagerRef;

    void Awake()
    {
        /** GMInstance는 이 클래스를 의미한다. */
        GMInstance = this;

        /** 화면이 바껴도 클래스 유지 */
        DontDestroyOnLoad(gameObject);
    }
}
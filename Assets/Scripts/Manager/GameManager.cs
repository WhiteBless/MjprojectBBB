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

    // �̱���
    public static GameManager GMInstance;

    [SerializeField]
    PlaySceneManager playscenemanagerRef;

    // Managers Reference
    public SoundManager SoundManagerRef;

    void Awake()
    {
        if (GMInstance == null)
        {
            GMInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    #region PlaySceneManager_Ref
    public PlaySceneManager Get_PlaySceneManager()
    {
        return playscenemanagerRef;
    }

    public void Set_PlaySceneManager(PlaySceneManager _ref)
    {
        playscenemanagerRef = _ref;
    }
    #endregion
}
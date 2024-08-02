using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class GameManager : MonoBehaviour
{
    public int DisplayWidth;
    public int DisplayHeight;
    //// ManagerRef
    //[SerializeField]
    //TitleSceneManager TitleSceneManagerRef;
    //public TitleSceneManager titleManagerRef { get { return TitleSceneManagerRef; } set { TitleSceneManagerRef = value; } }

    //[SerializeField]
    //LobbyManager LobbyManagerRef;
    //public LobbyManager lobbyManagerRef { get { return LobbyManagerRef; } set { LobbyManagerRef = value; } }

    // 싱글톤
    public static GameManager GMInstance;

    [SerializeField]
    PlaySceneManager playscenemanagerRef;
    [SerializeField]
    InGameSetting inGameSettingRef;
    [SerializeField]
    SetResolution SetResolutionRef;


    // Managers Reference
    public SoundManager SoundManagerRef;
    public CamShake CamShakeRef;

    // 현재 캐릭터 
    [SerializeField]
    Cur_Character Cur_Char;
    public Cur_Character cur_Char { get { return Cur_Char; } set { Cur_Char = value; } }

    // 현재 캐릭터 
    [SerializeField]
    Cur_Scene Cur_Scene;
    public Cur_Scene cur_Scene { get { return Cur_Scene; } set { Cur_Scene = value; } }

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

    #region inGameSetting_Ref
    public InGameSetting Get_InGameSetting()
    {
        return inGameSettingRef;
    }

    public void Set_InGameSetting(InGameSetting _ref)
    {
        inGameSettingRef = _ref;
    }
    #endregion

    #region SetResolution_Ref
    public SetResolution Get_SetResolution()
    {
        return SetResolutionRef;
    }

    public void Set_SetResolution(SetResolution _ref)
    {
        SetResolutionRef = _ref;
    }
    #endregion
}
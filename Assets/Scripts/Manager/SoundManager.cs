using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class SoundManager : MonoBehaviour
{
    /** 배경음악 관련 */
    [Header("---BGM---")]
    public AudioClip[] BGMClips;
    public AudioSource[] BGMPlayers;
    public int BGMChannels;
    int BGMChannelIndex;

    /** 효과음 관련 */
    [Header("---SFX---")]
    public AudioClip[] SFXClips;

    public AudioClip[] Assasin_SFXClips;
    public AudioClip[] Boss_1F_SFXClips;
    public AudioClip[] Boss_2F_SFXClips;
    public AudioClip[] Boss_3F_SFXClips;

    public int SFXChannels;
    public AudioSource[] SFXPlayers;
    int SFXChannelIndex;

    public bool bIsSFXOn;
    public bool bIsBGMOn;

    public enum BGM
    {
        Lobby,
        Floor_1,
        BOSS_1FLOOR,
        BOSS_2FLOOR,
        BOSS_3FLOOR,
        LOADING,
        CLEAR_SOUND,

    }

    #region Enum_SFX
    public enum SFX
    {
        HitSound,
        DeadSong,
        DoorOpening,
        DoorClosing,
        
        PORTAL,

        BOSS_HIT_1,
        BOSS_HIT_2,

        CLEAR_SOUND,
        END,
    }

    public enum Boss_1F_SFX
    {
        RAZER,
        JUMP_GROUND_ATK,
        COMBO_9_ATK,
        SWORD_SPIN,
        THROW_SWORDSPIRIT,
    }

    public enum Boss_2F_SFX
    {
        BASE_ATK_SFX,
        DARK_DECLINE_SFX,
        DARK_BALL_THROW_SFX,
        DARK_SOUL_SFX,

    }

    public enum Boss_3F_SFX
    {
        FORMCHANGE,         // 0
        BARRIER,            // 1
        NORMAL_ATK,         // 2

        // NORMAL SFX
        LEAF_BREATH_START,  // 3
        LEAF_BREATH,        // 4
        LEAF_MISSALE,       // 5
        LEAF_PLACE,         // 6
        LEAF_TURN,          // 7

        // SPEED SFX
        SPIRIT,             // 8
        SPIRIT_THROW,       // 9
        CLAP,               // 10
        CLAP_SHOCK,         // 11
        DASH,               // 12
        DASH_END,           // 13
        TURNWHEEL,          // 14
        TURNWHEEL_END,      // 15

        // POWER SFX
        GROUND_WAVE,        // 16
        GROUND_WAVE_START,  // 17
        GROUND_EXPLOSIN,    // 18
        SPAWNSTONE,         // 19
        THROW_STONE,        // 20
    }


    public enum Assasin_SFX
    {
       // 스킬 사운드
       SWING_1, // 0
       SWING_2, // 1
       SWING_3, // 2
       R_Sound, // 3

       // 보이스 사운드
       ASSASIN_VOICE_1, // 4
       ASSASIN_VOICE_2, // 5
       ASSASIN_VOICE_3, // 6
       ASSASIN_VOICE_4, // 7

       // 수리검 투척 사운드
       THROW_KNIFE,
    }

    #endregion

    #region Init
    void Awake()
    {
        Init();
    }

    void Init()
    {
        /** 배경음 플레이어 초기화 */
        GameObject BGMObject = new GameObject("BGMPlayer");
        /** BGMObject의 부모클래스 이 스크립트를 가진 오브젝트로 한다. */
        BGMObject.transform.parent = transform;
        /** 채널의 개수만큼 배경음 재생기 생성 */
        BGMPlayers = new AudioSource[BGMChannels];

        bIsBGMOn = true;

        for (int i = 0; i < BGMPlayers.Length; i++)
        {
            /** BGMPlayer는 BGMObject에 추가한 AudioSource를 가져온다. */
            BGMPlayers[i] = BGMObject.AddComponent<AudioSource>();
            /** 배경음 재생 무한 반복 */
            BGMPlayers[i].loop = true;
            /** 배경음 플레이 */
            BGMPlayers[i].clip = BGMClips[0];
            BGMPlayers[i].volume = 0.5f;
        }

        /** 효과음 플레이어 초기화 */
        GameObject SFXObject = new GameObject("SFXPlayer");
        /** SFXObject의 부모클래스 이 스크립트를 가진 오브젝트로 한다. */
        SFXObject.transform.parent = transform;
        /** 채널의 개수만큼 효과음 재생기 생성 */
        SFXPlayers = new AudioSource[SFXChannels];
        bIsSFXOn = true;


        /** 저장된 효과음 개수만큼 반복 */
        for (int i = 0; i < SFXPlayers.Length; i++)
        {
            SFXPlayers[i] = SFXObject.AddComponent<AudioSource>();
            /** 초기 재생 off */
            SFXPlayers[i].playOnAwake = false;
            SFXPlayers[i].volume = 0.5f;
        }

        // PlayBGM(BGM.Lobby);

    }
    #endregion

    #region SFX_Sound
    /** TODO ## SoundManager.cs 효과음 재생 관련 함수 */
    /** SFX를 매개변수로 받는 효과음 재생 함수 정의 */
    // 게임 UI, UX 관련 효과음 재생
    public void PlaySFX(SFX sfx)
    {
        /** 저장된 Length값만큼 반복 */
        for (int i = 0; i < SFXPlayers.Length; i++)
        {
            int LoopIndex = (i + SFXChannelIndex) % SFXPlayers.Length;

            /** 만약 지금 효과음이 실행중이면? */
            if (SFXPlayers[LoopIndex].isPlaying)
            {
                /** 다시 반복문 초기부터 실행 */
                continue;
            }

            /** ChanelIndex를 LoopIndex값으로 바꿔준다. */
            SFXChannelIndex = LoopIndex;
            /** SFXPlayers의 0번째 Clip은 SFX Enum의 순서를 가져온다. */
            SFXPlayers[LoopIndex].clip = SFXClips[(int)sfx];
            /** 재생 */
            SFXPlayers[LoopIndex].Play();
            break;
        }
    }

    // 캐릭터 어쌔신 클래스 관련 효과음 재생
    public void Play_Assasin_SFX(Assasin_SFX sfx)
    {
        /** 저장된 Length값만큼 반복 */
        for (int i = 0; i < SFXPlayers.Length; i++)
        {
            int LoopIndex = (i + SFXChannelIndex) % SFXPlayers.Length;

            /** 만약 지금 효과음이 실행중이면? */
            if (SFXPlayers[LoopIndex].isPlaying)
            {
                /** 다시 반복문 초기부터 실행 */
                continue;
            }

            /** ChanelIndex를 LoopIndex값으로 바꿔준다. */
            SFXChannelIndex = LoopIndex;
            /** SFXPlayers의 0번째 Clip은 SFX Enum의 순서를 가져온다. */
            SFXPlayers[LoopIndex].clip = Assasin_SFXClips[(int)sfx];
            /** 재생 */
            SFXPlayers[LoopIndex].Play();
            break;
        }
    }

    // 1층 보스 관련 효과음 재생
    public void Play_1FBoss_SFX(Boss_1F_SFX sfx)
    {
        /** 저장된 Length값만큼 반복 */
        for (int i = 0; i < SFXPlayers.Length; i++)
        {
            int LoopIndex = (i + SFXChannelIndex) % SFXPlayers.Length;

            /** 만약 지금 효과음이 실행중이면? */
            if (SFXPlayers[LoopIndex].isPlaying)
            {
                /** 다시 반복문 초기부터 실행 */
                continue;
            }

            /** ChanelIndex를 LoopIndex값으로 바꿔준다. */
            SFXChannelIndex = LoopIndex;
            /** SFXPlayers의 0번째 Clip은 SFX Enum의 순서를 가져온다. */
            SFXPlayers[LoopIndex].clip = Boss_1F_SFXClips[(int)sfx];
            /** 재생 */
            SFXPlayers[LoopIndex].Play();
            break;
        }
    }

    // 2층 보스 관련 효과음 재생
    public void Play_2FBoss_SFX(Boss_2F_SFX sfx)
    {
        /** 저장된 Length값만큼 반복 */
        for (int i = 0; i < SFXPlayers.Length; i++)
        {
            int LoopIndex = (i + SFXChannelIndex) % SFXPlayers.Length;

            /** 만약 지금 효과음이 실행중이면? */
            if (SFXPlayers[LoopIndex].isPlaying)
            {
                /** 다시 반복문 초기부터 실행 */
                continue;
            }

            /** ChanelIndex를 LoopIndex값으로 바꿔준다. */
            SFXChannelIndex = LoopIndex;
            /** SFXPlayers의 0번째 Clip은 SFX Enum의 순서를 가져온다. */
            SFXPlayers[LoopIndex].clip = Boss_2F_SFXClips[(int)sfx];
            /** 재생 */
            SFXPlayers[LoopIndex].Play();
            break;
        }
    }

    // 3층 보스 관련 효과음 재생
    public void Play_3FBoss_SFX(Boss_3F_SFX sfx)
    {
        /** 저장된 Length값만큼 반복 */
        for (int i = 0; i < SFXPlayers.Length; i++)
        {
            int LoopIndex = (i + SFXChannelIndex) % SFXPlayers.Length;

            /** 만약 지금 효과음이 실행중이면? */
            if (SFXPlayers[LoopIndex].isPlaying)
            {
                /** 다시 반복문 초기부터 실행 */
                continue;
            }

            /** ChanelIndex를 LoopIndex값으로 바꿔준다. */
            SFXChannelIndex = LoopIndex;
            /** SFXPlayers의 0번째 Clip은 SFX Enum의 순서를 가져온다. */
            SFXPlayers[LoopIndex].clip = Boss_3F_SFXClips[(int)sfx];
            /** 재생 */
            SFXPlayers[LoopIndex].Play();
            break;
        }
    }
    #endregion

    #region BGM_Sound
    /** TODO ## SoundManager.cs 배경음 재생 관련 함수 */
    public void PlayBGM(BGM bgm)
    {
        /** 저장된 Length값만큼 반복 */
        for (int i = 0; i < BGMPlayers.Length; i++)
        {
            int LoopIndex = (i + BGMChannelIndex) % BGMPlayers.Length;

            ///** 만약 지금 효과음이 실행중이면? */
            //if (BGMPlayers[LoopIndex].isPlaying)
            //{
            //    /** 다시 반복문 초기부터 실행 */
            //    continue;
            //}

            /** ChanelIndex를 LoopIndex값으로 바꿔준다. */
            BGMChannelIndex = LoopIndex;
            /** SFXPlayers의 0번째 Clip은 SFX Enum의 순서를 가져온다. */
            BGMPlayers[LoopIndex].clip = BGMClips[(int)bgm];
            /** 재생 */
            BGMPlayers[LoopIndex].Play();
            break;
        }
    }
    #endregion
}
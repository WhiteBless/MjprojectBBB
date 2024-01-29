using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; } // 인스턴스 추가

    /** 배경음악 관련 */
    [Header("---BGM---")]
    public AudioClip[] BGMClips;
    public AudioSource[] BGMPlayers;
    public int BGMChannels;
    int BGMChannelIndex;

    /** 효과음 관련 */
    [Header("---SFX---")]
    public AudioClip[] SFXClips;
    public int SFXChannels;
    public AudioSource[] SFXPlayers;
    int SFXChannelIndex;

    public bool bIsSFXOn;
    public bool bIsBGMOn;

    public enum BGM
    {
        Lobby,
        Floor_1,
    }

    public enum SFX
    {
        HitSound,
        DeadSong,

    }


    void Awake()
    {
        Init();

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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

        PlayBGM(BGM.Lobby);

    }

    void Start()
    {
        GameManager.GMInstance.SoundManagerRef = this;
    }

    /** TODO ## SoundManager.cs 효과음 재생 관련 함수 */
    /** SFX를 매개변수로 받는 효과음 재생 함수 정의 */
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

    public void StopBGM(AudioClip clipToStop)
    {
        // BGM 오디오 소스를 중단시킴
        foreach (AudioSource bgmSource in BGMPlayers)
        {
            if (bgmSource.clip == clipToStop)
            {
                bgmSource.Stop();
            }
        }
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class SoundManager : MonoBehaviour
{
    /** ������� ���� */
    [Header("---BGM---")]
    public AudioClip[] BGMClips;
    public AudioSource[] BGMPlayers;
    public int BGMChannels;
    int BGMChannelIndex;

    /** ȿ���� ���� */
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
        BOSS_1FLOOR,
    }

    public enum SFX
    {
        HitSound,
        DeadSong,
        DoorOpening,
        DoorClosing,

    }


    void Awake()
    {
        Init();
    }

    void Init()
    {
        /** ����� �÷��̾� �ʱ�ȭ */
        GameObject BGMObject = new GameObject("BGMPlayer");
        /** BGMObject�� �θ�Ŭ���� �� ��ũ��Ʈ�� ���� ������Ʈ�� �Ѵ�. */
        BGMObject.transform.parent = transform;
        /** ä���� ������ŭ ����� ����� ���� */
        BGMPlayers = new AudioSource[BGMChannels];

        bIsBGMOn = true;

        for (int i = 0; i < BGMPlayers.Length; i++)
        {
            /** BGMPlayer�� BGMObject�� �߰��� AudioSource�� �����´�. */
            BGMPlayers[i] = BGMObject.AddComponent<AudioSource>();
            /** ����� ��� ���� �ݺ� */
            BGMPlayers[i].loop = true;
            /** ����� �÷��� */
            BGMPlayers[i].clip = BGMClips[0];
            BGMPlayers[i].volume = 0.5f;
        }

        /** ȿ���� �÷��̾� �ʱ�ȭ */
        GameObject SFXObject = new GameObject("SFXPlayer");
        /** SFXObject�� �θ�Ŭ���� �� ��ũ��Ʈ�� ���� ������Ʈ�� �Ѵ�. */
        SFXObject.transform.parent = transform;
        /** ä���� ������ŭ ȿ���� ����� ���� */
        SFXPlayers = new AudioSource[SFXChannels];
        bIsSFXOn = true;


        /** ����� ȿ���� ������ŭ �ݺ� */
        for (int i = 0; i < SFXPlayers.Length; i++)
        {
            SFXPlayers[i] = SFXObject.AddComponent<AudioSource>();
            /** �ʱ� ��� off */
            SFXPlayers[i].playOnAwake = false;
            SFXPlayers[i].volume = 0.5f;
        }

        PlayBGM(BGM.Lobby);

    }

    void Start()
    {
        GameManager.GMInstance.SoundManagerRef = this;
    }

    /** TODO ## SoundManager.cs ȿ���� ��� ���� �Լ� */
    /** SFX�� �Ű������� �޴� ȿ���� ��� �Լ� ���� */
    public void PlaySFX(SFX sfx)
    {
        /** ����� Length����ŭ �ݺ� */
        for (int i = 0; i < SFXPlayers.Length; i++)
        {
            int LoopIndex = (i + SFXChannelIndex) % SFXPlayers.Length;

            /** ���� ���� ȿ������ �������̸�? */
            if (SFXPlayers[LoopIndex].isPlaying)
            {
                /** �ٽ� �ݺ��� �ʱ���� ���� */
                continue;
            }

            /** ChanelIndex�� LoopIndex������ �ٲ��ش�. */
            SFXChannelIndex = LoopIndex;
            /** SFXPlayers�� 0��° Clip�� SFX Enum�� ������ �����´�. */
            SFXPlayers[LoopIndex].clip = SFXClips[(int)sfx];
            /** ��� */
            SFXPlayers[LoopIndex].Play();
            break;
        }
    }

    /** TODO ## SoundManager.cs ����� ��� ���� �Լ� */
    public void PlayBGM(BGM bgm)
    {
        /** ����� Length����ŭ �ݺ� */
        for (int i = 0; i < BGMPlayers.Length; i++)
        {
            int LoopIndex = (i + BGMChannelIndex) % BGMPlayers.Length;

            ///** ���� ���� ȿ������ �������̸�? */
            //if (BGMPlayers[LoopIndex].isPlaying)
            //{
            //    /** �ٽ� �ݺ��� �ʱ���� ���� */
            //    continue;
            //}

            /** ChanelIndex�� LoopIndex������ �ٲ��ش�. */
            BGMChannelIndex = LoopIndex;
            /** SFXPlayers�� 0��° Clip�� SFX Enum�� ������ �����´�. */
            BGMPlayers[LoopIndex].clip = BGMClips[(int)bgm];
            /** ��� */
            BGMPlayers[LoopIndex].Play();
            break;
        }
    }

}
using System;
using UnityEngine;

public enum BGMType
{
    None = -1,
    Title,
    Round1,
    Round2,
    Round3,
    Fail,
    Success
}

public enum SFXType
{
    None = -1,
    Start,
    Correct,
    Wrong,
    Drink,
    Bomb,
    Clap369,
    Drum,
    ClapClap,
    Cleopatra
}

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private BGMType currentBGMClip = BGMType.None;
    [SerializeField] private AudioClip[] bgmClips;
    public float BGMVolume
    {
        get => bgmSource.volume;
        set => bgmSource.volume = value;
    }

    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip[] sfxClips;
    public float SFXVolume
    {
        get => sfxSource.volume;
        set => sfxSource.volume = value;
    }

    
    public void PlayBGM(BGMType type)
    {
        if (type == BGMType.None) return; 
        
        bgmSource.clip = bgmClips[(int) type];
        bgmSource.loop = true;
        bgmSource.Play();

        currentBGMClip = type;
    }

    public void StopBGM()
    {
        bgmSource.Stop();

        currentBGMClip = BGMType.None;
    }

    public void PlaySFX(SFXType type)
    {
        if (type == SFXType.None) return; 

        sfxSource.clip = sfxClips[(int) type];
        sfxSource.loop = false;
        sfxSource.Play();
    }
    
}

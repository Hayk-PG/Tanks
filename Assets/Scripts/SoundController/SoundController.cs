using System;
using UnityEngine;

[Serializable] public struct SoundsList
{
    public AudioClip[] _clips;
}

public class SoundController : MonoBehaviour
{
    private static SoundController _inst;
    private AudioSource _musicSRC;
    private AudioSource _soundSRC;
    private Animator _musicAnimator;
    public enum MusicVolume { Down, Up}

    [SerializeField] private SoundsList[] _soundsList;


    private void Awake()
    {
        Instance();
        _musicSRC = Get<AudioSource>.From(transform.Find("MusicSRC").gameObject);
        _soundSRC = Get<AudioSource>.From(transform.Find("SoundSRC").gameObject);
        _musicAnimator = Get<Animator>.From(_musicSRC.gameObject);
    }

    private void Instance()
    {
        if (_inst != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _inst = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public static void MusicSRCCondition(bool isMuted)
    {
        _inst._musicSRC.mute = !isMuted;
    }

    public static void SoundSRCCondition(bool isMuted)
    {
        _inst._soundSRC.mute = !isMuted;
    }

    public static void PlaySound(int soundsListIndex, int clipIndex, out float clipLength)
    {
        clipLength = 0;

        if (soundsListIndex < _inst._soundsList.Length && clipIndex < _inst._soundsList[soundsListIndex]._clips.Length)
        {
            _inst._soundSRC.PlayOneShot(_inst._soundsList[soundsListIndex]._clips[clipIndex]);
            clipLength = _inst._soundsList[soundsListIndex]._clips[clipIndex].length;
        }            
    }

    public static void MusicSRCVolume(MusicVolume musicVolume)
    {
        switch (musicVolume)
        {
            case MusicVolume.Down: _inst._musicAnimator.Play("MusicSRCVolumeDownAnim");  break;
            case MusicVolume.Up: _inst._musicAnimator.Play("MusicSRCVolumeUpAnim"); break;
        }
    }
}

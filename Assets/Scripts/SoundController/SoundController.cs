using System;
using UnityEngine;

[Serializable] public struct Clips
{
    public AudioClip _clip;

    public string _clipName;

    public int _score;
}

[Serializable] public struct SoundsList
{
    [Space] [Header("Clips")]
    public Clips[] _clips;
}


public class SoundController : MonoBehaviour
{
    public enum MusicVolume { Down, Up}

    private static SoundController _inst;

    private MyScene _myScene;

    private AudioSource _musicSRC;
    private AudioSource _soundSRC;

    [SerializeField]
    private AudioSource[] _allAudioSources;

    private Animator _musicAnimator;

    [SerializeField]
    private SoundsList[] _soundsList;

    public SoundsList[] SoundsList
    {
        get => _soundsList;
    }

    public static bool IsMuted => _inst._soundSRC.mute;

    public static event Action<bool> onAudioSourceMute;



    private void Awake()
    {
        Instance();
        _myScene = FindObjectOfType<MyScene>();

        _musicSRC = Get<AudioSource>.From(transform.Find("MusicSRC").gameObject);

        _soundSRC = Get<AudioSource>.From(transform.Find("SoundSRC").gameObject);

        _musicAnimator = Get<Animator>.From(_musicSRC.gameObject);       
    }

    private void OnEnable()
    {
        _myScene.OnDestroyOnLoadMenuScene += OnDestroyOnLoadMenuScene;
    }

    private void OnDisable()
    {
        _myScene.OnDestroyOnLoadMenuScene -= OnDestroyOnLoadMenuScene;
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

    private void OnDestroyOnLoadMenuScene()
    {
        Destroy(gameObject);
    }

    public static void MusicSRCCondition(bool isMuted)
    {
        _inst._musicSRC.mute = !isMuted;
    }

    public static void SoundSRCCondition(bool isMuted)
    {
        _inst._soundSRC.mute = !isMuted;

        GlobalFunctions.Loop<AudioSource>.Foreach(_inst._allAudioSources, audioSource => { audioSource.mute = !isMuted; });

        onAudioSourceMute?.Invoke(_inst._soundSRC.mute);
    }

    public static void PlaySound(int soundsListIndex, int clipIndex, out float clipLength)
    {
        clipLength = 0;

        if (soundsListIndex < _inst._soundsList.Length && clipIndex < _inst._soundsList[soundsListIndex]._clips.Length)
        {
            _inst._soundSRC.PlayOneShot(_inst._soundsList[soundsListIndex]._clips[clipIndex]._clip);

            clipLength = _inst._soundsList[soundsListIndex]._clips[clipIndex]._clip.length;
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

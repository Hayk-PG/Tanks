using UnityEngine;
using UnityEngine.AddressableAssets;
using System;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]

//ADDRESSABLE
public class ExternalSoundSource : MonoBehaviour
{
    public enum PlayMode { OnAwake, OnEnable, OnStart, OnReference}

    [SerializeField] [Space]
    private AudioSource _audioSource;

    [SerializeField] [Space]
    private Animator _animator;

    [SerializeField] [Space]
    private AssetReferenceAudioClip _assetReferenceClip;

    [SerializeField] [Space]
    private PlayMode _playMode;

    [SerializeField] [Space]
    private bool _isDestroyable, _dontPlayAnimation;

    



    private void Awake()
    {
        _audioSource = Get<AudioSource>.From(gameObject);

        _animator = Get<Animator>.From(gameObject);

        _audioSource.mute = SoundController.IsSoundMuted;

        Play(PlayMode.OnAwake);
    }

    private void Start() => Play(PlayMode.OnStart);

    private void OnEnable()
    {
        Play(PlayMode.OnEnable);

        SoundController.onAudioSourceMute += OnMute;
    }

    private void OnDisable() => SoundController.onAudioSourceMute -= OnMute;

    private void OnDestroy() => ReleaseAsset();

    private void LoadClipAndPlay()
    {
        if (String.IsNullOrEmpty(_assetReferenceClip.AssetGUID))
        {
            SetClipAndPlay();

            print($"ExternalSoundSource is not using addressables: / {transform.parent?.name}");

            return;
        }

        if (_assetReferenceClip.IsValid())
            SetClipAndPlay((AudioClip)_assetReferenceClip.OperationHandle.Result);
        else
            _assetReferenceClip.LoadAssetAsync().Completed += asset => { SetClipAndPlay(asset.Result); };
    }

    private void SetClipAndPlay(AudioClip audioClip = null)
    {
        if (audioClip != null)
            _audioSource.clip = audioClip;

        _audioSource.Play();
    }

    private void ReleaseAsset()
    {
        if (String.IsNullOrEmpty(_assetReferenceClip.AssetGUID))
            return;

        if (_assetReferenceClip.IsValid())
            _assetReferenceClip.ReleaseAsset();

    }

    private void OnMute(bool isMuted) => _audioSource.mute = isMuted;

    public void Play(PlayMode playMode)
    {
        if (playMode == _playMode)
        {
            LoadClipAndPlay();

            PlayAnimation(true);
        }
    }

    public void Stop(bool unparent, bool stop = false)
    {
        PlayAnimation(false);

        if (unparent)
            transform.SetParent(null);

        if (stop)
            _audioSource.Stop();
    }

    public void OnAnimationEnd()
    {
        if (_isDestroyable)
            DestroyGameobject();
    }

    private void PlayAnimation(bool play)
    {
        if (_dontPlayAnimation)
            return;

        if (play)
            _animator.SetTrigger("play");
        else
            _animator.SetTrigger("stop");
    }

    private void DestroyGameobject()
    {
        ReleaseAsset();

        Destroy(gameObject);
    }
}

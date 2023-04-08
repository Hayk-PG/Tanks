using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
public class ExternalSoundSource : MonoBehaviour
{
    public enum PlayMode { OnAwake, OnEnable, OnStart}

    [SerializeField] 
    private PlayMode _playMode;

    [SerializeField] [Space]
    private bool _isDestroyable, _dontPlayAnimation;

    private AudioSource _audioSource;

    private Animator _animator;

    private SoundController _soundController;



    private void Awake()
    {
        _audioSource = Get<AudioSource>.From(gameObject);

        _animator = Get<Animator>.From(gameObject);

        _soundController = FindObjectOfType<SoundController>();

        Play(PlayMode.OnAwake);

        _audioSource.mute = SoundController.IsSoundMuted;
    }

    private void Start()
    {
        Play(PlayMode.OnStart);
    }

    private void OnEnable()
    {
        Play(PlayMode.OnEnable);

        SoundController.onAudioSourceMute += OnMute;
    }

    private void OnDisable()
    {
        SoundController.onAudioSourceMute -= OnMute;
    }

    private void OnMute(bool isMuted)
    {
        _audioSource.mute = isMuted;
    }

    public void Play(PlayMode playMode)
    {
        if(playMode == _playMode)
        {
            _audioSource.Play();

            PlayAnimation(true);
        }
    }

    public void Stop(bool unparent)
    {
        PlayAnimation(false);

        if (unparent)
            transform.SetParent(null);
    }

    public void OnAnimationEnd()
    {
        if (_isDestroyable)
            Destroy(gameObject);
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
}

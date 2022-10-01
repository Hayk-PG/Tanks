using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
public class ExternalSoundSource : MonoBehaviour
{
    public enum PlayMode { OnAwake, OnEnable, OnStart}

    [SerializeField] 
    private PlayMode _playMode;

    [SerializeField]
    private bool _isDestroyable;

    private AudioSource _audioSource;
    private Animator _animator;
    private SoundController _soundController;


    private void Awake()
    {
        _audioSource = Get<AudioSource>.From(gameObject);
        _animator = Get<Animator>.From(gameObject);
        _soundController = FindObjectOfType<SoundController>();

        Play(PlayMode.OnAwake);
    }

    private void Start()
    {
        Play(PlayMode.OnStart);
    }

    private void OnEnable()
    {
        Play(PlayMode.OnEnable);
    }

    public void Play(PlayMode playMode)
    {
        if(playMode == _playMode)
        {
            _animator.SetTrigger("play");
            _audioSource.Play();
        }
    }

    public void Stop(bool unparent)
    {
        _animator.SetTrigger("stop");

        if (unparent)
            transform.SetParent(null);
    }

    public void OnAnimationEnd()
    {
        if (_isDestroyable)
            Destroy(gameObject);
    }
}

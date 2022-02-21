using UnityEngine;

public class JavellinTypeBulletSoundController : BulletSoundsController
{
    private ILockedMissile _iLockedMissile;

    private bool _hasClipPlayed;


    private void Awake()
    {
        _iLockedMissile = Get<ILockedMissile>.From(gameObject);
    }

    private void OnEnable()
    {
        if (_iLockedMissile != null && _audioSrc != null) _iLockedMissile.OnTargetLocked += OnTargetLocked;
    }

    private void OnDisable()
    {
        if (_iLockedMissile != null && _audioSrc != null) _iLockedMissile.OnTargetLocked -= OnTargetLocked;
    }

    private void OnTargetLocked(float d)
    {
        ChangeClip(0);
        _audioSrc.pitch =  1 + Mathf.InverseLerp(5, 0, d * 5 * Time.fixedDeltaTime);
    }

    private void ChangeClip(int index)
    {
        if (!_hasClipPlayed)
        {
            _audioSrc.Stop();
            if(index < _clips.Length) _audioSrc.clip = _clips[index];
            _audioSrc.Play();
            _hasClipPlayed = true;
        }
    }
}

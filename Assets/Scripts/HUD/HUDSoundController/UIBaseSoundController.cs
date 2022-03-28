using UnityEngine;

public class UIBaseSoundController : MonoBehaviour
{
    [SerializeField]
    protected AudioSource _audioSRC;

    [SerializeField]
    protected AudioClip[] _audioClips;


    protected void PlaySoundFX(int clipIndex)
    {
        _audioSRC.PlayOneShot(_audioClips[clipIndex]);
    }
}

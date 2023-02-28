using System;
using UnityEngine;

public class SecondarySoundController : MonoBehaviour
{
    private static SecondarySoundController _inst;

    private AudioSource _audioSRC;

    [Serializable]
    private struct ClipsList
    {
        [SerializeField]
        private string _title;

        public AudioClip[] _clips;
    }

    [SerializeField]
    private ClipsList[] _clipsList;


    private void Awake()
    {
        _inst = this;

        _audioSRC = Get<AudioSource>.From(transform.Find("SoundSRC_2").gameObject);
    }

    public static void PlaySound(int listIndex, int clipIndex)
    {
        if (listIndex >= _inst._clipsList.Length || clipIndex >= _inst._clipsList[listIndex]._clips.Length)
            return;

        _inst._audioSRC.PlayOneShot(_inst._clipsList[listIndex]._clips[clipIndex]);
    }
}

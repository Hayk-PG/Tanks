using System;
using UnityEngine;

public class UISoundController : MonoBehaviour
{
    private static UISoundController _inst;
    private AudioSource _audioSRC;

    [Serializable] private struct ClipsList
    {
        [SerializeField]
        private string _title;

        public AudioClip[] _clips;
    }
    [SerializeField] private ClipsList[] _clipsList;


    private void Awake()
    {
        _inst = this;
        _audioSRC = Get<AudioSource>.From(transform.Find("SoundSRC_UI").gameObject);
    }

    public static void PlaySound(int listIndex, int clipIndex)
    {
        if (listIndex >= _inst._clipsList.Length || clipIndex >= _inst._clipsList[listIndex]._clips.Length)
            return;

        _inst._audioSRC.PlayOneShot(_inst._clipsList[listIndex]._clips[clipIndex]);
    }
}

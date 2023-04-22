﻿using System;
using UnityEngine;

public class SecondarySoundController : MonoBehaviour
{
    public static SecondarySoundController Instance { get; private set; }

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
        Instance = this;

        _audioSRC = Get<AudioSource>.From(transform.Find("SoundSRC_2").gameObject);
    }

    public static void PlaySound(int listIndex, int clipIndex)
    {
        if (Instance == null)
            Instance = FindObjectOfType<SecondarySoundController>();

        if (listIndex >= Instance._clipsList.Length || clipIndex >= Instance._clipsList[listIndex]._clips.Length)
            return;

        Instance._audioSRC.PlayOneShot(Instance._clipsList[listIndex]._clips[clipIndex]);
    }
}

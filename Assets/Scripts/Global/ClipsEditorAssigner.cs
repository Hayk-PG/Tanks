using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class ClipsEditorAssigner : MonoBehaviour
{
    [SerializeField]
    private SoundController _soundController;

    [SerializeField] [Space]
    private AudioClip[] _clips;

    [SerializeField] [Space]
    private List<Clips> _clipsList;

    [SerializeField] [Space]
    private string _trim;
    
    [SerializeField] [Space]
    private bool _execute;





    private void Update() => Execute();

    private void Execute()
    {
        bool canExecute = _execute && _soundController != null;

        if (!canExecute)
            return;

        _clipsList = new List<Clips>();

        for (int i = 0; i < _clips.Length; i++)
        {
            string clipName = _clips[i].name;

            int startIndex = _trim.Length;
            int length = clipName.Length - startIndex;

            string trimmedClipName = _clips[i].name.Substring(startIndex, length);

            if (trimmedClipName.Contains("_"))
                continue;

            _clipsList.Add(new Clips { _clip = _clips[i], _clipName = trimmedClipName });

            print($"{i}: {trimmedClipName}");
        }

        _soundController.SoundsList[_soundController.SoundsList.Length - 1]._clips = _clipsList.ToArray();

        _execute = false;
    }
}
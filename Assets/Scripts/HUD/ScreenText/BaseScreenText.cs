using System;
using UnityEngine;
using UnityEngine.UI;

public class BaseScreenText : MonoBehaviour
{
    protected Animator _animator;

    [SerializeField]
    protected Text _text;

    public Action OnSoundFX { get; set; }


    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public virtual void Display(string text)
    {
        _text.text = text;
        _animator.SetTrigger(Names.Play);
        OnSoundFX?.Invoke();
    }

    public virtual void Display(int value)
    {
        _animator.SetTrigger(Names.Play);
        OnSoundFX?.Invoke();
    }
}

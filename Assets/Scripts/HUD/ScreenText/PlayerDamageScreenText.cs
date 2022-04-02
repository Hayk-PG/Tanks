using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDamageScreenText : MonoBehaviour
{
    private Animator _animator;

    [SerializeField]
    private Text _damageText;

    public Action OnSoundFX { get; set; }


    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Display(int damage)
    {
        _damageText.text = damage.ToString();
        _animator.SetTrigger(Names.Play);
        OnSoundFX?.Invoke();
    }
}

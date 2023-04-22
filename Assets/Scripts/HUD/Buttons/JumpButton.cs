using System;
using UnityEngine;


public class JumpButton : MonoBehaviour
{
    [SerializeField] private Btn _btn;

    public event Action onJump;


    private void OnEnable() => _btn.onPointerUp += OnPointerUp;

    private void OnDisable() => _btn.onPointerUp -= OnPointerUp;

    private void OnPointerUp() => onJump?.Invoke();
}

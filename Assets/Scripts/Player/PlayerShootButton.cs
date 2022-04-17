using System;
using UnityEngine;

public class PlayerShootButton : MonoBehaviour
{
    private ShootButton _shootButton;

    public Action<bool> OnShootButtonPointer { get; set; }
    public Action<bool> OnShootButtonClick { get; set; }


    protected virtual void Awake()
    {
        _shootButton = FindObjectOfType<ShootButton>();
    }

    protected virtual void OnEnable()
    {
        _shootButton.OnPointer += OnPointer;
        _shootButton.OnClick += OnClick;
    }

    protected virtual void OnDisable()
    {
        _shootButton.OnPointer -= OnPointer;
        _shootButton.OnClick += OnClick;
    }

    private void OnClick(bool t) => OnShootButtonClick?.Invoke(t);
    private void OnPointer(bool t) => OnShootButtonPointer?.Invoke(t);
}

﻿using System;
using UnityEngine;

public class AmmoTypeController : MonoBehaviour
{
    private AmmoTabButton _ammoTabButton;
    private Animator _animator;
    private RectTransform _rectTransform;

    private const string _play = "play";
    private const string _direction = "speed";

    private float _animatorSpeed;

    public event Action<int> OnGetActiveAmmoType;


    private void Awake()
    {
        _ammoTabButton = FindObjectOfType<AmmoTabButton>();
        _animator = GetComponent<Animator>();
        _rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        _ammoTabButton.OnAmmoTabActivity += OnAmmoTabActivity;
    }

    private void OnDisable()
    {
        _ammoTabButton.OnAmmoTabActivity -= OnAmmoTabActivity;
    }

    public void OnAmmoTabActivity()
    {
        _animatorSpeed = _rectTransform.anchoredPosition.x > 0 ? 1 : -1;
        _animator.SetFloat(_direction, _animatorSpeed);
        _animator.SetTrigger(_play);
    }

    public void OnClickAmmoTypeButton(int bulletTypeIndex)
    {
        OnGetActiveAmmoType?.Invoke(bulletTypeIndex);
    }
}
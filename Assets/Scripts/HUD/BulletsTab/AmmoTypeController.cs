using System;
using UnityEngine;

public class AmmoTypeController : MonoBehaviour
{    
    [SerializeField]
    private Animator _animator;

    [SerializeField] [Space]
    private RectTransform _rectTransform;

    [SerializeField] [Space]
    private AmmoTabCustomization _ammoTabCustomization;

    [SerializeField] [Space]
    private AmmoTabButton _ammoTabButton;

    [SerializeField] [Space]
    private BaseRemoteControlTarget _remoteControl;

    [SerializeField] [Space]
    private CameraBlur _cameraBlur;

    private const string _play = "play";
    private const string _direction = "speed";
    private float _animatorSpeed;

    public bool IsOpen => _rectTransform.anchoredPosition.x > 0;

    public Action<bool> OnInformAboutTabActivityToTabsCustomization { get; set; }





    private void OnEnable()
    {
        _ammoTabButton.OnAmmoTabActivity += OnAmmoTabActivity;

        _ammoTabCustomization.OnAmmoTypeController += OnAmmoTabActivity;

        _remoteControl.onRemoteControlActivity += isActive =>
        {
            if (isActive && IsOpen)
                OnAmmoTabActivity();
        };
    }

    private void OnDisable()
    {
        _ammoTabButton.OnAmmoTabActivity -= OnAmmoTabActivity;

        _ammoTabCustomization.OnAmmoTypeController -= OnAmmoTabActivity;

        _remoteControl.onRemoteControlActivity -= isActive =>
        {
            if (isActive && IsOpen)
                OnAmmoTabActivity();
        };
    }

    public void OnAmmoTabActivity()
    {
        _animatorSpeed = IsOpen ? 1 : -1;
        _animator.SetFloat(_direction, _animatorSpeed);
        _animator.SetTrigger(_play);

        OnInformAboutTabActivityToTabsCustomization?.Invoke(IsOpen);

        _cameraBlur.ScreenBlur(IsOpen);

        PlaySoundFx(IsOpen ? 0 : 1);
    }

    public void PlaySoundFx(int clipIndex)
    {
        UISoundController.PlaySound(1, clipIndex);
    }
}

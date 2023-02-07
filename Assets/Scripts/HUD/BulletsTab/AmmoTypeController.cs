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
    private SupportsTabCustomization _supportTabCustomization;

    [SerializeField] [Space]
    private PropsTabCustomization _propsTabCustomization;

    [SerializeField] [Space]
    private AmmoTabButton _ammoTabButton;

    [SerializeField] [Space]
    private CameraBlur _cameraBlur;

    private const string _play = "play";
    private const string _direction = "speed";
    private float _animatorSpeed;  
    
    public Action<bool> OnInformAboutTabActivityToTabsCustomization { get; set; }





    private void OnEnable()
    {
        _ammoTabButton.OnAmmoTabActivity += OnAmmoTabActivity;
        _ammoTabCustomization.OnAmmoTypeController += OnAmmoTabActivity;
        _propsTabCustomization.OnAmmoTypeController += OnAmmoTabActivity;
        _supportTabCustomization.OnAmmoTypeController += OnAmmoTabActivity;
    }

    private void OnDisable()
    {
        _ammoTabButton.OnAmmoTabActivity -= OnAmmoTabActivity;
        _ammoTabCustomization.OnAmmoTypeController -= OnAmmoTabActivity;
        _propsTabCustomization.OnAmmoTypeController -= OnAmmoTabActivity;
        _supportTabCustomization.OnAmmoTypeController -= OnAmmoTabActivity;
    }

    public void OnAmmoTabActivity()
    {
        _animatorSpeed = _rectTransform.anchoredPosition.x > 0 ? 1 : -1;
        _animator.SetFloat(_direction, _animatorSpeed);
        _animator.SetTrigger(_play);

        OnInformAboutTabActivityToTabsCustomization?.Invoke(_rectTransform.anchoredPosition.x > 0);
        _cameraBlur.ScreenBlur(_rectTransform.anchoredPosition.x > 0);
        PlaySoundFx(_rectTransform.anchoredPosition.x > 0 ? 0 : 1);
    }

    public void PlaySoundFx(int clipIndex)
    {
        UISoundController.PlaySound(1, clipIndex);
    }
}

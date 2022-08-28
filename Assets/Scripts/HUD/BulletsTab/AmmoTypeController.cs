using System;
using UnityEngine;

public class AmmoTypeController : MonoBehaviour
{    
    private Animator _animator;
    private RectTransform _rectTransform;
    private AmmoTabCustomization _ammoTabCustomization;
    private SupportsTabCustomization _supportTabCustomization;
    private PropsTabCustomization _propsTabCustomization;
    private AmmoTabButton _ammoTabButton;
    private CameraBlur _cameraBlur;
    private const string _play = "play";
    private const string _direction = "speed";
    private float _animatorSpeed;   
    public Action<bool> OnInformAboutTabActivityToTabsCustomization { get; set; }


    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rectTransform = GetComponent<RectTransform>();
        _ammoTabCustomization = Get<AmmoTabCustomization>.From(gameObject);
        _supportTabCustomization = Get<SupportsTabCustomization>.From(gameObject);
        _propsTabCustomization = Get<PropsTabCustomization>.From(gameObject);
        _ammoTabButton = FindObjectOfType<AmmoTabButton>();
        _cameraBlur = FindObjectOfType<CameraBlur>();
    }

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

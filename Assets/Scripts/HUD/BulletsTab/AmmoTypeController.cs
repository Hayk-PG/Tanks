using System;
using UnityEngine;

public class AmmoTypeController : MonoBehaviour, IHudTabsObserver
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
    private CameraBlur _cameraBlur;

    private const string _play = "play";
    private const string _direction = "speed";

    private float _animatorSpeed;

    public bool WasHidden => _rectTransform.anchoredPosition.x > 0;

    public Action<bool> OnInformAboutTabActivityToTabsCustomization { get; set; }





    private void OnEnable()
    {
        _ammoTabButton.OnAmmoTabActivity += OnAmmoTabActivity;

        _ammoTabCustomization.OnAmmoTypeController += OnAmmoTabActivity;

        GameSceneObjectsReferences.HudTabsHandler.onRequestTabActivityPermission += (IHudTabsObserver observer, HudTabsHandler.HudTab currentActiveTab, HudTabsHandler.HudTab requestedTab, bool isActive) => 
        {
            if (requestedTab == HudTabsHandler.HudTab.TabRemoteControl && !WasHidden)
                OnAmmoTabActivity();
        };
    }

    private void OnDisable()
    {
        _ammoTabButton.OnAmmoTabActivity -= OnAmmoTabActivity;

        _ammoTabCustomization.OnAmmoTypeController -= OnAmmoTabActivity;
    }

    public void OnAmmoTabActivity() => GameSceneObjectsReferences.HudTabsHandler.RequestTabActivityPermission(this, HudTabsHandler.HudTab.AmmoTypeController, WasHidden);

    public void PlaySoundFx(int clipIndex) => UISoundController.PlaySound(1, clipIndex);

    public void Execute(bool isActive)
    {
        _animatorSpeed = WasHidden ? 1 : -1;
        _animator.SetFloat(_direction, _animatorSpeed);
        _animator.SetTrigger(_play);

        _cameraBlur.ScreenBlur(WasHidden);

        PlaySoundFx(WasHidden ? 0 : 1);

        OnInformAboutTabActivityToTabsCustomization?.Invoke(WasHidden);
    }
}
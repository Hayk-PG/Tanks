using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tab_DisplayAvaiableWeapons : MonoBehaviour
{
    internal struct ChachedAmmoTypeButton
    {
        internal AmmoTypeButton _ammoTypeButton;
        internal Animator _animator;
        internal CanvasGroup _canvasGroup;
        internal RectTransform _rectTransform;
    }
    private ChachedAmmoTypeButton[] _chachedAmmoTypeButtons;

    [SerializeField]
    private AmmoTypeButton[] _buttons;
    [SerializeField]
    private Transform _layoutGroup;
    private Transform _thisTransform;
    private Vector3 _defaultPosition = new Vector3(0, -1000, 0);
    private const string _playTriggerName = "play";
    private const string _resetTriggerName = "reset";
    private CanvasGroup _canvasGroup;
    private AmmoTabButtonNotification _ammoTabButtonNotification;

    

    private void Awake()
    {
        _thisTransform = transform;
        _canvasGroup = Get<CanvasGroup>.From(gameObject);
        _ammoTabButtonNotification = FindObjectOfType<AmmoTabButtonNotification>();
        CacheAmmoTypeButtons();
    }

    private void OnEnable()
    {
        _ammoTabButtonNotification.OnDisplayAvailableWeapons += OnDisplayAvailableWeapons;
    }

    private void OnDisable()
    {
        _ammoTabButtonNotification.OnDisplayAvailableWeapons -= OnDisplayAvailableWeapons;
    }

    private void CacheAmmoTypeButtons()
    {
        _chachedAmmoTypeButtons = new ChachedAmmoTypeButton[_buttons.Length];

        for (int i = 0; i < _buttons.Length; i++)
        {
            _chachedAmmoTypeButtons[i]._ammoTypeButton = _buttons[i];
            _chachedAmmoTypeButtons[i]._animator = Get<Animator>.From(_buttons[i].gameObject);
            _chachedAmmoTypeButtons[i]._canvasGroup = Get<CanvasGroup>.From(_buttons[i].gameObject);
            _chachedAmmoTypeButtons[i]._rectTransform = Get<RectTransform>.From(_buttons[i].gameObject);
        }
    }

    private void LoopChachedTypeButtons(Action<ChachedAmmoTypeButton> cachedAmmoTypeButton)
    {
        GlobalFunctions.Loop<ChachedAmmoTypeButton>.Foreach(_chachedAmmoTypeButtons, cachedTypeButton => 
        {
            cachedAmmoTypeButton?.Invoke(cachedTypeButton);
        });
    }

    private void OnDisplayAvailableWeapons(List<AmmoTypeButton> ammoTypeButton, bool isTabOpen)
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, isTabOpen);

        if (isTabOpen)
        {
            StartCoroutine(Coroutine(ammoTypeButton));
        }
    }

    private IEnumerator Coroutine(List<AmmoTypeButton> ammoTypeButton)
    {
        for (int i = 0; i < ammoTypeButton.Count; i++)
        {
            _chachedAmmoTypeButtons[i]._ammoTypeButton._ammoStars._ammoTypeStars = ammoTypeButton[i]._ammoStars._ammoTypeStars;
            _chachedAmmoTypeButtons[i]._ammoTypeButton._properties.Value = ammoTypeButton[i]._properties.Value;
            _chachedAmmoTypeButtons[i]._ammoTypeButton._properties.Icon = ammoTypeButton[i]._properties.Icon;
            _chachedAmmoTypeButtons[i]._ammoTypeButton._properties.RequiredScoreAmmount = ammoTypeButton[i]._properties.RequiredScoreAmmount;
            _chachedAmmoTypeButtons[i]._ammoTypeButton._properties.DamageValue = ammoTypeButton[i]._properties.DamageValue;
            _chachedAmmoTypeButtons[i]._ammoTypeButton._properties.Radius = ammoTypeButton[i]._properties.Radius;
            _chachedAmmoTypeButtons[i]._ammoTypeButton._properties.WeaponType = ammoTypeButton[i]._properties.WeaponType;
            _chachedAmmoTypeButtons[i]._animator.SetTrigger(_playTriggerName);
            UISoundController.PlaySound(5, 0);

            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(3);
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, false);
        LoopChachedTypeButtons(cachedAmmoTypeButton =>
        {
            cachedAmmoTypeButton._rectTransform.SetParent(_thisTransform);
            cachedAmmoTypeButton._rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            cachedAmmoTypeButton._rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            cachedAmmoTypeButton._rectTransform.anchoredPosition = _defaultPosition;
            cachedAmmoTypeButton._animator.SetTrigger(_resetTriggerName);
        });
    }
}

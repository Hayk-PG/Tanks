﻿using UnityEngine;

public class UpperTab : MonoBehaviour
{
    private GameManager _gameManager;
    private CanvasGroup _canvasGroup;
    private AmmoTypeController _ammoTypeController;
    private AmmoTabCustomization _ammoTabCustomization;

    private RectTransform _rectTransform;
    private float _distance;



    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _canvasGroup = GetComponent<CanvasGroup>();

        _ammoTypeController = FindObjectOfType<AmmoTypeController>();
        _ammoTabCustomization = FindObjectOfType<AmmoTabCustomization>();

        _rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        _distance = _ammoTabCustomization._container.GetComponent<RectTransform>().sizeDelta.x;
    }

    private void OnEnable()
    {
        _gameManager.OnGameStarted += OnGameStarted;
        _ammoTypeController.OnInformAboutTabActivityToTabsCustomization += OnWeaponTabActivity;
    }

    private void OnDisable()
    {
        _gameManager.OnGameStarted -= OnGameStarted;
        _ammoTypeController.OnInformAboutTabActivityToTabsCustomization -= OnWeaponTabActivity;
    }

    private void OnGameStarted()
    {
        if(_canvasGroup != null) GlobalFunctions.CanvasGroupActivity(_canvasGroup, true);
    }

    private void OnWeaponTabActivity(bool isOpen)
    {
        if (isOpen)
        {
            _rectTransform.offsetMin = Vector2.zero;
            _rectTransform.offsetMax = new Vector2(-_distance, 0);
        }
        else
        {
            _rectTransform.offsetMin = Vector2.zero;
            _rectTransform.offsetMax = Vector2.zero;
        }
    }
}
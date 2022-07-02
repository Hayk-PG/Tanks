﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Tab_EndgameTimer : MonoBehaviour
{
    [SerializeField] private Text _textTimer;
    private CanvasGroup _canvasGroup;
    private Tab_EndGame _tabEndGame;
    private int _s = 30;

    public Action OnTimerEnd { get; set; }

    private void Awake()
    {
        _canvasGroup = Get<CanvasGroup>.From(gameObject);
        _tabEndGame = FindObjectOfType<Tab_EndGame>();
    }

    private void OnEnable()
    {
        _tabEndGame.OnGameResultsFinished += StartTimerCoroutine;
    }

    private void OnDisable()
    {
        _tabEndGame.OnGameResultsFinished -= StartTimerCoroutine;
    }

    private void StartTimerCoroutine()
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, true);
        StartCoroutine(TimerCoroutine());
    }

    private IEnumerator TimerCoroutine()
    {
        while(_s > 0)
        {
            if (_s > 0) _s--;
            if (_s <= 0)
            {
                GlobalFunctions.CanvasGroupActivity(_canvasGroup, false);
                OnTimerEnd?.Invoke();
            }

            _textTimer.text = _s.ToString("D2");
            yield return new WaitForSeconds(1);
        }
    }
}
﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Tab_EndgameTimer : MonoBehaviour
{
    [SerializeField] private Text _textTimer;
    private CanvasGroup _canvasGroup;
    private GameResultProcessor _gameResultProcessor;
    private int _seconds;

    public Action<string> OnTimerEnd { get; set; }

    private void Awake()
    {
        _canvasGroup = Get<CanvasGroup>.From(gameObject);
        _gameResultProcessor = FindObjectOfType<GameResultProcessor>();
    }

    private void Start()
    {
        _seconds = MyPhotonNetwork.IsOfflineMode ? 5 : 30;
    }

    private void OnEnable()
    {
        _gameResultProcessor.onFinishResultProcess += StartTimerCoroutine;
    }

    private void OnDisable()
    {
        _gameResultProcessor.onFinishResultProcess -= StartTimerCoroutine;
    }

    private void StartTimerCoroutine()
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, true);
        StartCoroutine(TimerCoroutine());
    }

    private IEnumerator TimerCoroutine()
    {
        while(_seconds > 0)
        {
            if (_seconds > 0) _seconds--;
            if (_seconds <= 0)
            {
                GlobalFunctions.CanvasGroupActivity(_canvasGroup, false);
                OnTimerEnd?.Invoke(MyPhotonNetwork.IsOfflineMode ? "Restart": "Rematch");
            }

            _textTimer.text = _seconds.ToString("D2");
            yield return new WaitForSeconds(1);
        }
    }
}

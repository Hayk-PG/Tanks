using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;

public class BaseEndGame : MonoBehaviourPun
{
    protected GameManager _gameManager;
    protected MyPlugins _myPlugins;

    protected HealthController _healthTank1;
    protected HealthController _healthTank2;

    public Action<string, string> OnEndGameTab { get; set; }


    protected virtual void Awake()
    {
        _gameManager = Get<GameManager>.From(gameObject);
        _myPlugins = FindObjectOfType<MyPlugins>();      
    }

    protected virtual void OnEnable()
    {
        _gameManager.OnGameStarted += OnGameStarted;
    }

    protected virtual void OnDisable()
    {
        _gameManager.OnGameStarted -= OnGameStarted;
        UnsubscribeFromPluginService();
    }

    protected virtual void OnGameStarted()
    {
        _healthTank1 = _gameManager?.Tank1.GetComponent<HealthController>();
        _healthTank2 = _gameManager?.Tank2.GetComponent<HealthController>();

        if (PlatformChecker.IsEditor)
        {
            StartCoroutine(RunningGameEndChecker());
        }
        else
        {
            UnsubscribeFromPluginService();
            SubscribeToPluginService();
        }
    }

    protected IEnumerator RunningGameEndChecker()
    {
        while (!_gameManager.IsGameEnded)
        {
            GameEndChecker();
            yield return new WaitForSeconds(1);
        }
    }

    protected void UnsubscribeFromPluginService()
    {
        _myPlugins.OnPluginService -= OnPluginService;
    }

    protected void SubscribeToPluginService()
    {
        _myPlugins.OnPluginService += OnPluginService;
    } 

    protected virtual void OnPluginService()
    {
        GameEndChecker();
    }

    protected virtual void GameEndChecker()
    {
        if (TanksSet())
        {
            if (FirstPlayerWon())
                OnGameEnded(_healthTank1.name, _healthTank2.name);

            if (SecondPlayerWon())
                OnGameEnded(_healthTank2.name, _healthTank1.name);
        }
    }

    protected bool TanksSet()
    {
        return _healthTank1 != null && _healthTank2 != null;
    }

    protected bool FirstPlayerWon()
    {
        return _healthTank2.Health <= 0;
    }

    protected bool SecondPlayerWon()
    {
        return _healthTank1.Health <= 0;
    }

    protected virtual void OnGameEnded(string successedPlayerName, string defeatedPlayerName)
    {
        UnsubscribeFromPluginService();
        _gameManager.OnGameEnded?.Invoke();
        OnEndGameTab?.Invoke(successedPlayerName, defeatedPlayerName);
        _gameManager.IsGameEnded = true;
    }
}

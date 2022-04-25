using Photon.Pun;
using UnityEngine;

public class BaseEndGame : MonoBehaviourPun
{
    protected GameManager _gameManager;
    protected MyPlugins _myPlugins;

    protected HealthController _healthTank1;
    protected HealthController _healthTank2;


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

        UnsubscribeFromPluginService();
        SubscribeToPluginService();
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
        if(TanksSet())
        {
            if (FirstPlayerWon())
                OnGameEnded("Player 1 has won");

            if (SecondPlayerWon())
                OnGameEnded("Player 2 has won");
        }
    }

    protected bool TanksSet()
    {
        return _healthTank1 != null && _healthTank2 != null;
    }

    protected bool FirstPlayerWon()
    {
        return _healthTank2.Health <= 0 || _healthTank2.transform.position.y <= -5;
    }

    protected bool SecondPlayerWon()
    {
        return _healthTank1.Health <= 0 || _healthTank1.transform.position.y <= -5;
    }

    protected virtual void OnGameEnded(string playHasWon)
    {
        print(playHasWon);
        UnsubscribeFromPluginService();
    }
}

using System.Collections.Generic;
using UnityEngine;

public class LocalPlayerInitializeHimselfs : MonoBehaviour
{
    private PlayerTurn _playerTurn;
    private ScoreController _scoreController;
    private HealthController _healthController;

    private TurnController _turnController;
    private AmmoTabButtonNotification _ammoTabButtonNotification;
    private ScreenText _screenText;


    private void Awake()
    {
        _playerTurn = Get<PlayerTurn>.From(gameObject);
        _scoreController = Get<ScoreController>.From(gameObject);
        _healthController = Get<HealthController>.From(gameObject);

        _turnController = FindObjectOfType<TurnController>();
        _ammoTabButtonNotification = FindObjectOfType<AmmoTabButtonNotification>();
        _screenText = FindObjectOfType<ScreenText>();
    }

    private void OnEnable()
    {
        _turnController.OnPlayers += OnPlayersCached;
    }

    private void OnDisable()
    {
        _turnController.OnPlayers -= OnPlayersCached;
    }

    private void OnPlayersCached(List<PlayerTurn> players)
    {
        //PHOTON

        if(_playerTurn != null)
        {
            if (players.Find(playerTurn => playerTurn == _playerTurn))
            {
                if (_scoreController != null)
                {
                    _ammoTabButtonNotification.GetPlayerScoreAndSubscibeToEvent(_scoreController);
                    _screenText.GetPlayerHealthControllerAndSubscribeToEvent(_healthController);
                }    
            }
        }    
    }
}

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
    private TempPoints _tempPoints;


    private void Awake()
    {
        _playerTurn = Get<PlayerTurn>.From(gameObject);
        _scoreController = Get<ScoreController>.From(gameObject);
        _healthController = Get<HealthController>.From(gameObject);

        _turnController = FindObjectOfType<TurnController>();
        _ammoTabButtonNotification = FindObjectOfType<AmmoTabButtonNotification>();
        _screenText = FindObjectOfType<ScreenText>();
        _tempPoints = FindObjectOfType<TempPoints>();
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
                _ammoTabButtonNotification.CallPlayerEvents(_scoreController);
                _screenText.CallPlayerEvents(_healthController, _scoreController);
                _tempPoints.CallPlayerEvents(_scoreController);
            }
        }    
    }
}

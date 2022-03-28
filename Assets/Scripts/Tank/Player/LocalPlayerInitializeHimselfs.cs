using System.Collections.Generic;
using UnityEngine;

public class LocalPlayerInitializeHimselfs : MonoBehaviour
{
    private PlayerTurn _playerTurn;
    private ScoreController _scoreController;

    private TurnController _turnController;
    private AmmoTabButtonNotification _ammoTabButtonNotification;


    private void Awake()
    {
        _playerTurn = GetComponent<PlayerTurn>();
        _scoreController = GetComponent<ScoreController>();

        _turnController = FindObjectOfType<TurnController>();
        _ammoTabButtonNotification = FindObjectOfType<AmmoTabButtonNotification>();
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
                    _ammoTabButtonNotification.GetPlayerScoreAndSubscibeToEvent(_scoreController);
            }
        }    
    }
}

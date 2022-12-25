using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

public class PhotonPlayerStatsTracker : MonoBehaviour
{
    private GameManager _gameManager;
    private Tab_EndGame _tabEndGame;

    private bool _areStartDatasUpdated;
    private bool _areEndDatasUpdated;


    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _tabEndGame = FindObjectOfType<Tab_EndGame>();
    }

    private void OnEnable()
    {
        _gameManager.OnGameStarted += OnGameStart;
        _tabEndGame.onResult += OnGameEndResult;                
    }

    private void OnDisable()
    {
        _gameManager.OnGameStarted -= OnGameStart;
        _tabEndGame.onResult -= OnGameEndResult;
    }

    private void OnGameStart()
    {
        if(!_areStartDatasUpdated)
        {
            UpdateTimePlayed();
            _areStartDatasUpdated = true;
        }
    }

    private void OnGameEndResult(Tab_EndGame.Result result)
    {
        if (!_areEndDatasUpdated)
        {
            UpdateUserStats(result);
            _areEndDatasUpdated = true;
        }
    }

    private void UpdateTimePlayed()
    {
        Dictionary<string, int> timePlayed = new Dictionary<string, int> { { Keys.TimePlayed, 1 } };
        User.UpdateStats(Data.Manager.PlayfabId, timePlayed, null);
    }

    private void UpdateUserStats(Tab_EndGame.Result result)
    {
        Dictionary<string, int> stats = new Dictionary<string, int>
        {
            {Keys.Wins, result._win },
            {Keys.Losses, result._lose },
            {Keys.Points, result._points },
            {Keys.Level, result._level }
        };

        User.UpdateStats(Data.Manager.PlayfabId, stats, null);
    }
}

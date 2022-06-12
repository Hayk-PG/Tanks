using System;
using System.Collections;
using UnityEngine;

public class GetScoreFromTerOccInd : MonoBehaviour
{
    private PlayerTurn _playerTurn;
    private TankController _tankController;
    private MyPlugins _myPlugins;
    private TerritoryOccupiedIndicator _terrOccInd;

    [SerializeField] private int _sec;

    private bool IsPlayer1PercentHigh => _terrOccInd.Player1Percentage >= 50;
    private bool IsPlayer2PercentHigh => _terrOccInd.Player2Percentage >= 50;
    public Action OnGetScoreFromTerOccInd { get; set; }


    private void Awake()
    {
        _playerTurn = Get<PlayerTurn>.From(gameObject);
        _tankController = Get<TankController>.From(gameObject);
    }

    private void OnEnable()
    {
        _tankController.OnInitialize += OnInitialize;
    }

    private void OnDisable()
    {
        _tankController.OnInitialize -= OnInitialize;
        if (_myPlugins != null) _myPlugins.OnPluginService -= OnPluginService;
    }

    private void OnInitialize()
    {
        _terrOccInd = FindObjectOfType<TerritoryOccupiedIndicator>();
        
        if (PlatformChecker.IsEditor)
        {
            StartCoroutine(OnPluginServiceCoroutine());
        }
        else
        {
            _myPlugins = FindObjectOfType<MyPlugins>();
            _myPlugins.OnPluginService += OnPluginService;
        }
    }

    private void OnPluginService()
    {
        if (_playerTurn.MyTurn == TurnState.Player1) RunSeconds(IsPlayer1PercentHigh);
        if (_playerTurn.MyTurn == TurnState.Player2) RunSeconds(IsPlayer2PercentHigh);

        GetScore(_sec);
    }

    private void RunSeconds(bool isPercentHigh)
    {
        if (isPercentHigh)  _sec++;
        if (!isPercentHigh) _sec = 0;
    }

    private void GetScore(int sec)
    {
        if (sec >= 60)
        {
            OnGetScoreFromTerOccInd?.Invoke();
            _sec = 0;
        }
    }

    private IEnumerator OnPluginServiceCoroutine()
    {
        while (true)
        {
            OnPluginService();
            yield return new WaitForSeconds(1);
        }
    }
}

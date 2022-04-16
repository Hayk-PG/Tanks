using System.Collections.Generic;
using System;
using Photon.Pun;

public enum TurnState { None, Transition, Player1, Player2, Other}

public class TurnController : MonoBehaviourPun
{
    public TurnState _turnState;
    public TurnState _previousTurnState;

    private GameManager _gameManager;
    private CameraMovement _cameraMovement;

    private List<PlayerTurn> _playersTurn;

    public Action<TurnState,CameraMovement> OnTurnChanged { get; set; }

    /// <summary>
    /// 0: PlayerOne 1:PlayerTwo
    /// </summary>
    public static List<PlayerTurn> Players { get; set; } = new List<PlayerTurn>();
    public event Action<List<PlayerTurn>> OnPlayers;
    

    private void Awake()
    {
        _gameManager = Get<GameManager>.From(gameObject);
        _cameraMovement = FindObjectOfType<CameraMovement>();
    }

    private void OnEnable()
    {
        _gameManager.OnGameStarted += SetPlayersTurnOnGameStart;
    }

    private void OnDisable()
    {
        _gameManager.OnGameStarted -= SetPlayersTurnOnGameStart;
    }

    private void SetPlayersTurnOnGameStart()
    {
        SetNextTurn(TurnState.Player1);
    }

    public void SetNextTurn(TurnState turnState)
    {
        if (!MyPhotonNetwork.IsOfflineMode && photonView.IsMine)
            photonView.RPC("NextTurn", RpcTarget.AllViaServer, (int)turnState);
        else if (MyPhotonNetwork.IsOfflineMode)
            NextTurn((int)turnState);
    }

    [PunRPC]
    private void NextTurn(int turnStateIndex)
    {
        if ((TurnState)turnStateIndex == TurnState.Player1 || (TurnState)turnStateIndex == TurnState.Player2)
            _previousTurnState = _turnState;

        _turnState = (TurnState)turnStateIndex;
        Invoke("NextTurnFromTransition", 2);
        OnTurnChanged?.Invoke(_turnState, _cameraMovement);
    }

    private void NextTurnFromTransition()
    {
        if (_turnState == TurnState.Transition)
        {
            SetNextTurn(_previousTurnState == TurnState.Player1 ? TurnState.Player2 : TurnState.Player1);
        }
    }
}

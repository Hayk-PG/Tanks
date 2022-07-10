using UnityEngine;

public class EnemyPlayerIconController : MonoBehaviour
{
    private TankController _tankController;
    private PlayerTurn _playerTurn;
    private GameManager _gameManager;
    private EnemyPlayerIcon _enemyPlayerIcon;


    private void Awake()
    {
        _tankController = Get<TankController>.From(gameObject);
        _playerTurn = Get<PlayerTurn>.From(gameObject);
        _gameManager = FindObjectOfType<GameManager>();
        _enemyPlayerIcon = FindObjectOfType<EnemyPlayerIcon>();
    }

    private void OnEnable()
    {
        _gameManager.OnGameStarted += OnGameStarted;
    }
   
    private void OnDisable()
    {
        _gameManager.OnGameStarted -= OnGameStarted;
    }

    private void OnGameStarted()
    {
        if(_tankController.BasePlayer != null)
        {
            Transform enemyPlayer = _playerTurn.MyTurn == TurnState.Player1 ?
            GlobalFunctions.ObjectsOfType<PlayerTurn>.Find(player => player.MyTurn == TurnState.Player2).transform :
            GlobalFunctions.ObjectsOfType<PlayerTurn>.Find(player => player.MyTurn == TurnState.Player1).transform;

            _enemyPlayerIcon?.SetInitialCoordinates(enemyPlayer);
        }
    }
}

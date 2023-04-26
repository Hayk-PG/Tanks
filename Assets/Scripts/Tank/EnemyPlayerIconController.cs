using UnityEngine;

public class EnemyPlayerIconController : MonoBehaviour
{
    private TankController _tankController;

    private PlayerTurn _playerTurn;

    private GameManager _gameManager;




    private void Awake()
    {
        _tankController = Get<TankController>.From(gameObject);

        _playerTurn = Get<PlayerTurn>.From(gameObject);

        _gameManager = FindObjectOfType<GameManager>();
    }

    private void OnEnable() => _gameManager.OnGameStarted += OnGameStarted;

    private void OnDisable() => _gameManager.OnGameStarted -= OnGameStarted;

    private void OnGameStarted()
    {
        if(_tankController.BasePlayer != null)
        {
            Transform enemyPlayer = _playerTurn.MyTurn == TurnState.Player1 ?

            GlobalFunctions.ObjectsOfType<PlayerTurn>.Find(player => player.MyTurn == TurnState.Player2).transform :
            GlobalFunctions.ObjectsOfType<PlayerTurn>.Find(player => player.MyTurn == TurnState.Player1).transform;

            GameSceneObjectsReferences.EnemyPlayerIcon?.Init(enemyPlayer);
        }
    }
}

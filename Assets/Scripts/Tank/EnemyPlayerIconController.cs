using UnityEngine;

public class EnemyPlayerIconController : MonoBehaviour
{
    private TankController _tankController;
    private GameManager _gameManager;
    private EnemyPlayerIcon _enemyPlayerIcon;


    private void Awake()
    {
        _tankController = Get<TankController>.From(gameObject);
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
            Transform enemyPlayer = name == Names.Tank_FirstPlayer ? GameObject.Find(Names.Tank_SecondPlayer)?.transform :
                                    name == Names.Tank_SecondPlayer ? GameObject.Find(Names.Tank_FirstPlayer)?.transform : null;

            _enemyPlayerIcon?.SetInitialCoordinates(enemyPlayer);
        }
    }
}

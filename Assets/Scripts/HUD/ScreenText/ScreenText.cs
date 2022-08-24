using UnityEngine;

public class ScreenText : MonoBehaviour
{
    private GameManager _gameManager;
    private HealthController _playerHealthController;
    private ScoreController _scoreController;



    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

    private void OnEnable()
    {
        _gameManager.OnGameStarted += OnGameStarted;
    }

    private void OnDisable()
    {
        _gameManager.OnGameStarted -= OnGameStarted;

        if (_playerHealthController != null)
            _playerHealthController.OnTakeDamage -= OnTakeDamage;

        if (_scoreController != null)
            _scoreController.OnHitEnemy -= OnGetPoints;
    }

    private void OnGameStarted()
    {
        TankController localTank = GlobalFunctions.ObjectsOfType<TankController>.Find(tank => tank.BasePlayer != null);

        if(localTank != null)
        {
            _playerHealthController = Get<HealthController>.From(localTank.gameObject);
            _scoreController = Get<ScoreController>.From(localTank.gameObject);

            _playerHealthController.OnTakeDamage += OnTakeDamage;
            _scoreController.OnHitEnemy += OnGetPoints;
        }
    }

    private void OnTakeDamage(BasePlayer basePlayer, int damage)
    {
        SecondarySoundController.PlaySound(0, 0);
    }

    private void OnGetPoints(int[] scoreValues)
    {
        SecondarySoundController.PlaySound(0, 1);
    }
}

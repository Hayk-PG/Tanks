using UnityEngine;

public class LocalPlayerInitializeHimselfs : MonoBehaviour
{
    private TankController _tankController;
    private ScoreController _scoreController;
    private HealthController _healthController;
    private PlayerTurn _playerTurn;
    private TankMovement _tankMovement;

    private AmmoTabButtonNotification _ammoTabButtonNotification;
    private TempPoints _tempPoints;
    private Fuel _fuel;


    private void Awake()
    {
        _tankController = Get<TankController>.From(gameObject);
        _scoreController = Get<ScoreController>.From(gameObject);
        _healthController = Get<HealthController>.From(gameObject);
        _playerTurn = Get<PlayerTurn>.From(gameObject);
        _tankMovement = Get<TankMovement>.From(gameObject);

        _ammoTabButtonNotification = FindObjectOfType<AmmoTabButtonNotification>();
        _tempPoints = FindObjectOfType<TempPoints>();
        _fuel = FindObjectOfType<Fuel>(); 
    }

    private void OnEnable()
    {
        _tankController.OnInitialize += OnInitializeHimself;
    }

    private void OnDisable()
    {
        _tankController.OnInitialize -= OnInitializeHimself;
    }

    private void OnInitializeHimself()
    {
        _ammoTabButtonNotification.CallPlayerEvents(_scoreController);
        _tempPoints.CallPlayerEvents(_scoreController);
        _fuel.CallPlayerEvents(_tankMovement);
    }
}

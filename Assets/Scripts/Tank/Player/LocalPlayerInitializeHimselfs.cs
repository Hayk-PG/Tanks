using UnityEngine;

public class LocalPlayerInitializeHimselfs : MonoBehaviour
{
    private TankController _tankController;
    private ScoreController _scoreController;
    private TankMovement _tankMovement;

    private AmmoTabButtonNotification _ammoTabButtonNotification;
    private Fuel _fuel;


    private void Awake()
    {
        _tankController = Get<TankController>.From(gameObject);
        _scoreController = Get<ScoreController>.From(gameObject);
        _tankMovement = Get<TankMovement>.From(gameObject);

        _ammoTabButtonNotification = FindObjectOfType<AmmoTabButtonNotification>();
        _fuel = FindObjectOfType<Fuel>(); 
    }

    private void OnEnable() => _tankController.OnInitialize += OnInitializeHimself;

    private void OnDisable() => _tankController.OnInitialize -= OnInitializeHimself;

    private void OnInitializeHimself()
    {
        _ammoTabButtonNotification.CallPlayerEvents(_scoreController);
        _fuel.CallPlayerEvents(_tankMovement);
    }
}

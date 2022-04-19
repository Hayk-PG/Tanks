using System.Collections.Generic;
using UnityEngine;

public class LocalPlayerInitializeHimselfs : MonoBehaviour
{
    private TankController _tankController;
    private ScoreController _scoreController;
    private HealthController _healthController;

    private AmmoTabButtonNotification _ammoTabButtonNotification;
    private ScreenText _screenText;
    private TempPoints _tempPoints;


    private void Awake()
    {
        _tankController = Get<TankController>.From(gameObject);
        _scoreController = Get<ScoreController>.From(gameObject);
        _healthController = Get<HealthController>.From(gameObject);

        _ammoTabButtonNotification = FindObjectOfType<AmmoTabButtonNotification>();
        _screenText = FindObjectOfType<ScreenText>();
        _tempPoints = FindObjectOfType<TempPoints>();
    }

    private void OnEnable()
    {
        _tankController.OnInitializeHimself += OnInitializeHimself;
    }

    private void OnDisable()
    {
        _tankController.OnInitializeHimself -= OnInitializeHimself;
    }

    private void OnInitializeHimself()
    {
        _ammoTabButtonNotification.CallPlayerEvents(_scoreController);
        _screenText.CallPlayerEvents(_healthController, _scoreController);
        _tempPoints.CallPlayerEvents(_scoreController);
    }
}

using System.Collections;
using UnityEngine;

public class AIShields : PlayerShields
{
    [SerializeField] private PropsProperties _shieldProperties;
    private ScoreController _scoreController;
    private HealthController _healthController;
    private int _seconds;


    protected override void Awake()
    {
        _playerTurn = Get<PlayerTurn>.From(gameObject);
        _scoreController = Get<ScoreController>.From(gameObject);
        _healthController = Get<HealthController>.From(gameObject);
        _turnController = FindObjectOfType<TurnController>();
    }

    protected override void OnEnable() => _turnController.OnTurnChanged += OnTurnChanged;

    protected override void OnDisable() => _turnController.OnTurnChanged -= OnTurnChanged;

    private void OnTurnChanged(TurnState turn)
    {
        if (turn == _playerTurn.MyTurn && _scoreController.Score >= _shieldProperties._requiredScoreAmmount && _healthController.Health <= 60 && _seconds <= 0)
            StartCoroutine(ActivateShields());
    }

    private IEnumerator ActivateShields()
    {
        ActivateShields(1);
        _seconds = (_shieldProperties._minutes * 60) + _shieldProperties._seconds;

        while (_seconds > 0)
        {
            _seconds--;
            yield return new WaitForSeconds(1);
        }
    }
}

using System.Collections;
using UnityEngine;

public class AIShields : PlayerShields
{
    [SerializeField] private PropsProperties _shieldProperties;
    private ScoreController _scoreController;
    private HealthController _healthController;
    private TurnController _turnController;
    private int _time;


    protected override void Awake()
    {
        _shields = Get<Shields>.FromChild(gameObject);
        _playerTurn = Get<PlayerTurn>.From(gameObject);
        _scoreController = Get<ScoreController>.From(gameObject);
        _healthController = Get<HealthController>.From(gameObject);
        _turnController = FindObjectOfType<TurnController>();
    }

    protected override void OnEnable()
    {
        _turnController.OnTurnChanged += OnTurnChanged;
    }

    protected override void OnDisable()
    {
        _turnController.OnTurnChanged -= OnTurnChanged;
    }

    private void OnTurnChanged(TurnState turn)
    {
        if(turn == _playerTurn.MyTurn && _scoreController.Score >= _shieldProperties._requiredScoreAmmount && _healthController.Health <= 60 && _time < 1)
        {
            StartCoroutine(ShieldUsageCoroutine());
        }
    }

    private IEnumerator ShieldUsageCoroutine()
    {
        _shields?.Activity(1, true);
        IsShieldActive = true;
        _time++;
        yield return new WaitForSeconds(60);
        IsShieldActive = false;
        _shields?.Activity(1, false);
    }
}

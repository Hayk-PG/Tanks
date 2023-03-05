using UnityEngine;
using System;

public abstract class PlayerFeedbackController : MonoBehaviour
{
    protected BasePlayerTankController<BasePlayer> _playerController;

    protected HealthController _healthController;

    protected ScoreController _scoreController;

    protected PlayerTurn _playerTurn;

    protected AmmoTabCustomization _ammoTabCustomization;

    protected PlayerFeedback _playerFeedback;

    protected int _playerHitsIndex, _playerTurnIndex;

    protected abstract bool IsAllowed { get; }






    protected virtual void Awake()
    {
        _playerController = Get<BasePlayerTankController<BasePlayer>>.From(gameObject);

        _ammoTabCustomization = FindObjectOfType<AmmoTabCustomization>();

        _playerFeedback = FindObjectOfType<PlayerFeedback>();
    }

    protected virtual void OnEnable()
    {
        GameSceneObjectsReferences.GameManager.OnGameStarted += OnGameStarted;

        GameSceneObjectsReferences.TurnController.OnTurnChanged += OnTurnChanged;
    }

    protected virtual void OnDisable()
    {
        GameSceneObjectsReferences.GameManager.OnGameStarted -= OnGameStarted;

        GameSceneObjectsReferences.TurnController.OnTurnChanged -= OnTurnChanged;
    }

    protected virtual void OnGameStarted()
    {
        if (IsAllowed)
        {
            GetTankComponents();

            SubscribeToEvents();
        }
    }

    protected virtual void GetTankComponents()
    {
        _healthController = Get<HealthController>.From(_playerController.OwnTank.gameObject);

        _scoreController = Get<ScoreController>.From(_playerController.OwnTank.gameObject);

        _playerTurn = Get<PlayerTurn>.From(_playerController.OwnTank.gameObject);
    }

    protected virtual void SubscribeToEvents()
    {
        _healthController.OnTakeDamage += OnTakeDamage;

        _scoreController.OnHitEnemy += scores => { OnHitEnemy(scores, hitsCount); };

        _ammoTabCustomization.OnPlayerWeaponChanged += OnPlayerWeaponChanged;
    }

    protected abstract void OnTakeDamage(BasePlayer basePlayer, int damage);

    protected abstract void OnHitEnemy(int[] scores, Func<int> hitsCount);

    protected virtual int hitsCount()
    {
        return _playerHitsIndex = _playerTurnIndex + 1;
    }

    protected abstract void OnPlayerWeaponChanged(AmmoTypeButton ammoTypeButton);

    protected virtual void OnTurnChanged(TurnState turnState)
    {
        if (_playerTurn == null)
            return;

        ResetHitsCount(turnState);
    }

    protected virtual void ResetHitsCount(TurnState turnState)
    {
        if (turnState == _playerTurn.MyTurn)
        {
            _playerTurnIndex++;

            if (_playerTurnIndex > _playerHitsIndex)
            {
                _playerTurnIndex = 0;
                _playerHitsIndex = 0;
            }
        }
    }
}

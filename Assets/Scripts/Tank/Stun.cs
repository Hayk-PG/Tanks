using System.Collections;
using UnityEngine;

public class Stun : MonoBehaviour
{
    private ParticleSystem _particles;
    private HealthController _healthController;
    private PlayerTurn _playerTurn;
    private TurnTimer _turnTimer;
    private GlobalTankStun _globalTankStun;

    public bool IsStunned
    {
        get => _particles.isPlaying;
        set => ParticlesActivity(value);
    }

    public event System.Action<bool> OnStunEffect;



    private void Awake()
    {
        _particles = Get<ParticleSystem>.FromChild(gameObject);
        _healthController = Get<HealthController>.From(transform.parent.gameObject);
        _playerTurn = Get<PlayerTurn>.From(transform.parent.gameObject);
        _turnTimer = FindObjectOfType<TurnTimer>();
        _globalTankStun = FindObjectOfType<GlobalTankStun>();
    }

    private void OnEnable() => _healthController.OnTakeDamage += OnTakeDamage;

    private void OnDisable() => _healthController.OnTakeDamage -= OnTakeDamage;

    private void ParticlesActivity(bool isActive)
    {
        if (isActive && _particles.isStopped)
            _particles.Play(true);

        if (!isActive && _particles.isPlaying)
            _particles.Stop(true);
    }

    private void OnTakeDamage(BasePlayer basePlayer, int damage)
    {
        if (damage >= 30 && _healthController.Health > 0)
        {
            float duration = Random.Range(1, _turnTimer.RoundTime / 2);

            Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode, () => OnStunned(duration), () => _globalTankStun.OnStunned(_playerTurn.MyTurn, duration));
        }
    }

    public void OnStunned(float duration)
    {
        if (!IsStunned)
        {           
            IsStunned = true;
            StartCoroutine(DisableStunningEffect(duration));
            OnStunEffect?.Invoke(true);
        }
    }

    public void OnDisableStunningEffect()
    {
        if (IsStunned)
        {
            IsStunned = false;
            OnStunEffect?.Invoke(false);
        }
    }

    private IEnumerator DisableStunningEffect(float duration)
    {
        yield return new WaitForSeconds(duration);

        Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode, OnDisableStunningEffect, () => _globalTankStun.OnDisableStunningEffect(_playerTurn.MyTurn));
    }
}

using System;
using UnityEngine;

public class PlayerDamageCameraFX : BaseCameraFX
{
    [SerializeField] 
    private Animator _animator;

    [SerializeField] 
    private GameManager _gameManager;

    private HealthController _localHp;

    private const string _damageFX = "PPImageFilteringAnim";

    public event Action<int> onDamageFX;


    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        _gameManager.OnGameStarted += GetLocalPlayerOnGameStart;
    }

    private void OnDisable()
    {
        _gameManager.OnGameStarted -= GetLocalPlayerOnGameStart;

        if (_localHp != null)
            _localHp.OnTakeDamage -= PlayerDamageFX;
    }

    private void GetLocalPlayerOnGameStart()
    {
        _localHp = GlobalFunctions.ObjectsOfType<TankController>.Find(tank => tank.BasePlayer != null).GetComponent<HealthController>();

        if (_localHp != null)
            _localHp.OnTakeDamage += PlayerDamageFX;
    }

    public void PlayerDamageFX(BasePlayer basePlayer,int damage)
    {
        _animator.Play(_damageFX);
        onDamageFX?.Invoke(damage);
    }
}

using UnityEngine;

public class PlayerDamageCameraFX : BaseCameraFX<PlayerDamageCameraFX>
{
    private Animator _animator;

    private const string _damageFX = "PPImageFilteringAnim";

    private GameManager _gameManager;
    private HealthController _localHp;


    protected override void Awake()
    {
        base.Awake();
        _animator = Get<Animator>.From(transform.parent.gameObject);
        _gameManager = FindObjectOfType<GameManager>();
    }

    private void OnEnable()
    {
        if (_gameManager != null) _gameManager.OnGameStarted += GetLocalPlayerOnGameStart;
    }

    private void OnDisable()
    {
        if (_gameManager != null) _gameManager.OnGameStarted -= GetLocalPlayerOnGameStart;
        if (_localHp != null) _localHp.OnTakeDamage -= PlayerDamageFX;
    }

    private void GetLocalPlayerOnGameStart()
    {
        _localHp = GlobalFunctions.ObjectsOfType<HealthController>.Find(hp => hp.tag == Tags.Player);

        if (_localHp != null) _localHp.OnTakeDamage += PlayerDamageFX;
    }

    public void PlayerDamageFX(int damage)
    {
        _animator.Play(_damageFX);
    }
}

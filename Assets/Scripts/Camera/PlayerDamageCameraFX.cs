using UnityEngine;

public class PlayerDamageCameraFX : BaseCameraFX
{
    private Animator _animator;  
    private GameManager _gameManager;
    private HealthController _localHp;
    private const string _damageFX = "PPImageFilteringAnim";

    protected override void Awake()
    {
        base.Awake();
        _animator = Get<Animator>.From(transform.parent.gameObject);
        _gameManager = FindObjectOfType<GameManager>();
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
    }
}

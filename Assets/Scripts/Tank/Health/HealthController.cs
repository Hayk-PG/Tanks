using UnityEngine;

public class HealthController : MonoBehaviour, IDamage
{
    [SerializeField]
    private int _currentHealth;
    private int _minHealth = 0;
    private int _maxHealth = 100;

    private HealthBar _healthBar;
    private PlayerTurn _playerTurn;

    public int Health
    {
        get => _currentHealth;
        set => _currentHealth = value;
    }


    private void Awake()
    {
        Health = _maxHealth;
        _healthBar = FindObjectOfType<HealthBar>();
        _playerTurn = Get<PlayerTurn>.From(gameObject);
    }

    public void Damage(int damage)
    {
        Health -= damage;
        _healthBar.OnUpdateHealthBar(_playerTurn.MyTurn, Health);
    }
}

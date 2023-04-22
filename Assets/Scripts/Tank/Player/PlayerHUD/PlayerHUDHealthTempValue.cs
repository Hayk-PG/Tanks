using UnityEngine;
using TMPro;

public class PlayerHUDHealthTempValue : MonoBehaviour
{
    [SerializeField] 
    private Color[] _colors;

    private Animator _animator;

    private TMP_Text _text;

    private HealthController _healthController;

    private int _healthOldValue, _healthNewValue;




    private void Awake()
    {
        _animator = Get<Animator>.From(gameObject);

        _text = Get<TMP_Text>.From(gameObject);

        _healthController = Get<HealthController>.From(transform.parent.gameObject);

        _healthOldValue = _healthController.Health;
    }

    private void OnEnable()
    {
        if (_healthController == null)
            return;

        _healthController.OnUpdateHealthBar += OnUpdateHealthBar;
    }

    private void OnUpdateHealthBar(int health)
    {
        _healthNewValue = health > _healthOldValue ? health - _healthOldValue : 
                          health < _healthOldValue ? -(Mathf.Abs(_healthOldValue - health)) : 0;

        _healthOldValue = health;

        _text.color = _healthNewValue > 0 ? _colors[1] : _colors[0];
        _text.text = _healthNewValue > 0 ? "+" + _healthNewValue : _healthNewValue.ToString();

        if (_healthNewValue != 0)
            _animator.SetTrigger("play");
    }
}

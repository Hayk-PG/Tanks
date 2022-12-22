using UnityEngine;
using UnityEngine.UI;

public class HUDComponentHealthBar : MonoBehaviour
{
    [SerializeField] private TurnState _turnState;
    [SerializeField] private Image _imgFillLayer1, _imgFillLayer2;
    [SerializeField] private Animator _animator;
    private TurnController _turnController;
    private HealthController _healthController;

    private bool _isSubscribed;



    private void Awake() => _turnController = FindObjectOfType<TurnController>();

    private void OnEnable() => _turnController.OnTurnChanged += delegate { SubscribeToHealthController(); };

    private void OnDisable()
    {
        _turnController.OnTurnChanged -= delegate { SubscribeToHealthController(); };

        if (_healthController != null)
            _healthController.OnUpdateHealthBar -= OnUpdateHealthBar;
    }

    private void SubscribeToHealthController()
    {
        if (!_isSubscribed)
        {
            PlayerTurn playerTurn = GlobalFunctions.ObjectsOfType<PlayerTurn>.Find(turn => turn.MyTurn == _turnState);

            if (playerTurn != null)
            {
                _healthController = Get<HealthController>.From(playerTurn.gameObject);
                _healthController.OnUpdateHealthBar += OnUpdateHealthBar;
            }

            _isSubscribed = true;
        }
    }

    private void OnUpdateHealthBar(int value)
    {
        _imgFillLayer1.fillAmount = (float)value / 100;
        _animator.SetTrigger("play");
    }

    public void UpdateHealthBarLayer2() => _imgFillLayer2.fillAmount = _imgFillLayer1.fillAmount;
}

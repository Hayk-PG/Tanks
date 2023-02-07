using UnityEngine;
using UnityEngine.UI;

public class HUDComponentHealthBar : MonoBehaviour
{
    [SerializeField] [Space]
    protected TurnState _turnState;

    [SerializeField] [Space]
    protected Image _imgFillLayer1, _imgFillLayer2;

    [SerializeField] [Space]
    protected Animator _animator;

    [SerializeField] [Space]
    protected TurnController _turnController;

    protected PlayerTurn _playerTurn;
    protected HealthController _healthController;

    protected bool _isSubscribed;

    protected virtual string AnimationStateName => "LastHealthFillAnim";
    protected virtual int AnimationLayer => 0;



    protected virtual void OnEnable()
    {
        _turnController.OnTurnChanged += delegate { Initialize(); };
    }

    protected virtual void OnDisable()
    {
        _turnController.OnTurnChanged -= delegate { Initialize(); };

        UnsubscribeFromHealthController();
    }

    protected virtual void Initialize()
    {
        if (!_isSubscribed)
        {
            _playerTurn = GlobalFunctions.ObjectsOfType<PlayerTurn>.Find(turn => turn.MyTurn == _turnState);

            if (_playerTurn != null)
            {
                GetHealthController();
                SubscribeToHealthController();
            }

            _isSubscribed = true;
        }
    }

    protected virtual void GetHealthController()
    {
        _healthController = Get<HealthController>.From(_playerTurn.gameObject);
    }

    protected virtual void SubscribeToHealthController()
    {
        if(_healthController != null)
            _healthController.OnUpdateHealthBar += OnUpdateBar;
    }

    protected virtual void UnsubscribeFromHealthController()
    {
        if (_healthController != null)
            _healthController.OnUpdateHealthBar -= OnUpdateBar;
    }

    protected virtual void OnUpdateBar(int value)
    {
        _imgFillLayer1.fillAmount = (float)value / 100;
        _animator.Play(AnimationStateName, AnimationLayer, 0);
    }

    public void MatchHealthBarValues()
    {
        _imgFillLayer2.fillAmount = _imgFillLayer1.fillAmount;      
    }
}

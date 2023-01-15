using System;

public class PlayerShields : PlayerDeployProps
{
    protected Shields _shields;

    public bool IsShieldActive { get; set; }

    public event Action<bool> onShieldActivity;



    protected override void Awake() => base.Awake();

    protected override void Start() => InitializeRelatedPropsButton(Names.Shield);

    protected override void OnDisable()
    {
        _tankController.OnInitialize -= OnInitialize;
        _propsTabCustomization.OnActivateShields -= OnActivateShields;
    }

    protected override void SubscribeToPropsEvent() => _propsTabCustomization.OnActivateShields += OnActivateShields;

    public virtual void SetShield(Shields shields) => _shields = shields;

    private void OnActivateShields()
    {
        if (_playerTurn.IsMyTurn)
        {
            int index = _playerTurn.MyTurn == TurnState.Player1 ? 0 : 1;
            _iPlayerDeployProps?.ActivateShields(index);
            _propsTabCustomization.OnSupportOrPropsChanged?.Invoke(_relatedPropsTypeButton);
        }
    }

    public void ActivateShields(int index)
    {
        if (_shields == null)
            return;

        if (IsShieldActive == false)
        {
            IsShieldActive = true;
            _shields.Activity(index, IsShieldActive);
            onShieldActivity?.Invoke(true);
        }
    }

    public void DeactivateShields()
    {
        if (_shields == null)
            return;

        if (IsShieldActive == true)
        {
            IsShieldActive = false;
            for (int i = 0; i < 2; i++)
            {
                _shields.Activity(i, IsShieldActive);
            }
            onShieldActivity?.Invoke(false);
        }
    }
}

using UnityEngine;

public class AbilityShield : BaseAbility
{
    [SerializeField] [Space]
    private PlayerShields _playerShields;

    protected override string Title => "Shield";

    protected override string Ability => "Activate an impenetrable shield to protect your tank.";





    protected override void OnEnable()
    {
        base.OnEnable();

        _playerShields.onShieldActivity += OnShieldActive;
    }

    protected override void OnAbilityActivated(object[] data = null)
    {
        base.OnAbilityActivated(data);

        ActivateShield();
    }

    private void ActivateShield() => _playerShields.ActivateShields(_playerTurn.MyTurn == TurnState.Player1 ? 0 : 1);

    private void OnShieldActive(bool isActive)
    {
        if (isActive)
            return;

        IsAbilityActive = false;
    }

    protected override void ActivateBuffDebuffIcon(object[] data = null)
    {
        
    }

    protected override void DeactivateBuffDebuffIcon()
    {
        
    }

    protected override void UpdateAbilityBuffIcon()
    {
        
    }

    public override void AssignBuffDebuffUIElement(BuffDebuffUIElement buffDebuffUIElement)
    {
        
    }
}

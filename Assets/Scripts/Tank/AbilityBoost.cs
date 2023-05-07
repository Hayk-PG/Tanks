using UnityEngine;

public class AbilityBoost : BaseAbility
{
    [SerializeField] [Space]
    private BaseTankMovement _baseTankMovement;

    protected override string Title => "Boost";

    protected override string Ability => "Increase movement speed by 2 for 4 turn";





    protected override void OnAbilityActivated(object[] data = null)
    {
        base.OnAbilityActivated(data);

        Boost(true);
    }

    protected override void OnTurnChanged(TurnState turnState)
    {
        if (IsAbilityActive && _playerTurn.IsMyTurn)
            DeactivateAbilityAfterLimit();
    }

    protected override void OnAbilityDeactivated()
    {
        base.OnAbilityDeactivated();

        Boost(false);
    }

    private void Boost(bool isMovementBoosted) => _baseTankMovement.Boost(isMovementBoosted);

    public override void AssignBuffDebuffUIElement(BuffDebuffUIElement buffDebuffUIElement) => BuffDebuffUIElement = buffDebuffUIElement;
}

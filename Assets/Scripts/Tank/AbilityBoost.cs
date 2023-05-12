using UnityEngine;

public class AbilityBoost : BaseAbility
{
    private IMovementBoostObserver[] _iMovementBoostObservers;

    private bool _isMovementObserversGet;

    protected override string Title => "Boost";
    protected override string Ability => "Increase movement speed by 2 for 4 turn";





    protected override void OnAbilityActivated(object[] data = null)
    {
        base.OnAbilityActivated(data);

        Boost(true);

        PlayBoostSoundEffect(2);
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

    private void Boost(bool isMovementBoostActive)
    {
        GetMovementBoostObservers();

        SetMovementBoostObserversActive(isMovementBoostActive);
    }

    private void PlayBoostSoundEffect(int clipIndex) => SecondarySoundController.PlaySound(10, clipIndex);

    // The boolean variable "_isMovementObserversGet" is used to ensure that the method is only executed once.

    private void GetMovementBoostObservers()
    {
        if (_isMovementObserversGet)
            return;

        _isMovementObserversGet = true;

        _iMovementBoostObservers = GetComponentsInChildren<IMovementBoostObserver>();
    }

    private void SetMovementBoostObserversActive(bool isMovementBoostActive)
    {
        GlobalFunctions.Loop<IMovementBoostObserver>.Foreach(_iMovementBoostObservers, observer => observer?.SetMovementBoostActive(isMovementBoostActive));
    }

    public override void AssignBuffDebuffUIElement(BuffDebuffUIElement buffDebuffUIElement) => BuffDebuffUIElement = buffDebuffUIElement;
}

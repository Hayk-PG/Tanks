public class HUDComponentArmorBar : HUDComponentHealthBar
{
    protected override string AnimationStateName => "LastArmorFillAnim";
    protected override int AnimationLayer => 1;


    protected override void SubscribeToHealthController()
    {
        if (_healthController != null)
            _healthController.onUpdateArmorBar += OnUpdateBar;
    }

    protected override void UnsubscribeFromHealthController()
    {
        if (_healthController != null)
            _healthController.onUpdateArmorBar -= OnUpdateBar;
    }

    public void MatchArmorBarValues()
    {
        _imgFillLayer2.fillAmount = _imgFillLayer1.fillAmount;
    }
}

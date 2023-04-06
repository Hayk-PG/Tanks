using System;

public class OfflinePlayerFeedbacksController : PlayerFeedbackController
{
    protected override bool IsAllowed => _playerController != null;

    protected override void OnHitEnemy(int[] scores, Func<int> hitsCount)
    {
        _playerFeedback.OnHitEnemy(_playerController.OwnTank.name, hitsCount(), scores);
    }

    protected override void OnPlayerWeaponChanged(AmmoTypeButton ammoTypeButton)
    {
        _playerFeedback.DisplayWeaponChangeText(_playerController.OwnTank.name, ammoTypeButton._properties.Name);
    }

    protected override void OnTakeDamage(BasePlayer basePlayer, int damage) => _playerFeedback.DisplayDamageText(_playerController.OwnTank.name, damage);
}

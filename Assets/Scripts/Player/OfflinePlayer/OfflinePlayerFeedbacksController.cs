using System;

public class OfflinePlayerFeedbacksController : PlayerFeedbackController
{
    protected override bool IsAllowed => _playerController != null;

    protected override void OnHitEnemy(int[] scores, Func<int> hitsCount)
    {
        GameSceneObjectsReferences.PlayerFeedback.OnHitEnemy(_playerController.OwnTank.name, hitsCount(), scores);
    }

    protected override void OnPlayerWeaponChanged(AmmoTypeButton ammoTypeButton)
    {
        GameSceneObjectsReferences.PlayerFeedback.DisplayWeaponChangeText(_playerController.OwnTank.name, ammoTypeButton._properties.Name);
    }

    protected override void OnTakeDamage(BasePlayer basePlayer, int damage) => GameSceneObjectsReferences.PlayerFeedback.DisplayDamageText(_playerController.OwnTank.name, damage);
}

public class ExplosionCameraFX : Explosion
{
    protected override void DamageAndScoreInOfflineMode(IDamage iDamage, int damageValue, int[] scoreValues)
    {
        base.DamageAndScoreInOfflineMode(iDamage, damageValue, scoreValues);
        iDamage.CameraChromaticAberrationFX();
    }

    protected override void DamageAndScoreInOlineMode(IDamage iDamage, int damageValue, int[] scoreValues)
    {
        _gameManagerBulletSerializer.CallDamageAndScoreRPC(iDamage, OwnerScore, _currentDamageValue, scoreValues, 1);
    }
}


public class ArmorIgnoringExplosion : Explosion
{
    protected override void ApplyDamage(IDamage iDamage, int damageValue, bool ignoreArmor = false)
    {
        // Override the 'ignoreArmor' parameter and set it to true to ignore the armor while applying damage.

        base.ApplyDamage(iDamage, damageValue, true);
    }

    protected override void DamageAndScoreInOlineMode(IDamage iDamage, int damageValue, int[] scores, bool ignoreArmor = false)
    {
        // Override the 'ignoreArmor' parameter and set it to true to ignore the armor while applying damage.

        base.DamageAndScoreInOlineMode(iDamage, damageValue, scores, true);
    }
}

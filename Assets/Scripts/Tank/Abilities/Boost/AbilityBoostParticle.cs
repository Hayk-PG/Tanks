
public class AbilityBoostParticle : BaseAbilityParticle<AbilityBoost>
{
    protected override void PlayParticle()
    {
        base.PlayParticle();

        Unparent();
    }

    private void Unparent() => transform.SetParent(null);
}

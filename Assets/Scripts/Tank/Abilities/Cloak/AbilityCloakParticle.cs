
public class AbilityCloakParticle : BaseAbilityParticle<AbilityCloak>
{
    protected override void Awake()
    {
        base.Awake();

        AssignAbilityParticle();
    }

    private void AssignAbilityParticle() => _ability.AssignAbilityParticle(_particle);
}

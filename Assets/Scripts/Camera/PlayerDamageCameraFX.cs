using UnityEngine;

public class PlayerDamageCameraFX : BasePlayerDependentCameraEffects<HealthController>
{
    [SerializeField] [Space]
    private Animator _animator;

    private const string _damageFX = "PPImageFilteringAnim";



    protected override void Execute() => _t.OnTakeDamage += PlayerDamageFX;

    private void PlayerDamageFX(BasePlayer basePlayer, int damage) => _animator.Play(_damageFX);
}

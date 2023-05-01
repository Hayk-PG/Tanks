using UnityEngine;

//ADDRESSABLE
public class ExplosionAcidAdressable : ExplosionAddressables
{
    [SerializeField] [Space]
    private BaseExplosion _baseExplosion;



    protected override void OnAssetInstantiate(GameObject asset)
    {
        SetOwnerScore(asset);

        base.OnAssetInstantiate(asset);
    }

    private void SetOwnerScore(GameObject asset)
    {
        AcidExplosionParticleController acidExplosionParticleController = Get<AcidExplosionParticleController>.FromChild(asset);

        if (acidExplosionParticleController == null)
            return;

        acidExplosionParticleController.OwnerScore = _baseExplosion.OwnerScore;
        acidExplosionParticleController.IDamageAi = _baseExplosion.IDamageAi;
    }
}

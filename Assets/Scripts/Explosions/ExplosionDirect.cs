using UnityEngine;

public class ExplosionDirect : BaseExplosion
{
    [SerializeField] 
    private int _damage, _destructDamage;

    public override float DamageValue { get => _damage; set => _damage = Mathf.RoundToInt(value); }
    public override float DestructDamageValue { get => _destructDamage; set => _destructDamage = Mathf.RoundToInt(value); }





    private void Start()
    {
        base.Start();

        HitTank(Get<IDamage>.From(Collider.gameObject));
        HitTile(Get<IDestruct>.From(Collider.gameObject));
    }

    private void HitTank(IDamage iDamage)
    {
        if (iDamage == default)
            return;

        int dmg = Mathf.RoundToInt(DamageValue);

        int[] scores = new int[] { Mathf.RoundToInt(DamageValue * 10) + Mathf.RoundToInt((Distance) * 100), Mathf.RoundToInt((Distance * RadiusValue) * 10) };

        Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode, () => DamageTankAndGetScored(iDamage, dmg, scores),
                                                                () => DamageTankAndGetScoredRPC(iDamage, dmg, scores));
    }

    private void HitTile(IDestruct iDestruct)
    {
        if (iDestruct == default)
            return;

        Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode, () => DamageTileAndGetScored(iDestruct), DamageTileAndGetScoredRPC);
    }

    private void DamageTankAndGetScored(IDamage iDamage, int damageValue, int[] scores)
    {
        iDamage.Damage(Collider, damageValue);

        Score(null, scores);
    }

    private void DamageTankAndGetScoredRPC(IDamage iDamage, int damageValue, int[] scores)
    {
        GameSceneObjectsReferences.GameManagerBulletSerializer.CallDamageAndScoreRPC(iDamage, OwnerScore, damageValue, scores, 0);
    }

    private void DamageTileAndGetScored(IDestruct iDestruct)
    {
        iDestruct.Destruct(_destructDamage, 0);

        OwnerScore.GetScore(Random.Range(10, 110), null, transform.position);
    }

    private void DamageTileAndGetScoredRPC()
    {
        GameSceneObjectsReferences.GameManagerBulletSerializer.CallOnCollisionRPC(Collider, OwnerScore, _destructDamage);
    }
}

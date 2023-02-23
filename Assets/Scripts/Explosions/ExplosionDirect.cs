using UnityEngine;

public class ExplosionDirect : BaseExplosion
{
    [SerializeField] 
    private int _damage, _destructDamage;

    private System.Action<IDamage, int, int[]> DamageTankAndGetScoredFunction;
    private System.Action<IDestruct> DamageTileAndGetScoredFunction;

    public override float DamageValue { get => _damage; set => _damage = Mathf.RoundToInt(value); }
    public override float DestructDamageValue { get => _destructDamage; set => _destructDamage = Mathf.RoundToInt(value); }




    private void Awake()
    {
        DamageTankAndGetScoredFunction = MyPhotonNetwork.IsOfflineMode ? DamageTankAndGetScored : DamageTankAndGetScoredRPC;
        DamageTileAndGetScoredFunction = MyPhotonNetwork.IsOfflineMode ? DamageTileAndGetScored : DamageTileAndGetScoredRPC;
    }

    private void Start()
    {
        HitTank(Get<IDamage>.From(Collider.gameObject));
        HitTile(Get<IDestruct>.From(Collider.gameObject));
    }

    private void HitTank(IDamage iDamage)
    {
        if (iDamage == default)
            return;

        int dmg = Mathf.RoundToInt(DamageValue);
        int[] scores = new int[] { Mathf.RoundToInt(DamageValue * 10) + Mathf.RoundToInt((Distance) * 100), Mathf.RoundToInt((Distance * RadiusValue) * 10) };

        DamageTankAndGetScoredFunction?.Invoke(iDamage, dmg, scores);
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

    private void HitTile(IDestruct iDestruct)
    {
        if (iDestruct == default)
            return;

        DamageTileAndGetScoredFunction?.Invoke(iDestruct);
    }

    private void DamageTileAndGetScored(IDestruct iDestruct)
    {
        iDestruct.Destruct(_destructDamage, 0);

        OwnerScore.GetScore(10, null);
    }

    private void DamageTileAndGetScoredRPC(IDestruct iDestruct)
    {
        GameSceneObjectsReferences.GameManagerBulletSerializer.CallOnCollisionRPC(Collider, _destructDamage);
    }
}

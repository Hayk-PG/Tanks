using UnityEngine;

public class ExplosionDirect : BaseExplosion
{
    [SerializeField] 
    private int _damage, _destructDamage;

    public override float DamageValue { get => _damage; set => _damage = Mathf.RoundToInt(value); }
    public override float DestructDamageValue { get => _destructDamage; set => _destructDamage = Mathf.RoundToInt(value); }




    private void Start()
    {
        HitTank(Get<IDamage>.From(Collider.gameObject));
        HitTile(Get<IDestruct>.From(Collider.gameObject));
    }

    private void HitTank(IDamage iDamage)
    {
        if (iDamage == null)
            return;

        int dmg = Mathf.RoundToInt(DamageValue);
        int[] scores = new int[] { Mathf.RoundToInt(DamageValue * 10) + Mathf.RoundToInt((Distance) * 100), Mathf.RoundToInt((Distance * RadiusValue) * 10) };

        Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode, () => DamageTankAndGetScoredInOfflineMode(iDamage, dmg, scores),
        () => DamageTankAndGetScoredInOnlineMode(iDamage, dmg, scores));
    }

    private void DamageTankAndGetScoredInOfflineMode(IDamage iDamage, int damageValue, int[] scores)
    {
        iDamage.Damage(Collider, damageValue);
        Score(null, scores);
    }

    private void DamageTankAndGetScoredInOnlineMode(IDamage iDamage, int damageValue, int[] scores)
    {
        _gameManagerBulletSerializer.CallDamageAndScoreRPC(iDamage, OwnerScore, damageValue, scores, 0);
    }

    private void HitTile(IDestruct iDestruct)
    {
        if (iDestruct == null)
            return;

        Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode, () => DamageTileAndGetScoredInOfflineMode(iDestruct), DamageTileAndGetScoredInOnlineMode);
    }

    private void DamageTileAndGetScoredInOfflineMode(IDestruct iDestruct)
    {
        iDestruct.Destruct(_destructDamage, 0);
        OwnerScore.GetScore(10, null);
    }

    private void DamageTileAndGetScoredInOnlineMode()
    {
        _gameManagerBulletSerializer.CallOnCollisionRPC(Collider, _destructDamage);
    }
}

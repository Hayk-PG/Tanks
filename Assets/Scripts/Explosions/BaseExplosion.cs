using UnityEngine;

public class BaseExplosion : MonoBehaviour
{
    public IScore OwnerScore { get; set; }
    public Collider Collider { get; set; }

    public float Distance { get; set; }
    public virtual float DamageValue { get; set; }
    public virtual float DestructDamageValue { get; set; }
    public virtual float RadiusValue { get; set; } = 0.1f;



    protected virtual void Score(IDamage iDamage, int[] scores)
    {
        OwnerScore?.GetScore(scores[0] + scores[1], iDamage);
        OwnerScore?.HitEnemyAndGetScore(scores, iDamage);
    }
}

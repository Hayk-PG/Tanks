using UnityEngine;

public class BulletWithMultipleHitParticles : BulletParticles
{
    [SerializeField] private Explosion[] _multipleExplosions;
    [HideInInspector] public int _explosionsCount;
    private int _explosionIndex;
    

    protected override void Awake()
    {
        base.Awake();

        _explosionsCount = _multipleExplosions.Length;
    }

    protected override void OnExplosion(IScore ownerScore, float distance)
    {
        _multipleExplosions[_explosionIndex].OwnerScore = ownerScore;
        _multipleExplosions[_explosionIndex].Distance = distance;
        _multipleExplosions[_explosionIndex].gameObject.SetActive(true);
        _multipleExplosions[_explosionIndex].transform.parent = null;
        _explosionIndex++;
    }
}

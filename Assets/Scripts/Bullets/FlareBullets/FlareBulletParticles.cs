using System;
using UnityEngine;

public class FlareBulletParticles : BulletParticles
{
    public Action<IScore, PlayerTurn, Vector3> OnFlareSignal { get; set; }


    protected override void OnEnable()
    {
        base.OnEnable();

        if (_iBulletExplosion != null) _iBulletExplosion.OnFlareBulletExplosion += OnFlareBulletExplosion;        
    }

    protected override void OnDisable()
    {
        base.OnEnable();

        if (_iBulletExplosion != null) _iBulletExplosion.OnFlareBulletExplosion -= OnFlareBulletExplosion;       
    }

    private void OnFlareBulletExplosion(IScore ownerScore, float distance, Vector3? point)
    {
        PlayerTurn ownerTurn = GlobalFunctions.ObjectsOfType<PlayerTurn>.Find(turn => Get<IScore>.From(turn.gameObject) == ownerScore);
        _explosion.gameObject.SetActive(true);
        _explosion.transform.parent = null;
        _explosion.transform.eulerAngles = Vector3.zero;
        if (point != null) _explosion.transform.position = point.Value;
        OnFlareSignal?.Invoke(ownerScore, ownerTurn, _explosion.transform.position);
    }
}

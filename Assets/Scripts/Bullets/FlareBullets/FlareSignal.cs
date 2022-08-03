using UnityEngine;

public abstract class FlareSignal<T> : MonoBehaviour where T: MonoBehaviour
{
    protected FlareBulletParticles _flareBulletParticles;
    protected T _signalReceiver;


    protected virtual void Awake()
    {
        _flareBulletParticles = Get<FlareBulletParticles>.From(gameObject);
        _signalReceiver = FindObjectOfType<T>();
    }

    protected virtual void OnEnable()
    {
        if (_flareBulletParticles != null)
            _flareBulletParticles.OnFlareSignal += OnFlareSignal;
    }

    protected virtual void OnDisable()
    {
        if (_flareBulletParticles != null)
            _flareBulletParticles.OnFlareSignal -= OnFlareSignal;
    }

    protected abstract void OnFlareSignal(IScore ownerScore, PlayerTurn ownerTurn, Vector3 point);
}

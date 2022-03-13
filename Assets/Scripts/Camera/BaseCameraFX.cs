using UnityEngine;

public class BaseCameraFX<T> : MonoBehaviour
{
    protected static T _instance;

    protected MobilePostProcessing _pp;


    protected virtual void Awake()
    {
        _pp = Get<MobilePostProcessing>.From(gameObject);
    }

    protected void Singleton(T i)
    {
        if (_instance == null)
            _instance = i;
    }
}

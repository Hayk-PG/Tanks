using UnityEngine;

public class BaseCameraFX : MonoBehaviour
{
    protected MobilePostProcessing _pp;


    protected virtual void Awake()
    {
        _pp = Get<MobilePostProcessing>.From(gameObject);
    }
}

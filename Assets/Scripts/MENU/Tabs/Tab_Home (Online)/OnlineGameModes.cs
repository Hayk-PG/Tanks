using UnityEngine;

public class OnlineGameModes<T> : MonoBehaviour
{
    protected T _broadcaster;


    protected virtual void Awake()
    {
        _broadcaster = Get<T>.From(gameObject);
    }
}

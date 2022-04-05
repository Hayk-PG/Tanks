using UnityEngine;

public class BasePlayVs : MonoBehaviour
{
    protected Tab_StartGame _tabStartGame;


    protected virtual void Awake()
    {
        _tabStartGame = Get<Tab_StartGame>.From(gameObject);
    } 
}

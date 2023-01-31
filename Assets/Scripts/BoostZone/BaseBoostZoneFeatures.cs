using UnityEngine;

public abstract class BaseBoostZoneFeatures 
{
    public void Use(GameObject player)
    {
        if (player == null)
            return;

        OnTriggerEnter(player);
    }

    public void Release(GameObject player)
    {
        if (player == null)
            return;

        OnTriggerExit(player);
    }

    protected abstract void OnTriggerEnter(GameObject player);
    protected abstract void OnTriggerExit(GameObject player);
}

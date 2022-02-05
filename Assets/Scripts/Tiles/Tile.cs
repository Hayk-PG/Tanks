using UnityEngine;

public class Tile : MonoBehaviour, IDamage
{
    Collider _collider;


    void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    public void Damage(float damage)
    {
        _collider.isTrigger = true;
        Destroy(gameObject);
    }
}

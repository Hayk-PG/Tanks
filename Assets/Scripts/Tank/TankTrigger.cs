using UnityEngine;

public class TankTrigger : MonoBehaviour
{
    private IDamage _iDamage;
    private IDestruct _iDestruct;
    private int _damageValue = 10;
    private float _time;

    private void Awake()
    {
        _iDamage = Get<IDamage>.From(gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.GetComponent<IDestruct>() != null)
        {
            _time += Time.deltaTime;

            if (_time >= 0.5f)
                TakeDamageAndDestroy(other);
        }
    }

    private void TakeDamageAndDestroy(Collider other)
    {
        _iDamage.Damage(_damageValue);
        _iDestruct = other.GetComponent<IDestruct>();
        _iDestruct.Destruct(100, 1000);
        _time = 0;
    }
}

using UnityEngine;

public class SandbagsReceiver : MonoBehaviour
{
    private Sandbags _sandBags;
    private TankController _tankController;
    private TankMovement _tankMovement;
    private ShootController _shootController;


    private void Awake()
    {
        _tankController = Get<TankController>.From(gameObject);
        _tankMovement = Get<TankMovement>.From(gameObject);
        _shootController = Get<ShootController>.From(gameObject);
    }

    public void SubscirbeToSandbagsEvents(Sandbags sandbags)
    {
        if (_tankController.BasePlayer != null)
        {
            if (_sandBags != sandbags)
            {
                UnsubscribeFromSandbagsEvents(sandbags);
                _sandBags = sandbags;
                _sandBags.OnSandbags += OnSandbags;
            }
        } 
    }

    public void UnsubscribeFromSandbagsEvents(Sandbags sandbags)
    {
        sandbags.OnSandbags -= OnSandbags;
    }

    private void OnSandbags(bool isEntered)
    {
        _tankMovement.OnEnteredSandbagsTrigger(isEntered);
        _shootController.OnEnteredSandbagsTrigger(isEntered);
    }
}

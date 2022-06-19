using System;
using UnityEngine;

public class Sandbags : MonoBehaviour
{
    public Action<bool> OnSandbags { get; set; }
    private SandbagsReceiver _sandBagsReceiver;


    private void OnTriggerEnter(Collider other)
    {
        SendEvent(other, true);
    }

    private void OnTriggerExit(Collider other)
    {
        SendEvent(other, false);
    }

    private bool IsTank(Collider other)
    {
        return Get<SandbagsReceiver>.From(other.gameObject);
    }

    private void CommunicateWithSandbagsReceiver(Collider other)
    {
        if (_sandBagsReceiver == null)
        {
            _sandBagsReceiver = Get<SandbagsReceiver>.From(other.gameObject);
            _sandBagsReceiver?.SubscirbeToSandbagsEvents(this);
        }
    }

    private void SendEvent(Collider other, bool isEntered)
    {
        if (IsTank(other))
        {
            CommunicateWithSandbagsReceiver(other);
            OnSandbags?.Invoke(isEntered);
        }
    }

    public void SandbagsDirection(bool isPlayer1)
    {
        transform.localEulerAngles = new Vector3(0, isPlayer1 ? 90 : -90, 0);
    }
}

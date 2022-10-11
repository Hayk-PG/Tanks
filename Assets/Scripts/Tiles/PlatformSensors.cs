using System;
using UnityEngine;

public class PlatformSensors : MonoBehaviour
{
    public enum SensorDirection { Bottom, Left, Right}

    public SensorDirection _sensorDirection;

    public Action<SensorDirection, Vector3> OnTriggerEntered { get; set; }


    private void OnTriggerEnter(Collider other)
    {
        if (!IsRigidbody(other.gameObject))
        {
            OnTriggerEntered?.Invoke(_sensorDirection, NewPosition(_sensorDirection));
        }
    }

    private bool IsRigidbody(GameObject gameObject)
    {
        return Get<Rigidbody>.From(gameObject);
    }

    private Vector3 NewPosition(SensorDirection sensorDirection)
    {
        if (sensorDirection == SensorDirection.Left)
            return new Vector3(0.5f, 0, 0);

        if(sensorDirection == SensorDirection.Right)
            return new Vector3(-0.5f, 0, 0);

        else
            return new Vector3(0, 0.5f, 0);
    }
}

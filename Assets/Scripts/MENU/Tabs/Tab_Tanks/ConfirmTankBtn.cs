using System;
using UnityEngine;

[RequireComponent(typeof(Btn))]

public class ConfirmTankBtn : MonoBehaviour
{
    private Btn _btn;

    public event Action onConfirmTankOffline;
    public event Action onConfirmTankOnline;
    public event Action onChangeTankInRoom;

    private void Awake()
    {
        _btn = Get<Btn>.From(gameObject);
    }

    private void OnEnable()
    {
        _btn.onSelect += Select;
    }

    private void OnDisable()
    {
        _btn.onSelect -= Select;
    }

    private void Select()
    {
        if (MyPhotonNetwork.IsOfflineMode)
        {
            onConfirmTankOffline?.Invoke();
        }

        if(!MyPhotonNetwork.IsOfflineMode)
        {
            if (MyPhotonNetwork.IsInRoom)
            {
                onChangeTankInRoom?.Invoke();
            }
            else
            {
                onConfirmTankOnline?.Invoke();
            }
        }
    }
}

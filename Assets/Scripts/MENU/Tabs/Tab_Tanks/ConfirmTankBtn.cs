using System;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmTankBtn : MonoBehaviour
{
    [SerializeField] private Button _btnConfirm;

    public event Action onConfirmTankOffline;
    public event Action onConfirmTankOnline;
    public event Action onChangeTankInRoom;


    private void Update()
    {
        _btnConfirm.onClick.RemoveAllListeners();
        _btnConfirm.onClick.AddListener(Click);
    }

    private void Click()
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

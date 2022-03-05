using System;
using UnityEngine;

public class AmmoTabButton : MonoBehaviour
{
    public event Action OnAmmoTabActivity;

    public void OnClickButton()
    {
        OnAmmoTabActivity?.Invoke();
    }
}

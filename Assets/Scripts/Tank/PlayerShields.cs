using UnityEngine;
using System;

public class PlayerShields : MonoBehaviour
{
    protected Shields _shields;

    public bool IsShieldActive { get; set; }


    public event Action<bool> onShieldActivity;




    public void SetShield(Shields shields) => _shields = shields;

    public void ActivateShields(int index)
    {
        if (_shields == null)
            return;

        if (IsShieldActive == false)
        {
            IsShieldActive = true;

            _shields.Activity(index, IsShieldActive);

            onShieldActivity?.Invoke(true);
        }
    }

    public void DeactivateShields()
    {
        if (_shields == null)
            return;

        if (IsShieldActive == true)
        {
            IsShieldActive = false;

            for (int i = 0; i < 2; i++)
            {
                _shields.Activity(i, IsShieldActive);
            }

            onShieldActivity?.Invoke(false);
        }
    }
}

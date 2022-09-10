using UnityEngine;

public class PlayerNewHUDShootValues : PlayerHUDShootValues
{
    private float _angle, _force;

    protected override void OnUpdateValues(ShootController.PlayerHUDValues hudValues)
    {
        _angle = Mathf.Round(Mathf.InverseLerp(hudValues._minAngle, hudValues._maxAngle, hudValues._currentAngle) * 100);
        _force = Mathf.Round(Mathf.InverseLerp(hudValues._minForce, hudValues._maxForce, hudValues._currentForce) * 100);

        OnPlayerHudShootValues?.Invoke(_angle.ToString(), _force.ToString());
    }
}

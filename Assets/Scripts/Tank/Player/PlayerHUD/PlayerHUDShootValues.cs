using UnityEngine;
using TMPro;
using System;

public class PlayerHUDShootValues : MonoBehaviour
{
    private ShootController _shootController;

    [SerializeField]
    private TMP_Text _angleText, _forceText;

    public Action<string, string> OnPlayerHudShootValues { get; set; }


    private void Awake()
    {
        _shootController = Get<ShootController>.From(gameObject);
    }

    private void OnEnable()
    {
        if (_shootController != null) _shootController.OnUpdatePlayerHUDValues += OnUpdateValues;
    }

    private void OnDisable()
    {
        if (_shootController != null) _shootController.OnUpdatePlayerHUDValues -= OnUpdateValues;
    }

    private void OnUpdateValues(ShootController.PlayerHUDValues hudValues)
    {
        if (_angleText != null) _angleText.text = Mathf.Round(Mathf.InverseLerp(hudValues._minAngle, hudValues._maxAngle, hudValues._currentAngle) * 100) + "°";
        if (_forceText != null) _forceText.text = Mathf.Round(Mathf.InverseLerp(hudValues._minForce, hudValues._maxForce, hudValues._currentForce) * 100).ToString();

        OnPlayerHudShootValues?.Invoke(_forceText.text, _angleText.text);
    }
}

using System.Collections;
using UnityEngine;

public class CameraVignette : BaseCameraFX, IEndGame
{
    [SerializeField]
    protected PlayerDamageCameraFX _playerDamageCameraFX;

    private float _dmg;


    private void OnEnable() => _playerDamageCameraFX.onDamageFX += OnDamage;

    private void OnDisable() => _playerDamageCameraFX.onDamageFX -= OnDamage;

    private void OnDamage(int damage)
    {
        _dmg += damage;

        StartCoroutine(ChangeAmount((_dmg / 100) / 1.2f <= 0.12f ? 0.12f : (_dmg / 100) / 1.2f, -5));
    }

    private IEnumerator ChangeAmount(float targetValue, float lerp)
    {
        float defaultValue = targetValue;

        yield return StartCoroutine(RunCalculation(defaultValue, lerp));

        _pp.VignetteAmount = defaultValue;
    }

    private IEnumerator RunCalculation(float defaultValue, float lerp)
    {
        while (_pp.VignetteAmount > defaultValue)
        {
            _pp.VignetteAmount += lerp * Time.deltaTime;

            yield return null;
        }
    }

    public void OnGameEnd(object[] data = null)
    {
        _pp.Vignette = false;
    }
}

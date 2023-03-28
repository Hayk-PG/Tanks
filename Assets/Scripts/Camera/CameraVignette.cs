using System.Collections;
using UnityEngine;

public class CameraVignette : BasePlayerDependentCameraEffects<HealthController>, IEndGame
{
    private float _defaultIntensity, _previousIntensity, _currentIntensity, _vignetteTargetIntensity;

    private bool? _isIncrementing;




    private void Awake()
    {
        _defaultIntensity = 0.12f;
        _previousIntensity = 100;
    }

    protected override void Execute()
    {
        ModifyVignetteIntensity(_t.Health);

        _t.OnUpdateHealthBar += ModifyVignetteIntensity;
    }

    private void ModifyVignetteIntensity(int value)
    {
        _previousIntensity = _currentIntensity;
        _currentIntensity = value;
        _vignetteTargetIntensity = _defaultIntensity + (1 - (_currentIntensity / 100));

        _isIncrementing = _currentIntensity < _previousIntensity ? true : _currentIntensity > _previousIntensity ? false : null;

        StartCoroutine(UpdateVignetteIntensity(_isIncrementing, _vignetteTargetIntensity, 3));
    }

    private IEnumerator UpdateVignetteIntensity(bool? isIncrementing, float targetValue, float lerp)
    {
        if (isIncrementing == null)
            yield break;

        if (isIncrementing.Value)
            yield return StartCoroutine(RunIncrement(targetValue, lerp));
        else
            yield return StartCoroutine(RunDecrement(targetValue, lerp));

        _pp.VignetteAmount = targetValue;
    }

    private IEnumerator RunIncrement(float targetValue, float lerp)
    {
        while (_pp.VignetteAmount < targetValue)
        {
            _pp.VignetteAmount += lerp * Time.deltaTime;

            yield return null;
        }
    }

    private IEnumerator RunDecrement(float targetValue, float lerp)
    {
        while (_pp.VignetteAmount > targetValue)
        {
            _pp.VignetteAmount -= lerp * Time.deltaTime;

            yield return null;
        }
    }

    public void OnGameEnd(object[] data = null) => _pp.Vignette = false;

    public void WrapUpGame(object[] data = null)
    {

    }
}

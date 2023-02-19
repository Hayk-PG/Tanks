using System;
using System.Collections;
using UnityEngine;

public class RainManager : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _rain;

    [SerializeField] [Space]
    private bool _isPlaying, _stop;

    public bool IsRaining
    {
        get => _rain.isPlaying;
    }

    public event Action<bool> onRainActivity;


    private void Update()
    {
        if (_isPlaying)
        {
            _rain.Play();

            onRainActivity?.Invoke(true);

            _isPlaying = false;
        }

        if (_stop)
        {
            _rain.Stop();

            onRainActivity?.Invoke(false);

            _stop = false;
        }
    }

    private IEnumerator ControlRainActivity()
    {
        yield return null;
    }
}

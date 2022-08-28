using System.Collections;
using UnityEngine;

public class CameraChromaticAberration : BaseCameraFX
{
    private float _currentDuration;
    private IEnumerator _coroutine;


    public void CameraGlitchFX(float duration)
    {
        float newDuration = _currentDuration + duration;
        _currentDuration = 0;
        CoroutineStop();
        CoroutineStart(newDuration);
    }

    private void CoroutineStart(float duration)
    {
        _coroutine = Coroutine(duration);
        StartCoroutine(_coroutine);
    }

    private void CoroutineStop()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }

    private IEnumerator Coroutine(float duration)
    {
        _currentDuration = duration;
        ChromaticAberration(true);

        while (_currentDuration > 0)
        {
            _currentDuration -= 1 * Time.deltaTime;
            
            if (_currentDuration <= 0)
            {
                _currentDuration = 0;
                ChromaticAberration(false);
            }

            yield return null;
        }
    }

    private void ChromaticAberration(bool isEnabled)
    {
        _pp.ChromaticAberration = isEnabled;
        _pp.Offset = isEnabled ? 5: 0;
        _pp.FishEyeDistortion = isEnabled ? 0.5f : 0;
        _pp.GlitchAmount = isEnabled ? 0.5f : 0;
    }
}

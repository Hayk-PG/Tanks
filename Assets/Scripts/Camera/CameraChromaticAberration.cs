using System.Collections;
using UnityEngine;

public class CameraChromaticAberration : BaseCameraFX
{
    private IEnumerator _coroutine;


    public void CameraGlitchFX(float duration)
    {
        CoroutineStop();

        CoroutineStart(duration);
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
        ChromaticAberration(true);

        yield return new WaitForSeconds(duration);

        ChromaticAberration(false);
    }

    private void ChromaticAberration(bool isEnabled)
    {
        _pp.ChromaticAberration = isEnabled;
        _pp.Offset = isEnabled ? 5: 0;
        _pp.FishEyeDistortion = isEnabled ? 0.5f : 0;
        _pp.GlitchAmount = isEnabled ? 0.5f : 0;
    }
}

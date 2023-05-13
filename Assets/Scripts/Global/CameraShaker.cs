using CartoonFX;
using System.Collections;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    [SerializeField]
    private CFXR_Effect _cfxrEffect;

    private float _shakeDuration;

    

    private void Awake()
    {
        _cfxrEffect.enabled = false;

        _shakeDuration = _cfxrEffect.cameraShake.duration;
    }

    public void ToggleShake()
    {
        SetCfxrEffectEnabled();

        StartCoroutine(DisableCfxrEffectAfterDelay());
    }

    private IEnumerator DisableCfxrEffectAfterDelay()
    {
        yield return new WaitForSeconds(_shakeDuration);

        SetCfxrEffectEnabled(false);
    }

    private void SetCfxrEffectEnabled(bool isEnabled = true) => _cfxrEffect.enabled = isEnabled;
}

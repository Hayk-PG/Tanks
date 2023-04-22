using System.Collections;
using UnityEngine;

public class RipplePostProcessor : MonoBehaviour
{
    [SerializeField]
    private Material _rippleMaterial;

    [SerializeField]
    [Space]
    public float _maxAmount;

    [SerializeField]
    [Range(0, 1)]
    public float _friction;

    private float _amount = 0f;




    private void Awake() => SetRippleMaterialAmount();

    private void OnRenderImage(RenderTexture src, RenderTexture dst) => Graphics.Blit(src, dst, this._rippleMaterial);

    public void ApplyRippleFX(Vector3 pos)
    {
        if (_amount > 0)
            return;

        StartCoroutine(ManageRippleEffectActivity(pos));
    }

    private IEnumerator ManageRippleEffectActivity(Vector3 pos)
    {
        Vector3 ScreenPoint = Camera.main.WorldToScreenPoint(pos);

        _amount = _maxAmount;

        _rippleMaterial.SetFloat("_CenterX", ScreenPoint.x);
        _rippleMaterial.SetFloat("_CenterY", ScreenPoint.y);

        SetRippleMaterialAmount();

        yield return StartCoroutine(ResetRippleEffect());
    }

    private IEnumerator ResetRippleEffect()
    {
        while (_amount >= 0.05f)
        {
            _amount *= _friction;

            SetRippleMaterialAmount();

            yield return null;
        }

        _amount = 0;
    }

    private void SetRippleMaterialAmount() => _rippleMaterial.SetFloat("_Amount", _amount);
}

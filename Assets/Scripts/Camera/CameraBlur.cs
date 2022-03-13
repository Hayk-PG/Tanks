using System.Collections;
using UnityEngine;

public class CameraBlur : BaseCameraFX<CameraBlur>
{
    private IEnumerator _blurCoroutine;

    private bool _isScreenBlurred;


    protected override void Awake()
    {
        base.Awake();

        Singleton(this);
    }

    public static void CameraShakeBlur()
    {
        if(_instance._blurCoroutine == null)
        {
            _instance._blurCoroutine = _instance.BlurCoroutine();
            _instance.StartCoroutine(_instance._blurCoroutine);
        }
    }

    private IEnumerator BlurCoroutine()
    {
        float blurAmount = 0;

        bool isBlurry = false;
        bool isBlurDisabled = false;

        while (!isBlurDisabled)
        {
            if (blurAmount < 1 && !isBlurry)
            {
                blurAmount = Mathf.Lerp(blurAmount, 1, 10 * Time.deltaTime);

                if(blurAmount >= 0.95f)
                {
                    blurAmount = 1;
                    isBlurry = true;
                }
            }

            if(isBlurry)
            {
                blurAmount = Mathf.Lerp(blurAmount, 0, 10 * Time.deltaTime);

                if(blurAmount <= 0.15f)
                {
                    blurAmount = 0;
                    isBlurDisabled = true;
                }
            }

            if(!_isScreenBlurred) _pp.BlurAmount = blurAmount;

            yield return null;
        }

        _instance._blurCoroutine = null;
    }

    public static void ScreenBlur(bool blur)
    {
        if (_instance != null)
        {
            _instance._isScreenBlurred = blur;
            _instance._pp.BlurAmount = blur ? 1 : 0;
        }
    }

    
}


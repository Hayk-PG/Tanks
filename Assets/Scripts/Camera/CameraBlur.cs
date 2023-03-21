using System.Collections;
using UnityEngine;

public class CameraBlur : BaseCameraFX, IEndGame
{
    private bool _isScreenBlurred;


    protected override void Awake() => base.Awake();

    public void CameraShakeBlur() => StartCoroutine(BlurCoroutine());

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
    }

    public void ScreenBlur(bool blur)
    {
        _isScreenBlurred = blur;

        _pp.BlurAmount = blur ? 1 : 0;
    }

    public void OnGameEnd(object[] data = null)
    {
        ScreenBlur(false);
    }

    public void WrapUpGame(object[] data = null)
    {
        
    }
}


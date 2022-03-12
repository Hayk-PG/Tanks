using System.Collections;
using UnityEngine;

public class PPController : MonoBehaviour
{
    private static PPController _instance;

    [SerializeField]
    private MobilePostProcessing _p;

    private IEnumerator _blurCoroutine;


    private void Awake()
    {
        if (_instance == null)
            _instance = this;
    }

    public static void Blur()
    {
        if(_instance._blurCoroutine == null)
        {
            _instance._blurCoroutine = _instance.BlurCoroutine();
            _instance.StartCoroutine(_instance._blurCoroutine);
        }
    }

    private IEnumerator BlurCoroutine()
    {
        float blurAmount = _p.BlurAmount;
        float blurResetTime = 0;

        bool isBlurry = false;
        bool isBlurDisabled = false;

        while (!isBlurDisabled)
        {
            if (blurAmount < 1 && !isBlurry)
            {
                blurAmount = Mathf.Lerp(blurAmount, 1, 10 * Time.deltaTime);
            }

            if(blurAmount >= 0.9f)
            {               
                blurResetTime = Mathf.Lerp(blurResetTime, 1, 10 * Time.deltaTime);
                blurAmount = 1;
                isBlurry = true;
            }

            if (blurResetTime >= 0.95f)
            {
                blurAmount = Mathf.Lerp(blurAmount, 0, 10 * Time.deltaTime);
                blurResetTime = 1;

                if(blurAmount <= 0.1f)
                {
                    isBlurDisabled = true;
                    blurAmount = 0;
                }
            }
                
            _p.BlurAmount = blurAmount;
            yield return null;
        }

        _instance._blurCoroutine = null;
    }
}


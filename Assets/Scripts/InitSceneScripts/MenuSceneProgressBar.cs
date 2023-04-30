using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MenuSceneProgressBar : MonoBehaviour
{
    [SerializeField] [Space]
    private CanvasGroup _canvasGroup;

    [SerializeField] [Space]
    private Image _imgFill;

    [SerializeField] [Space]
    private InitAddressablesValidationChecklist _initAddressablesValidationChecklist;

    private float _progress = 0;
    private float _currentVelocity;




    private void Start() => StartCoroutine(LoadSceneWithProgressBar());
    private void OnEnable()
    {
        _initAddressablesValidationChecklist.onVerifyLoaders += LoadSceneImmediately;
        _initAddressablesValidationChecklist.onCheckValidation += OnCheckValidation;
    }

    private void LoadSceneImmediately() => MyScene.Manager.LoadScene(MyScene.SceneName.Menu);

    private void OnCheckValidation(float value) => _progress += value;

    private IEnumerator LoadSceneWithProgressBar()
    {
        StartCoroutine(ShowProgressBarOnNextFrame());

        while (_imgFill.fillAmount < 1)
        {
            _imgFill.fillAmount = Mathf.SmoothDamp(_imgFill.fillAmount, _progress, ref _currentVelocity, 0.1f, 1f);

            yield return null;
        }

        yield return null;

        MyScene.Manager.LoadScene(MyScene.SceneName.Menu);
    }

    private IEnumerator ShowProgressBarOnNextFrame()
    {
        yield return null;

        GlobalFunctions.CanvasGroupActivity(_canvasGroup, true);
    }
}

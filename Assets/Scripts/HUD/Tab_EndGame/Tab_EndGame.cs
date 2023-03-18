using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using System.Collections;


//ADDRESSABLE
public class Tab_EndGame : MonoBehaviour, IGameOutcomeHandler
{
    [SerializeField]
    private CanvasGroup _canvasGroup;

    [SerializeField] [Space]
    private Image _imgBackground, _imgBackgroundBlue;

    [SerializeField] [Space]
    private AssetReferenceSprite _assetReferenceBackgroundImg, _assetReferenceBlurredBackgroundImg;
    
    [SerializeField] [Space]
    private WinnerLoserIdentifier _winnerLoserIdentifier;

    public IGameOutcomeHandler This { get; set; }
    public IGameOutcomeHandler OperationHandler { get; set; }




    private void Awake() => This = this;

    private void Start()
    {
        InitializeBackgroundImage();
    }

    private void OnEnable() => _winnerLoserIdentifier.onIdentified += OnReultSet;

    private void OnDisable() => _winnerLoserIdentifier.onIdentified -= OnReultSet;

    private void OnReultSet(ScoreController scoreController, bool isWin)
    {
        StartCoroutine(LoadBlurredBackgroundImage());
    }

    private void InitializeBackgroundImage()
    {
        _assetReferenceBackgroundImg.LoadAssetAsync().Completed += background => { _imgBackground.sprite = background.Result; };
    }

    private IEnumerator LoadBlurredBackgroundImage()
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, true);

        yield return new WaitForSeconds(1);

        _assetReferenceBlurredBackgroundImg.LoadAssetAsync().Completed += blurredBackground =>
        {
            BlurBackgroundImage(blurredBackground.Result);

            GameOutcomeHandler.SubmitOperation(this, GameOutcomeHandler.Operation.GameResultTab);
        };
    }

    private void BlurBackgroundImage(Sprite sprite)
    {
        _imgBackground.sprite = sprite;

        _imgBackgroundBlue.gameObject.SetActive(true);
    }

    public void OnSucceed()
    {
        
    }

    public void OnFailed()
    {
        
    }
}

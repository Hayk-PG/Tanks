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

    // If game result is a "Victory", use these sprites"
    [SerializeField] [Space]
    private AssetReferenceSprite _assetReferenceVicotryBgImg, _assetReferenceVictoryBlurredBgImg;

    // If game result is a "Defeat", use these sprites"
    [SerializeField] [Space]
    private AssetReferenceSprite _assetReferenceDefeatBgImg, _assetReferenceDefeatBlurredBgImg;

    private AssetReferenceSprite _currentAssetReferenceBgImg, _currentAssetReferenceBlurredBgImg;

    [SerializeField] [Space]
    private WinnerLoserIdentifier _winnerLoserIdentifier;

    public IGameOutcomeHandler This { get; set; }
    public IGameOutcomeHandler OperationHandler { get; set; }




    private void Awake() => This = this;

    private void OnEnable() => GameOutcomeHandler.onSubmit += OnSubmit;

    private void OnDisable() => GameOutcomeHandler.onSubmit -= OnSubmit;

    private void OnSubmit(IGameOutcomeHandler handler, GameOutcomeHandler.Operation operation, Animator animator, object[] data)
    {
        if (operation != GameOutcomeHandler.Operation.Start)
            return;

        OperationHandler = handler;

        InitializeBackgroundImage((ScoreController)data[0], (bool)data[1]);
    }

    private void InitializeBackgroundImage(ScoreController scoreController, bool isWin)
    {
        _currentAssetReferenceBgImg = isWin ? _assetReferenceVicotryBgImg : _assetReferenceDefeatBgImg;

        _currentAssetReferenceBgImg.LoadAssetAsync().Completed += background => 
        { 
            _imgBackground.sprite = background.Result;

            StartCoroutine(Execute(scoreController, isWin));
        };
    }

    private IEnumerator Execute(ScoreController scoreController, bool isWin)
    {
        yield return StartCoroutine(SetActive(isWin));

        yield return StartCoroutine(Submit(scoreController, isWin));
    }

    private IEnumerator SetActive(bool isWin)
    {
        yield return new WaitForSeconds(5);

        _currentAssetReferenceBlurredBgImg = isWin ? _assetReferenceVictoryBlurredBgImg : _assetReferenceDefeatBlurredBgImg;

        GlobalFunctions.CanvasGroupActivity(_canvasGroup, true);

        GameOutcomeHandler.SubmitOperation(this, GameOutcomeHandler.Operation.WrapUp);
    }

    private IEnumerator Submit(ScoreController scoreController, bool isWin)
    {
        yield return new WaitForSeconds(2);

        _currentAssetReferenceBlurredBgImg.LoadAssetAsync().Completed += blurredBackground =>
        {
            _imgBackground.sprite = blurredBackground.Result;

            _imgBackgroundBlue.gameObject.SetActive(true);

            GameOutcomeHandler.SubmitOperation(this, GameOutcomeHandler.Operation.GameResultTab, new object[] { scoreController, isWin });
        };
    }

    public void OnSucceed()
    {
        
    }

    public void OnFailed()
    {
        
    }
}

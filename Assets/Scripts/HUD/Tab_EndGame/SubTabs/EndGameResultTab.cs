using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class EndGameResultTab : BaseEndGameSubTab
{
    [SerializeField] [Space]
    private TMP_Text _txtVictory, _txtDefeat;
    
    [SerializeField] [Space]
    private Image _imgLine;

    [SerializeField] [Space]
    private GameObject _objTrophy, _objSkull;

    private const string _gameResultTabAnim = "GameResultTabAnim";
    private const string _gameResultPositionAnim = "GameResultPositionAnim";




    protected override void OnSubmit(IGameOutcomeHandler handler, GameOutcomeHandler.Operation operation, Animator animator, object[] data)
    {
        if (operation != GameOutcomeHandler.Operation.GameResultTab)
            return;

        OperationHandler = handler;

        GetData(data);

        InitializeAnimator(animator);

        SetActive();

        DisplayGameResult();
    }

    protected override void SetActive()
    {
        _animator?.Play(_gameResultTabAnim, 0, 0);
    }

    private void DisplayGameResult()
    {
        Conditions<bool>.Compare((bool)_data[1], 

            delegate 
            {
                ModifyGUIElements(_objTrophy, _txtVictory);
            }, 
            delegate 
            {
                ModifyGUIElements(_objSkull, _txtDefeat);
            });
    }

    private void ModifyGUIElements(GameObject obj, TMP_Text tMP_Text)
    {
        obj.SetActive(true);

        tMP_Text.gameObject.SetActive(true);

        _imgLine.color = tMP_Text.colorGradient.topLeft;
    }

    // Used as an animation event callback.
    public void Move()
    {
        _animator?.Play(_gameResultPositionAnim, 1, 0);
    }

    // Used as an animation event callback.
    public override void SubmitOperation()
    {
        GameOutcomeHandler.SubmitOperation(this, GameOutcomeHandler.Operation.ScoresTab, _data);
    }

    public override void OnSucceed()
    {
        
    }

    public override void OnFailed()
    {
        
    }
}

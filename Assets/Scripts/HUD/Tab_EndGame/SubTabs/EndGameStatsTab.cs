using UnityEngine;

public class EndGameStatsTab : BaseEndGameSubTab
{
    private const string _statsTabAnim = "StatsTabAnim";


    protected override void OnSubmit(IGameOutcomeHandler handler, GameOutcomeHandler.Operation operation, Animator animator, object[] data)
    {
        if (operation != GameOutcomeHandler.Operation.StatsTab)
            return;

        OperationHandler = handler;

        InitializeAnimator(animator);

        SetActive();
    }

    protected override void SetActive()
    {
        _animator?.Play(_statsTabAnim, 4, 0);
    }

    public override void SubmitOperation()
    {
        GameOutcomeHandler.SubmitOperation(this, GameOutcomeHandler.Operation.ItemsTab);
    }

    public override void OnSucceed()
    {
        
    }

    public override void OnFailed()
    {
        
    }
}

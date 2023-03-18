using UnityEngine;

public class EndGameScoresTab : BaseEndGameSubTab
{
    private const string _scoresTabAnim = "ScoresTabAnim";
    private const string _scoresTabPositionAnim = "ScoresTabPositionAnim";
    

    
    protected override void OnSubmit(IGameOutcomeHandler handler, GameOutcomeHandler.Operation operation, Animator animator, object[] data)
    {
        if (operation != GameOutcomeHandler.Operation.ScoresTab)
            return;

        OperationHandler = handler;

        InitializeAnimator(animator);

        SetActive();
    }

    protected override void SetActive()
    {
        _animator?.Play(_scoresTabAnim, 2, 0);
    }

    public void Move()
    {
        _animator?.Play(_scoresTabPositionAnim, 3, 0);
    }

    public override void SubmitOperation()
    {
        GameOutcomeHandler.SubmitOperation(this, GameOutcomeHandler.Operation.StatsTab);
    }

    public override void OnSucceed()
    {

    }

    public override void OnFailed()
    {
        
    }
}

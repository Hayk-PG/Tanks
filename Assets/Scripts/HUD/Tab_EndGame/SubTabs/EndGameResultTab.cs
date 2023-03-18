using UnityEngine;

public class EndGameResultTab : BaseEndGameSubTab
{
    private const string _gameResultTabAnim = "GameResultTabAnim";
    private const string _gameResultPositionAnim = "GameResultPositionAnim";




    protected override void OnSubmit(IGameOutcomeHandler handler, GameOutcomeHandler.Operation operation, Animator animator, object[] data)
    {
        if (operation != GameOutcomeHandler.Operation.GameResultTab)
            return;

        OperationHandler = handler;

        InitializeAnimator(animator);

        SetActive();
    }

    protected override void SetActive()
    {
        //print(_animator.GetCurrentAnimatorStateInfo(0).tagHash);

        _animator?.Play(_gameResultTabAnim, 0, 0);
    }

    // Used as an animation event callback.
    public void Move()
    {
        _animator?.Play(_gameResultPositionAnim, 1, 0);

        print("aaa");
    }

    // Used as an animation event callback.
    public override void SubmitOperation()
    {
        GameOutcomeHandler.SubmitOperation(this, GameOutcomeHandler.Operation.ScoresTab);
    }

    public override void OnSucceed()
    {
        
    }

    public override void OnFailed()
    {
        
    }
}

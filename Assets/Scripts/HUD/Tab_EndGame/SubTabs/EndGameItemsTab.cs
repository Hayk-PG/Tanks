using UnityEngine;

public class EndGameItemsTab : BaseEndGameSubTab
{
    private const string _itemsTabAnim = "ItemsTabAnim";


    protected override void OnSubmit(IGameOutcomeHandler handler, GameOutcomeHandler.Operation operation, Animator animator, object[] data)
    {
        if (operation != GameOutcomeHandler.Operation.ItemsTab)
            return;

        OperationHandler = handler;

        InitializeAnimator(animator);

        SetActive();
    }

    protected override void SetActive()
    {
        _animator?.Play(_itemsTabAnim, 5, 0);
    }

    public override void SubmitOperation()
    {
        
    }

    public override void OnSucceed()
    {
        
    }

    public override void OnFailed()
    {
        
    }
}

using System.Collections;
using UnityEngine;

public class EndGameItemsTab : BaseEndGameSubTab
{
    [SerializeField] [Space]
    private EndGameItemsGroup[] _itemsGroupds;

    private const string _itemsTabAnim = "ItemsTabAnim";


    protected override void OnSubmit(IGameOutcomeHandler handler, GameOutcomeHandler.Operation operation, Animator animator, object[] data)
    {
        if (operation != GameOutcomeHandler.Operation.ItemsTab)
            return;

        OperationHandler = handler;

        GetData(data);

        InitializeAnimator(animator);

        SetActive();
    }

    protected override void SetActive()
    {
        _animator?.Play(_itemsTabAnim, 5, 0);

        StartCoroutine(RunIteration());
    }

    private IEnumerator RunIteration()
    {
        for (int i = 0; i < _itemsGroupds.Length; i++)
            yield return StartCoroutine(InitializeItemGroup(_itemsGroupds[i]));

        yield return new WaitForSeconds(1);

        SubmitOperation();
    }

    private IEnumerator InitializeItemGroup(EndGameItemsGroup itemGroup)
    {
        while (itemGroup.CanvasGroup.alpha < 1)
        {
            itemGroup.CanvasGroup.alpha += 10 * Time.deltaTime;

            if (itemGroup.CanvasGroup.alpha >= 1)
                itemGroup.CanvasGroup.interactable = true;

            yield return null;
        }
    }

    public override void SubmitOperation()
    {
        GameOutcomeHandler.SubmitOperation(this, GameOutcomeHandler.Operation.UIShiny);
    }

    public override void OnSucceed()
    {
        
    }

    public override void OnFailed()
    {
        
    }
}

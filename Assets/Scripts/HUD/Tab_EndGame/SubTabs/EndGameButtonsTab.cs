using System.Collections;
using UnityEngine;

public class EndGameButtonsTab : BaseEndGameSubTab
{
    [SerializeField] [Space]
    private Btn[] _btns;

    protected override void OnSubmit(IGameOutcomeHandler handler, GameOutcomeHandler.Operation operation, Animator animator, object[] data)
    {
        if (operation != GameOutcomeHandler.Operation.ButtonsTab)
            return;

        OperationHandler = handler;

        SetActive();
    }

    protected override void SetActive()
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, true);

        StartCoroutine(RunIteration());
    }

    private IEnumerator RunIteration()
    {
        //for (int i = 0; i < _btns.Length; i++)
        //{
        //    yield return StartCoroutine(ActivateButton(_btns[i]));
        //}

        yield return new WaitForSeconds(1);

        SubmitOperation();
    }

    private IEnumerator ActivateButton(Btn btn)
    {
        yield return new WaitForSeconds(0.5f);

        btn.gameObject.SetActive(true);
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

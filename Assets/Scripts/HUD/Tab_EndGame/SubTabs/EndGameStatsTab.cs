using System.Collections;
using UnityEngine;

public class EndGameStatsTab : BaseEndGameSubTab
{
    [SerializeField] [Space]
    private EndGameStatsGroup[] _statsGroups;

    private const string _statsTabAnim = "StatsTabAnim";



    protected override void OnSubmit(IGameOutcomeHandler handler, GameOutcomeHandler.Operation operation, Animator animator, object[] data)
    {
        if (operation != GameOutcomeHandler.Operation.StatsTab)
            return;

        OperationHandler = handler;

        GetData(data);

        InitializeAnimator(animator);

        SetActive();
    }

    protected override void SetActive()
    {
        _animator?.Play(_statsTabAnim, 4, 0);
    }

    public override void SubmitOperation()
    {
        StartCoroutine(RunIteration());
    }

    private IEnumerator RunIteration()
    {
        for (int i = 0; i < _statsGroups.Length; i++)
            yield return StartCoroutine(InitializeStatsGroup(_statsGroups[i]));

        GameOutcomeHandler.SubmitOperation(this, GameOutcomeHandler.Operation.ItemsTab);
    }

    private IEnumerator InitializeStatsGroup(EndGameStatsGroup statsGroup)
    {
        while (statsGroup.CanvasGroup.alpha < 1)
        {
            statsGroup.CanvasGroup.alpha += 10 * Time.deltaTime;

            if (statsGroup.CanvasGroup.alpha >= 1)
                statsGroup.CanvasGroup.interactable = true;

            yield return null;
        }
    }

    public override void OnSucceed()
    {
        
    }

    public override void OnFailed()
    {
        
    }
}

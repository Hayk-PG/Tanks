using UnityEngine;

public class EndGameAnimationsEventsCallbacks : MonoBehaviour
{
    [SerializeField]
    private EndGameResultTab _resultTab;

    [SerializeField] [Space]
    private EndGameScoresTab _scoresTab;

    [SerializeField] [Space]
    private EndGameStatsTab _statsTab;




    // Used as an animation event callback.
    public void InvokeResultTabAnimFirstEvent() => _resultTab.Move();

    // Used as an animation event callback.
    public void InvokeResultTabAnimSecondEvent() => _resultTab.SubmitOperation();

    // Used as an animation event callback.
    public void InvokeScoresTabAnimFirstEvent() => _scoresTab.Move();

    // Used as an animation event callback.
    public void InvokeScoresTabAnimSecondEvent() => _scoresTab.SubmitOperation();

    // Used as an animation event callback.
    public void InvokeStatsTabAnimEvent() => _statsTab.SubmitOperation();
}

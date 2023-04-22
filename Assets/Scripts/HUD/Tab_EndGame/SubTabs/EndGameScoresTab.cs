using UnityEngine;
using TMPro;
using System.Collections;

public class EndGameScoresTab : BaseEndGameSubTab
{
    [SerializeField] [Space]
    private TMP_Text _txtScore;

    private const string _scoresTabAnim = "ScoresTabAnim";
    private const string _scoresTabPositionAnim = "ScoresTabPositionAnim";
    

    
    protected override void OnSubmit(IGameOutcomeHandler handler, GameOutcomeHandler.Operation operation, Animator animator, object[] data)
    {
        if (operation != GameOutcomeHandler.Operation.ScoresTab)
            return;

        OperationHandler = handler;

        GetData(data);

        InitializeAnimator(animator);

        SetActive();
    }

    protected override void SetActive()
    {
        _animator?.Play(_scoresTabAnim, 2, 0);
    }

    // Used as an animation event callback.
    public void DisplayFinalScore()
    {
        StartCoroutine(DisplayFinalScoreCoroutine());
    }

    private IEnumerator DisplayFinalScoreCoroutine()
    {
        _txtScore.text = "0";

        ScoreController scoreController = (ScoreController)_data[0];

        float score = 0;
        float playPoints = 100;
        float _experiencePoints = 200;
        float winPoints = (bool)_data[1] ? 400 : 0;
        float newScore = scoreController.MainScore + playPoints + _experiencePoints + winPoints;

        while (score < newScore)
        {
            score += (Mathf.RoundToInt(newScore) / 5) * Time.deltaTime;

            if (score > newScore)
                score = newScore;

            _txtScore.text = $"{score:0}";

            yield return null;
        }

        yield return null;

        _animator?.Play(_scoresTabPositionAnim, 3, 0);
    }

    // Used as an animation event callback.
    public override void SubmitOperation()
    {
        GameOutcomeHandler.SubmitOperation(this, GameOutcomeHandler.Operation.StatsTab, _data);
    }

    public override void OnSucceed()
    {

    }

    public override void OnFailed()
    {
        
    }
}

using UnityEngine;
using Coffee.UIEffects;
using System.Collections;

public class EndGameShiny : MonoBehaviour,IGameOutcomeHandler
{
    [SerializeField]
    private WinnerLoserIdentifier _winnerLoserIdentifier;

    private UIShiny[] _uIShinies;

    public IGameOutcomeHandler This { get; set; }
    public IGameOutcomeHandler OperationHandler { get; set; }




    private void Awake() => This = this;

    private void OnEnable() => GameOutcomeHandler.onSubmit += OnSubmit;

    private void OnDisable() => GameOutcomeHandler.onSubmit -= OnSubmit;

    private void OnSubmit(IGameOutcomeHandler handler, GameOutcomeHandler.Operation operation, Animator animator, object[] data)
    {
        if (operation == GameOutcomeHandler.Operation.Start)
            _uIShinies = GetComponentsInChildren<UIShiny>(true);

        if (operation == GameOutcomeHandler.Operation.UIShiny)
        {
            OperationHandler = handler;

            StartCoroutine(PlayUIShinyEffect());

            GameOutcomeHandler.SubmitOperation(this, GameOutcomeHandler.Operation.MenuScene);
        }
    }

    private IEnumerator PlayUIShinyEffect()
    {
        GlobalFunctions.Loop<UIShiny>.Foreach(_uIShinies, ush => { ush.effectFactor = 0; });

        yield return null;

        GlobalFunctions.Loop<UIShiny>.Foreach(_uIShinies, ush => { ush.Play(); });
    }

    public void OnSucceed()
    {
        
    }

    public void OnFailed()
    {
        
    }
}

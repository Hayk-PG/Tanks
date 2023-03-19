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

    private void OnEnable()
    {
        _winnerLoserIdentifier.onIdentified += delegate { InitializeUIShinies(); };

        GameOutcomeHandler.onSubmit += OnSubmit;
    }

    private void OnDisable()
    {
        _winnerLoserIdentifier.onIdentified -= delegate { InitializeUIShinies(); };

        GameOutcomeHandler.onSubmit -= OnSubmit;
    }

    private void InitializeUIShinies() => _uIShinies = GetComponentsInChildren<UIShiny>(true);

    private void OnSubmit(IGameOutcomeHandler handler, GameOutcomeHandler.Operation operation, Animator animator, object[] data)
    {
        if (operation != GameOutcomeHandler.Operation.UIShiny)
            return;

        OperationHandler = handler;

        StartCoroutine(PlayUIShinyEffect());

        GameOutcomeHandler.SubmitOperation(this, GameOutcomeHandler.Operation.MenuScene);
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

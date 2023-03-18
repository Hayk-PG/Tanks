using UnityEngine;

public abstract class BaseEndGameSubTab : MonoBehaviour, IGameOutcomeHandler
{
    [SerializeField]
    protected CanvasGroup _canvasGroup;

    protected Animator _animator;

    public IGameOutcomeHandler This { get; set; }
    public IGameOutcomeHandler OperationHandler { get; set; }

    


    protected virtual void Awake() => This = this;

    protected virtual void OnEnable() => GameOutcomeHandler.onSubmit += OnSubmit;

    protected virtual void OnDisable() => GameOutcomeHandler.onSubmit -= OnSubmit;

    protected abstract void OnSubmit(IGameOutcomeHandler handler, GameOutcomeHandler.Operation operation, Animator animator, object[] data);

    protected virtual void InitializeAnimator(Animator animator)
    {
        if (_animator != null)
            return;

        _animator = animator;
    }

    protected abstract void SetActive();

    public abstract void SubmitOperation();

    //IGameOutcomeHandler Callback
    public abstract void OnSucceed();

    //IGameOutcomeHandler Callback
    public abstract void OnFailed();
}

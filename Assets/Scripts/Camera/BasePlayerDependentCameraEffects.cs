
public abstract class BasePlayerDependentCameraEffects<T> : BaseCameraFX
{
    protected T _t;

    protected virtual void OnEnable() => GameSceneObjectsReferences.GameManager.OnGameStarted += Initialize;

    protected virtual void Initialize() => GetT();

    protected virtual void GetT()
    {
        _t = GlobalFunctions.ObjectsOfType<TankController>.Find(tk => tk.BasePlayer != null).GetComponent<T>();

        if (_t == null)
            return;

        Execute();
    }

    protected abstract void Execute();
}

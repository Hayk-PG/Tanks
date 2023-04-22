using Photon.Pun;

public abstract class BaseStartGame : MonoBehaviourPun
{
    protected GameManager _gameManager;


    protected virtual void Awake() => _gameManager = Get<GameManager>.From(gameObject);

    protected virtual void Update() => Conditions<bool>.Compare(CanStartGame(), StartGame, null);

    protected abstract void StartGame();

    protected abstract bool CanStartGame();
}

using Photon.Pun;

public class BasePlayer : MonoBehaviourPun
{
    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

    protected virtual void AssignGameObjectName(string name)
    {
        this.name = name;
    }
    protected virtual void PlayerReady(int playerIndex)
    {
        if (playerIndex == 0)
            _gameManager.MasterPlayerReady = true;
        else
            _gameManager.SecondPlayerReady = true;
    }
}

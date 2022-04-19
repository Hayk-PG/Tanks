using Photon.Pun;

public class BasePlayer : MonoBehaviourPun
{  
    protected virtual void AssignGameObjectName(string name)
    {
        this.name = name;
    }
    protected virtual void PlayerReady(int playerIndex)
    {
        if (playerIndex == 0)
            GameManager.MasterPlayerReady = true;
        else
            GameManager.SecondPlayerReady = true;
    }
}

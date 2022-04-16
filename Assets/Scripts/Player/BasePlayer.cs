using UnityEngine;

public class BasePlayer : MonoBehaviour
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

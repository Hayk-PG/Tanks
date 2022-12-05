using UnityEngine;

public class LobbyAvailability : MonoBehaviour, IReset
{
    private LobbyRequirements _lobbyRequirements;
    private LobbyLock _lobbyLock;


    private void Awake()
    {
        _lobbyRequirements = Get<LobbyRequirements>.From(gameObject);
        _lobbyLock = Get<LobbyLock>.FromChild(gameObject);
    }

    public void SetDefault()
    {
        int requiredCoins = _lobbyRequirements.RequiredQuantityOfCoins;
        int requiredMasters = _lobbyRequirements.RequiredQuantityOfMasters;
        int requiredStrengths = _lobbyRequirements.RequiredQuantityOfStrengths;

        if (Data.Manager.Coins >= requiredCoins && Data.Manager.Masters >= requiredMasters && Data.Manager.Strengths >= requiredStrengths)
            _lobbyLock.SetActivity(false);
        else
            _lobbyLock.SetActivity(true);
    }
}

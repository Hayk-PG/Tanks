using System;
using UnityEngine;

public class LobbyRequirements : MonoBehaviour
{
    [SerializeField] private LobbyItem[] _lobbyItemsEntryFee;
    [SerializeField] private LobbyItem[] _lobbyItemsWin;
    [SerializeField] private LobbyItem[] _lobbyItemsLose;

    public int RequiredQuantityOfCoins { get; private set; }
    public int RequiredQuantityOfMasters { get; private set; }
    public int RequiredQuantityOfStrengths { get; private set; }



    private void Start()
    {
        GetRequiredItemsTotal();
    }

    private void CalculateRequiredItemTotal(LobbyItem[] lobbyItems, string key, Action<int> onTotal)
    {
        int total = 0;

        for (int i = 0; i < lobbyItems.Length; i++)
        {
            if (lobbyItems[i].Type == key)
                total += Math.Abs(lobbyItems[i].Quantity);
        }

        onTotal?.Invoke(total);
    }

    public void GetRequiredItemsTotal()
    {
        CalculateRequiredItemTotal(_lobbyItemsEntryFee, Keys.ItemCoins, total => { RequiredQuantityOfCoins += total; });
        CalculateRequiredItemTotal(_lobbyItemsLose, Keys.ItemCoins, total => { RequiredQuantityOfCoins += total; });
        CalculateRequiredItemTotal(_lobbyItemsEntryFee, Keys.ItemMaster, total => { RequiredQuantityOfMasters += total; });
        CalculateRequiredItemTotal(_lobbyItemsLose, Keys.ItemMaster, total => { RequiredQuantityOfMasters += total; });
        CalculateRequiredItemTotal(_lobbyItemsEntryFee, Keys.ItemStrength, total => { RequiredQuantityOfStrengths += total; });
        CalculateRequiredItemTotal(_lobbyItemsLose, Keys.ItemStrength, total => { RequiredQuantityOfStrengths += total; });
    }
}

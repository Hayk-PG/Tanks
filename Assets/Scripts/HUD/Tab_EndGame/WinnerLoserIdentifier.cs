using System;
using UnityEngine;

public class WinnerLoserIdentifier : MonoBehaviour
{
    private BaseEndGame _baseEndGame;

    public event Action<ScoreController, bool> onIdentified;


    private void Awake() => _baseEndGame = MyPhotonNetwork.IsOfflineMode ? FindObjectOfType<EndOfflineGame>() : FindObjectOfType<EndOnlineGame>();

    private void OnEnable() => _baseEndGame.OnEndGameTab += GetGameResult;

    private void OnDisable() => _baseEndGame.OnEndGameTab -= GetGameResult;

    private void GetGameResult(string successedPlayerName, string defeatedPlayerName)
    {
        GameObject successedTank = GameObject.Find(successedPlayerName);
        GameObject defeatedTank = GameObject.Find(defeatedPlayerName);

        if (successedTank != null && defeatedTank != null)
        {
            DetermineWinner(successedTank);
            DetermineLoser(defeatedTank);
        }
    }

    private bool IsLocalPlayer(TankController tankController)
    {
        return tankController?.BasePlayer != null;
    }

    private void Process(GameObject tank, bool isWin)
    {
        ScoreController scoreController = Get<ScoreController>.From(tank);
        onIdentified?.Invoke(scoreController, isWin);
    }

    private void DetermineWinner(GameObject successedTank)
    {
        if (IsLocalPlayer(Get<TankController>.From(successedTank)))
            Process(successedTank, true);
    }

    private void DetermineLoser(GameObject defeatedTank)
    {
        if (IsLocalPlayer(Get<TankController>.From(defeatedTank)))
            Process(defeatedTank, false);
    }
}
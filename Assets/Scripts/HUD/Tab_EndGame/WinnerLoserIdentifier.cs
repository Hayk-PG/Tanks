using System;
using UnityEngine;

public class WinnerLoserIdentifier : MonoBehaviour,IGameOutcomeHandler
{
    public IGameOutcomeHandler This { get; set; }
    public IGameOutcomeHandler OperationHandler { get; set; }




    private void Awake() => This = this;

    private void OnEnable() => GameSceneObjectsReferences.BaseEndGame.onEndGame += GetGameResult;

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

    private bool IsLocalPlayer(TankController tankController)
    {
        return tankController?.BasePlayer != null;
    }

    private void Process(GameObject tank, bool isWin)
    {
        ScoreController scoreController = Get<ScoreController>.From(tank);

        GameOutcomeHandler.SubmitOperation(this, GameOutcomeHandler.Operation.Start, new object[] { scoreController, isWin });
    }

    public void OnSucceed()
    {
        
    }

    public void OnFailed()
    {
        
    }
}
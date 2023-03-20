using System.Collections;
using UnityEngine;

public class EndGameCleanupHandler : MonoBehaviour
{
    [SerializeField]
    private WinnerLoserIdentifier _winnerLoserIdentifier;



    private void OnEnable() => GameOutcomeHandler.onSubmit += OnSubmit;

    private void OnDisable() => GameOutcomeHandler.onSubmit -= OnSubmit;

    private void OnSubmit(IGameOutcomeHandler handler, GameOutcomeHandler.Operation operation, Animator animator, object[] data)
    {
        if (operation == GameOutcomeHandler.Operation.Start)
            DeactivateDefeatedTank((ScoreController)data[0], (bool)data[1]);

        if (operation == GameOutcomeHandler.Operation.CleanUp)
            DeactivateGameobjects();

        if (operation == GameOutcomeHandler.Operation.MenuScene)
            StartCoroutine(FinishGame());
    }

    private void DeactivateDefeatedTank(ScoreController sc, bool isWin)
    {
        if (isWin)
            GlobalFunctions.ObjectsOfType<ScoreController>.Find(s => s != sc).gameObject.SetActive(false);
        else
            sc.gameObject.SetActive(false);
    }

    private void DeactivateGameobjects()
    {
        DeactivateAllTanks();
    }

    private void DeactivateAllTanks() => GlobalFunctions.Loop<TankController>.Foreach(FindObjectsOfType<TankController>(), tk => { tk.gameObject.SetActive(false); });

    private IEnumerator FinishGame()
    {
        yield return new WaitForSeconds(5);

        if (MyPhotonNetwork.IsOfflineMode)
            PopupManager.Instance?.PrepareGameResultPopup();

        MyScene.Manager.LoadScene(MyScene.SceneName.Menu);
    }
}

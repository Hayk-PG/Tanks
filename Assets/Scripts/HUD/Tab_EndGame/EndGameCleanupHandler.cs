using System.Collections;
using UnityEngine;

public class EndGameCleanupHandler : MonoBehaviour
{
    [SerializeField]
    private WinnerLoserIdentifier _winnerLoserIdentifier;



    private void OnEnable()
    {
        _winnerLoserIdentifier.onIdentified += DeactivateGameobjects;

        GameOutcomeHandler.onSubmit += OnSubmit;
    }

    private void OnDisable()
    {
        _winnerLoserIdentifier.onIdentified -= DeactivateGameobjects;

        GameOutcomeHandler.onSubmit -= OnSubmit;
    }

    private void DeactivateGameobjects(ScoreController sc, bool isWin)
    {
        DeactivateTanks();
    }

    private void DeactivateTanks() => GlobalFunctions.Loop<TankController>.Foreach(FindObjectsOfType<TankController>(), tk => { tk.gameObject.SetActive(false); });

    private void OnSubmit(IGameOutcomeHandler handler, GameOutcomeHandler.Operation operation, Animator animator, object[] data)
    {
        if (operation != GameOutcomeHandler.Operation.MenuScene)
            return;

        StartCoroutine(FinishGame());
    }

    private IEnumerator FinishGame()
    {
        yield return new WaitForSeconds(5);

        if (MyPhotonNetwork.IsOfflineMode)
            PopupManager.Instance?.PrepareGameResultPopup();

        MyScene.Manager.LoadScene(MyScene.SceneName.Menu);
    }
}

using System.Collections;
using UnityEngine;
using System.Linq;

public class EndGameCleanupHandler : MonoBehaviour
{
    [SerializeField]
    private WinnerLoserIdentifier _winnerLoserIdentifier;

    private IEndGame[] _iEndGames;




    private void Awake() => _iEndGames = FindObjectsOfType<MonoBehaviour>().OfType<IEndGame>().ToArray();

    private void OnEnable() => GameOutcomeHandler.onSubmit += OnSubmit;

    private void OnDisable() => GameOutcomeHandler.onSubmit -= OnSubmit;

    private void OnSubmit(IGameOutcomeHandler handler, GameOutcomeHandler.Operation operation, Animator animator, object[] data)
    {
        if (operation == GameOutcomeHandler.Operation.Start)
            GlobalFunctions.Loop<IEndGame>.Foreach(FindObjectsOfType<MonoBehaviour>().OfType<IEndGame>().ToArray(), iEndGame => { iEndGame.OnGameEnd(); });

        if (operation == GameOutcomeHandler.Operation.WrapUp)
            GlobalFunctions.Loop<IEndGame>.Foreach(FindObjectsOfType<MonoBehaviour>().OfType<IEndGame>().ToArray(), iEndGame => { iEndGame.WrapUpGame(); });

        if (operation == GameOutcomeHandler.Operation.MenuScene)
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

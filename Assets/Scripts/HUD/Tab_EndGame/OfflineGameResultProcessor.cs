using UnityEngine;


public class OfflineGameResultProcessor : MonoBehaviour
{
    [SerializeField]
    private WinnerLoserIdentifier _winnerLoserIndentifier;



    private void OnEnable() => _winnerLoserIndentifier.onIdentified += OnWinnerLoserIdentified;

    private void OnDisable() => _winnerLoserIndentifier.onIdentified += OnWinnerLoserIdentified;

    private void OnWinnerLoserIdentified(ScoreController scoreController, bool isWin)
    {
        PrepareGameResultPopup(isWin);

        MyScene.Manager.LoadScene(MyScene.SceneName.Menu);
    }

    private void PrepareGameResultPopup(bool isWin)
    {
        if (PopupManager.Instance == null)
            return;

        PopupManager.Instance.PrepareGameResultPopup(isWin);
    }
}

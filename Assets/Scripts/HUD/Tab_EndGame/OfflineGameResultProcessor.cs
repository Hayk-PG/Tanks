using System.Collections;
using UnityEngine;


public class OfflineGameResultProcessor : MonoBehaviour
{
    [SerializeField]
    private WinnerLoserIdentifier _winnerLoserIndentifier;



    private void OnEnable() => _winnerLoserIndentifier.onIdentified += OnWinnerLoserIdentified;

    private void OnDisable() => _winnerLoserIndentifier.onIdentified += OnWinnerLoserIdentified;

    private void OnWinnerLoserIdentified(ScoreController scoreController, bool isWin)
    {
        StartCoroutine(PrepareGameResultPopup(isWin));
    }

    private IEnumerator PrepareGameResultPopup(bool isWin)
    {
        PopupManager.Instance?.PrepareGameResultPopup(isWin);

        yield return new WaitForSeconds(1);

        MyScene.Manager.LoadScene(MyScene.SceneName.Menu);
    }
}

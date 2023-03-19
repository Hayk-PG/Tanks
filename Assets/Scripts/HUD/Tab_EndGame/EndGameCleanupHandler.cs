using UnityEngine;

public class EndGameCleanupHandler : MonoBehaviour
{
    [SerializeField]
    private WinnerLoserIdentifier _winnerLoserIdentifier;



    private void OnEnable() => _winnerLoserIdentifier.onIdentified += DeactivateGameobjects;

    private void OnDisable() => _winnerLoserIdentifier.onIdentified -= DeactivateGameobjects;

    private void DeactivateGameobjects(ScoreController sc, bool isWin)
    {
        DeactivateTanks();
    }

    private void DeactivateTanks() => GlobalFunctions.Loop<TankController>.Foreach(FindObjectsOfType<TankController>(), tk => { tk.gameObject.SetActive(false); });
}

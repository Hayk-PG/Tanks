using UnityEngine;

public class DropBoxItemSelectionPanelOwner : MonoBehaviour
{
    private ScoreController _localPlayerScoreController;



    private void OnEnable() => GameSceneObjectsReferences.GameManager.OnGameStarted += OnGameStarted;

    private void OnDisable() => GameSceneObjectsReferences.GameManager.OnGameStarted -= OnGameStarted;

    private void OnGameStarted()
    {
        TankController tankController = GlobalFunctions.ObjectsOfType<TankController>.Find(tc => tc.BasePlayer != null);

        if (tankController == null)
            return;

        _localPlayerScoreController = Get<ScoreController>.From(tankController.gameObject);
    }

    public bool HasEnoughPoints(int price)
    {
        return _localPlayerScoreController?.Score >= price;
    }
}

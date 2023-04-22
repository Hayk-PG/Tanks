using UnityEngine;

public class ScreenText : MonoBehaviour
{
    private HealthController _playerHealthController;
    private ScoreController _scoreController;




    private void OnEnable() => GameSceneObjectsReferences.GameManager.OnGameStarted += OnGameStarted;

    private void OnGameStarted()
    {
        TankController localTank = GlobalFunctions.ObjectsOfType<TankController>.Find(tank => tank.BasePlayer != null);

        if (localTank == null)
            return;

        _playerHealthController = Get<HealthController>.From(localTank.gameObject);

        _scoreController = Get<ScoreController>.From(localTank.gameObject);

        _playerHealthController.OnTakeDamage += OnTakeDamage;

        _scoreController.OnHitEnemy += OnGetPoints;
    }

    private void OnTakeDamage(BasePlayer basePlayer, int damage) => SecondarySoundController.PlaySound(0, 0);

    private void OnGetPoints(int[] scoreValues) => SecondarySoundController.PlaySound(0, 1);
}

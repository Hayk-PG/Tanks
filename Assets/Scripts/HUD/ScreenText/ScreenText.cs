using UnityEngine;

public class ScreenText : MonoBehaviour
{
    private PlayerDamageScreenText _playerDamageScreenText;
    private EnemyHitScreenText _enemyHitScreenText;

    private HealthController _playerHealthController;
    private ScoreController _scoreController;

    private string _onGetPointsText;


    private void Awake()
    {
        _playerDamageScreenText = Get<PlayerDamageScreenText>.FromChild(gameObject);
        _enemyHitScreenText = Get<EnemyHitScreenText>.FromChild(gameObject);
    }

    private void OnDisable()
    {
        if (_playerHealthController != null) _playerHealthController.OnTakeDamage -= OnTakeDamage;
        if (_scoreController != null) _scoreController.OnHitEnemy -= OnGetPoints; 
    }

    public void CallPlayerEvents(HealthController playerHealth, ScoreController scoreController)
    {
        _playerHealthController = playerHealth;
        _scoreController = scoreController;

        if (_playerHealthController != null) _playerHealthController.OnTakeDamage += OnTakeDamage;
        if (_scoreController != null) _scoreController.OnHitEnemy += OnGetPoints;
    }

    private void OnTakeDamage(BasePlayer basePlayer, int damage)
    {
        if (basePlayer != null && _playerDamageScreenText != null)
            _playerDamageScreenText.Display(-damage);
    }

    private void OnGetPoints(int[] scoreValues)
    {
        _onGetPointsText = "+" + scoreValues[0] + " (Hit)" + "\n" + "+" + scoreValues[1] + " (Bonus)";
        _enemyHitScreenText.Display(_onGetPointsText);
    }
}

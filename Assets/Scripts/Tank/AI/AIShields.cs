
public class AIShields : PlayerShields
{
    private ScoreController _scoreController;

    private HealthController _healthController;




    private void Awake()
    {
        _scoreController = Get<ScoreController>.From(gameObject);

        _healthController = Get<HealthController>.From(gameObject);
    }

    public void ActivateShield()
    {
        if (_scoreController.Score < 1200 || _healthController.Health > 60)
            return;

        ActivateShields(1);
    }
}

public class BoostZoneXp : BaseBoostZoneFeatures
{
    private ScoreController _scoreController;


    protected override void OnTriggerEnter(TankController tankController)
    {
        _scoreController = Get<ScoreController>.From(tankController.gameObject);
        _scoreController?.BoostXp(true);
    }

    protected override void OnTriggerExit(TankController tankController)
    {
        _scoreController?.BoostXp(false);
        _scoreController = null;
    }
}

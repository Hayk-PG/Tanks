public class PlayVsPlayer : BasePlayVs
{
    private void OnEnable()
    {
        _tabStartGame.OnStartPlayVsOtherPlayer += OnStartPlayVsOtherPlayer;
    }

    private void OnDisable()
    {
        _tabStartGame.OnStartPlayVsOtherPlayer -= OnStartPlayVsOtherPlayer;
    }

    private void OnStartPlayVsOtherPlayer()
    {
        MyPhoton.StartConnection();
    }
}

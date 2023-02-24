using Photon.Pun;

public class PhotonNetworkWeatherManager : MonoBehaviourPun
{
    public void SetCurrentWeatherActivity(bool isRaining, bool isSnowing)
    {
        photonView.RPC("SetCurrentWeatherActivityRPC", RpcTarget.AllViaServer, isRaining, isSnowing);
    }

    [PunRPC]
    private void SetCurrentWeatherActivityRPC(bool isRaining, bool isSnowing)
    {
        GameSceneObjectsReferences.WeatherManager.SetCurrentWeatherActivity(isRaining, isSnowing);
    }

    public void RaiseWeatherActivity(bool isRaining, bool isSnowing)
    {
        photonView.RPC("RaiseWeatherActivityRPC", RpcTarget.AllViaServer, isRaining, isSnowing);
    }

    [PunRPC]
    private void RaiseWeatherActivityRPC(bool isRaining, bool isSnowing)
    {
        GameSceneObjectsReferences.WeatherManager.RaiseWeatherActivity(isRaining, isSnowing);
    }
}

using Photon.Pun;

public class PhotonNetworkWeatherManager : MonoBehaviourPun
{
    public void RaiseWeatherActivity()
    {
        photonView.RPC("RaiseWeatherActivityRPC", RpcTarget.AllViaServer);
    }

    [PunRPC]
    private void RaiseWeatherActivityRPC()
    {
        GameSceneObjectsReferences.WeatherManager.RaiseWeatherActivity();
    }

    public void ChangeWeather()
    {
        photonView.RPC("ChangeWeatherRPC", RpcTarget.AllViaServer);
    }

    [PunRPC]
    private void ChangeWeatherRPC()
    {
        GameSceneObjectsReferences.WeatherManager.ChangeWeather();
    }
}

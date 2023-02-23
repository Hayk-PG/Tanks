using Photon.Pun;

public class PhotonNetworkWeatherManager : MonoBehaviourPun
{
    public void RaiseWeatherActivity()
    {
        photonView.RPC("RaiseWeatherActivityRPC", RpcTarget.Others);
    }

    [PunRPC]
    private void RaiseWeatherActivityRPC()
    {
        GameSceneObjectsReferences.WeatherManager.RaiseWeatherActivity();
    }

    public void ChangeWeather()
    {
        photonView.RPC("ChangeWeatherRPC", RpcTarget.Others);
    }

    [PunRPC]
    private void ChangeWeatherRPC()
    {
        GameSceneObjectsReferences.WeatherManager.ChangeWeather();
    }
}

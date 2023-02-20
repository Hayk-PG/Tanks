using Photon.Pun;

public class WeatherManagerSerializeView : BaseGameManagerSerializeView
{
    protected override void Read(PhotonStream stream, PhotonMessageInfo info)
    {
        GameSceneObjectsReferences.WeatherManager.IsRaining = (bool)stream.ReceiveNext();
        GameSceneObjectsReferences.WeatherManager.IsSnowing = (bool)stream.ReceiveNext();
    }

    protected override void Write(PhotonStream stream)
    {
        stream.SendNext(GameSceneObjectsReferences.WeatherManager.IsRaining);
        stream.SendNext(GameSceneObjectsReferences.WeatherManager.IsSnowing);
    }
}

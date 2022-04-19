using Photon.Pun;

public partial class NetworkSettings : MonoBehaviourPun
{
    private MyPhotonCallbacks _myPhotonCallbacks;


    private void Awake()
    {
        _myPhotonCallbacks = Get<MyPhotonCallbacks>.From(gameObject);
    }

    private void OnEnable()
    {
        _myPhotonCallbacks._OnConnectedToMaster += OnConnectedToMaster;
    }

    private void OnDisable()
    {
        _myPhotonCallbacks._OnConnectedToMaster -= OnConnectedToMaster;
    }

    private void OnConnectedToMaster()
    {
        PhotonNetwork.RunRpcCoroutines = true;
    }
}

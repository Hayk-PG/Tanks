using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;

public class WoodenBoxSpawner : MonoBehaviour
{
    [SerializeField] private ParachuteWithWoodBoxController _parachuteWithWoodBoxController;
    private InstantiatePickables _instantiatePickables;
    private WoodenBoxSerializer _woodenBoxSerializer;


    private void Awake()
    {
        _instantiatePickables = FindObjectOfType<InstantiatePickables>();
        _woodenBoxSerializer = FindObjectOfType<WoodenBoxSerializer>();
    }

    private void OnEnable()
    {
        if (MyPhotonNetwork.IsOfflineMode)
            _instantiatePickables.OnInstantiateWoodenBox += OnInstantiateWoodenBox;
        else
            PhotonNetwork.NetworkingClient.EventReceived += OnInstantiateWoodenBox;
    }

    private void OnDisable()
    {
        if (MyPhotonNetwork.IsOfflineMode)
            _instantiatePickables.OnInstantiateWoodenBox -= OnInstantiateWoodenBox;
        else
            PhotonNetwork.NetworkingClient.EventReceived -= OnInstantiateWoodenBox;
    }

    private void Instantiate(object[] data) 
    {
        ParachuteWithWoodBoxController parachute = Instantiate(_parachuteWithWoodBoxController, (Vector3)data[0], Quaternion.identity);
        parachute.RandomContent = (int)data[1];
        _woodenBoxSerializer.ParachuteWithWoodBoxController = parachute;
    }

    //ONLINE
    private void OnInstantiateWoodenBox(EventData data)
    {
        if(data.Code == EventInfo.Code_InstantiateWoodenBox)
        {
            object[] datas = (object[])data.CustomData;
            Instantiate(datas);
        }
    }

    //OFFLINE
    private void OnInstantiateWoodenBox(object[] content)
    {
        Instantiate(content);
    }
}

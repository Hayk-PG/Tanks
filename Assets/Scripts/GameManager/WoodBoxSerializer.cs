using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class WoodBoxSerializer : MonoBehaviourPun
{
    [SerializeField] private ParachuteWithWoodBoxController _parachuteWithWoodBoxController;
    [SerializeField] private WoodBox _woodBox;

    public ParachuteWithWoodBoxController ParachuteWithWoodBoxController
    {
        get => _parachuteWithWoodBoxController;
        set => _parachuteWithWoodBoxController = value;
    }
    public WoodBox WoodBox
    {
        get => _woodBox;
        set => _woodBox = value;
    }

    public System.Action<object[]> OnBoxTriggerEnteredSerializer { get; set; }



    public void AllocateWoodBoxController()
    {
        if (MyPhotonNetwork.IsMasterClient(MyPhotonNetwork.LocalPlayer))
            photonView.RPC("AllocateWoodBoxControllerRPC", RpcTarget.AllViaServer);
    }

    [PunRPC]
    private void AllocateWoodBoxControllerRPC()
    {
        ParachuteWithWoodBoxController = FindObjectOfType<ParachuteWithWoodBoxController>();
        ParachuteWithWoodBoxController.SetInitValues();
    }

    public void AllocateWoodBox()
    {
        if (MyPhotonNetwork.IsMasterClient(MyPhotonNetwork.LocalPlayer))
            photonView.RPC("AllocateWoodBoxRPC", RpcTarget.AllViaServer);
    }

    [PunRPC]
    private void AllocateWoodBoxRPC()
    {
        WoodBox = FindObjectOfType<WoodBox>();
        WoodBox.SetInitValues();
    }

    public void DestroyParachuteWithWoodBoxController()
    {
        if (MyPhotonNetwork.IsMasterClient(MyPhotonNetwork.LocalPlayer))
        {
            photonView.RPC("DestroyRPC", RpcTarget.AllViaServer);
        }
    }

    [PunRPC]
    private void DestroyRPC()
    {
        if(ParachuteWithWoodBoxController != null)
        {
            ParachuteWithWoodBoxController.Explosion();
            Destroy(ParachuteWithWoodBoxController.gameObject);
        }
    }

    public void BoxTriggerEntered(Collider other)
    {
        if (MyPhotonNetwork.IsOfflineMode || !MyPhotonNetwork.IsOfflineMode && MyPhotonNetwork.IsMasterClient(MyPhotonNetwork.LocalPlayer))
        {
            EventInfo.Content_WoodBoxTriggerEntered = new object[]
            {
            other.transform.position
            };

            //ONLINE
            PhotonNetwork.RaiseEvent(EventInfo.Code_WoodBoxTriggerEntered, EventInfo.Content_WoodBoxTriggerEntered, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
            //OFFLINE
            OnBoxTriggerEnteredSerializer?.Invoke(EventInfo.Content_WoodBoxTriggerEntered);
        }
    }
}

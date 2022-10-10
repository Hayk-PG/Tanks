using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class WoodenBoxSerializer : MonoBehaviourPun
{
    [SerializeField] ParachuteWithWoodBoxController _parachuteWithWoodBoxController;
    public ParachuteWithWoodBoxController ParachuteWithWoodBoxController
    {
        get => _parachuteWithWoodBoxController;
        set => _parachuteWithWoodBoxController = value;
    }


    public System.Action<object[]> OnBoxTriggerEnteredSerializer { get; set; }


    public void DestroyParachuteWithWoodBoxController()
    {
        if (MyPhotonNetwork.AmPhotonViewOwner(photonView))
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
        if (MyPhotonNetwork.IsOfflineMode || !MyPhotonNetwork.IsOfflineMode && MyPhotonNetwork.AmPhotonViewOwner(photonView))
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

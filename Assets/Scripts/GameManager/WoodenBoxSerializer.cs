using Photon.Pun;
using UnityEngine;

public class WoodenBoxSerializer : MonoBehaviourPun
{
    [SerializeField] ParachuteWithWoodBoxController _parachuteWithWoodBoxController;
    public ParachuteWithWoodBoxController ParachuteWithWoodBoxController
    {
        get => _parachuteWithWoodBoxController;
        set => _parachuteWithWoodBoxController = value;
    }



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
}

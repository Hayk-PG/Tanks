using Photon.Pun;
using UnityEngine;

public class WoodBoxSerializer : MonoBehaviourPun
{
    [SerializeField] 
    private ParachuteWithWoodBoxController _parachuteWithWoodBoxController;

    public ParachuteWithWoodBoxController ParachuteWithWoodBoxController
    {
        get => _parachuteWithWoodBoxController;
        set => _parachuteWithWoodBoxController = value;
    }



    public void DestroyParachuteWithWoodBoxController(Vector3 id)
    {
        if (MyPhotonNetwork.IsMasterClient(MyPhotonNetwork.LocalPlayer))
            photonView.RPC("DestroyParachuteWithWoodBoxControllerRPC", RpcTarget.AllViaServer, id);
    }

    [PunRPC]
    private void DestroyParachuteWithWoodBoxControllerRPC(Vector3 id)
    {
        ParachuteWithWoodBoxController parachuteWithWoodBoxController = GlobalFunctions.ObjectsOfType<ParachuteWithWoodBoxController>.Find(p => p.Id == id);

        if (parachuteWithWoodBoxController == null)
            return;

        parachuteWithWoodBoxController.DestroyGameobject();
    }
}

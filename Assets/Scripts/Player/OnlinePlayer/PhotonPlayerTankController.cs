using UnityEngine;

public class PhotonPlayerTankController : BasePlayerTankController<TankController>
{
    private PhotonPlayerController _photonPlayerController;

    internal TankMovement _tankMovement;
    internal Rigidbody _tankRigidbody;


    private void Awake()
    {
        _photonPlayerController = Get<PhotonPlayerController>.From(gameObject);
    }

    public override void GetControl(TankController tank)
    {
        OwnTank = tank;
        Conditions<bool>.Compare(photonView.IsMine, OnPhotonViewIsMine, null);

        _tankMovement = OwnTank?.GetComponent<TankMovement>();
        _tankRigidbody = OwnTank?.GetComponent<Rigidbody>();
    }

    private void OnPhotonViewIsMine()
    {
        OwnTank.GetTankControl(_photonPlayerController);
    }
}

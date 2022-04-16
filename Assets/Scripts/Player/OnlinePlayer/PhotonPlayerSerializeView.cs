﻿using Photon.Pun;
using UnityEngine;

public class PhotonPlayerSerializeView : MonoBehaviourPun, IPunObservable
{
    private PhotonPlayerTankController _photonPlayerTankController;


    private void Awake()
    {
        _photonPlayerTankController = Get<PhotonPlayerTankController>.From(gameObject);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            if (_photonPlayerTankController._tankMovement != null)
                stream.SendNext(_photonPlayerTankController._tankMovement.Direction);

            if (_photonPlayerTankController._tankRigidbody != null)
            {
                stream.SendNext(_photonPlayerTankController._tankRigidbody.position);
                stream.SendNext(_photonPlayerTankController._tankRigidbody.rotation);
            }
        }
        else
        {
            if (_photonPlayerTankController._tankMovement != null)
                _photonPlayerTankController._tankMovement.Direction = (float)stream.ReceiveNext();

            if (_photonPlayerTankController._tankRigidbody != null)
            {
                _photonPlayerTankController._tankRigidbody.position = (Vector3)stream.ReceiveNext();
                _photonPlayerTankController._tankRigidbody.rotation = (Quaternion)stream.ReceiveNext();
            }
        }
    }
}
﻿using Photon.Pun;
using UnityEngine;

public class PhotonPlayerSerializeView : MonoBehaviourPun, IPunObservable
{
    private PhotonPlayerTankController _photonPlayerTankController;
    private float _lag;



    private void Awake()
    {
        _photonPlayerTankController = Get<PhotonPlayerTankController>.From(gameObject);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            if (_photonPlayerTankController._tankMovement != null)
            {
                stream.SendNext(_photonPlayerTankController._tankMovement.Direction);
                stream.SendNext(_photonPlayerTankController._tankMovement.SynchedPosition);
                stream.SendNext(_photonPlayerTankController._tankMovement.SynchedRotation);
            }

            if (_photonPlayerTankController._shootController != null)
            {
                stream.SendNext(_photonPlayerTankController._shootController.Direction);
                stream.SendNext(_photonPlayerTankController._shootController.ActiveAmmoIndex);
                stream.SendNext(_photonPlayerTankController._shootController.CanonPivotPointEulerAngles);
                stream.SendNext(_photonPlayerTankController._shootController.CurrentForce);
                stream.SendNext(_photonPlayerTankController._shootController.IsApplyingForce);
            }

            if(_photonPlayerTankController._playerAmmoType != null)
            {
                stream.SendNext(_photonPlayerTankController._playerAmmoType.WeaponsBulletsCount);
            }

            if (_photonPlayerTankController._playerTurn != null)
            {
                stream.SendNext(_photonPlayerTankController._playerTurn.IsMyTurn);
            }

            if(_photonPlayerTankController._healthController != null)
            {
                stream.SendNext(_photonPlayerTankController._healthController.Health);
                stream.SendNext(_photonPlayerTankController._healthController.IsSafeZone);
            }

            if (_photonPlayerTankController._scoreController != null)
            {
                stream.SendNext(_photonPlayerTankController._scoreController.IsXpBoost);
            }

            if (_photonPlayerTankController._healthBar != null)
            {
                stream.SendNext(_photonPlayerTankController._healthBar.Value);
            }
        }
        else
        {
            if (_photonPlayerTankController._tankMovement != null)
            {
                _photonPlayerTankController._tankMovement.Direction = (float)stream.ReceiveNext();
                _photonPlayerTankController._tankMovement.SynchedPosition = (Vector3)stream.ReceiveNext();
                _photonPlayerTankController._tankMovement.SynchedRotation = (Quaternion)stream.ReceiveNext();
            }         

            if (_photonPlayerTankController._shootController != null)
            {
                _photonPlayerTankController._shootController.Direction = (float)stream.ReceiveNext();
                _photonPlayerTankController._shootController.ActiveAmmoIndex = (int)stream.ReceiveNext();
                _photonPlayerTankController._shootController.CanonPivotPointEulerAngles = (Vector3)stream.ReceiveNext();
                _photonPlayerTankController._shootController.CurrentForce = (float)stream.ReceiveNext();
                _photonPlayerTankController._shootController.IsApplyingForce = (bool)stream.ReceiveNext();
            }

            if(_photonPlayerTankController._playerAmmoType != null)
            {
                _photonPlayerTankController._playerAmmoType.WeaponsBulletsCount = (int[])stream.ReceiveNext();
            }

            if (_photonPlayerTankController._playerTurn != null)
            {
                _photonPlayerTankController._playerTurn.IsMyTurn = (bool)stream.ReceiveNext();
            }

            if(_photonPlayerTankController._healthController != null)
            {
                _photonPlayerTankController._healthController.Health = (int)stream.ReceiveNext();
                _photonPlayerTankController._healthController.IsSafeZone = (bool)stream.ReceiveNext();
            }

            if(_photonPlayerTankController._scoreController != null)
            {
                _photonPlayerTankController._scoreController.IsXpBoost = (bool)stream.ReceiveNext();
            }

            if(_photonPlayerTankController._healthBar != null)
            {
                _photonPlayerTankController._healthBar.Value = (float)stream.ReceiveNext();
            }
        }
    }
}

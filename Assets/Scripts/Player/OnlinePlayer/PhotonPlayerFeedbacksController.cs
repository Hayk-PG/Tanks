using System;
using UnityEngine;
using Photon.Pun;

public class PhotonPlayerFeedbacksController : PlayerFeedbackController
{
    [SerializeField]
    private PhotonPlayerController _photonPlayerController;

    protected override bool IsAllowed => _playerController.OwnTank?.BasePlayer != null;




    protected override void OnHitEnemy(int[] scores, Func<int> hitsCount)
    {
        // This method is called on the local side 

        _playerFeedback.OnHitEnemy(_playerController.OwnTank.name, hitsCount(), scores);

        // This method is called on the server side
        // And should not be called on the local side

        int hc = hitsCount();

        _photonPlayerController.PhotonView.RPC("OnHitEnemyRPC", RpcTarget.Others, _playerController.OwnTank.name, scores, hc);
    }

    [PunRPC]
    private void OnHitEnemyRPC(string tankName, int[] scores, int hitsCount)
    {
        _playerFeedback.OnHitEnemy(tankName, hitsCount, scores);
    }

    protected override void OnPlayerWeaponChanged(AmmoTypeButton ammoTypeButton)
    {
        // This method is called on the local side 

        _playerFeedback.DisplayWeaponChangeText(_playerController.OwnTank.name, ammoTypeButton._properties.Name);

        // This method is called on the server side
        // And should not be called on the local side

        _photonPlayerController.PhotonView.RPC("OnPlayerWeaponChangedRPC", RpcTarget.Others, _playerController.OwnTank.name, ammoTypeButton._properties.Name);
    }

    [PunRPC]
    private void OnPlayerWeaponChangedRPC(string tankName, string weaponType)
    {
        _playerFeedback.DisplayWeaponChangeText(tankName, weaponType);
    }

    protected override void OnTakeDamage(BasePlayer basePlayer, int damage)
    {
        // This method is called on the local side

        if (damage > 0)
            _playerFeedback.DisplayDamageText(_playerController.OwnTank.name, damage);

        // This method is called on the server side
        // And should not be called on the local side

        if (damage > 0)
            _photonPlayerController.PhotonView.RPC("OnTakeDamageRPC", RpcTarget.Others, _playerController.OwnTank.name, damage);
    }

    [PunRPC]
    private void OnTakeDamageRPC(string tankName, int damage)
    {
        _playerFeedback.DisplayDamageText(tankName, damage);
    }
}

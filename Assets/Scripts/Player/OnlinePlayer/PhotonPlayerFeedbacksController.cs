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
        _photonPlayerController.PhotonView.RPC("OnHitEnemyRPC", RpcTarget.AllViaServer, _playerController.OwnTank.name, scores, hitsCount());
    }

    protected override void OnPlayerWeaponChanged(AmmoTypeButton ammoTypeButton)
    {
        _photonPlayerController.PhotonView.RPC("OnPlayerWeaponChangedRPC", RpcTarget.AllViaServer, _playerController.OwnTank.name, ammoTypeButton._properties.WeaponType);
    }

    protected override void OnTakeDamage(BasePlayer basePlayer, int damage)
    {
        if (damage > 0)
            _photonPlayerController.PhotonView.RPC("OnTakeDamageRPC", RpcTarget.AllViaServer, _playerController.OwnTank.name, damage);
    }

    [PunRPC]
    private void OnHitEnemyRPC(string tankName, int[] scores, Func<int> hitsCount)
    {
        _playerFeedback.OnHitEnemy(tankName, hitsCount(), scores);
    }

    [PunRPC]
    private void OnPlayerWeaponChangedRPC(string tankName, string weaponType)
    {
        _playerFeedback.DisplayWeaponChangeText(tankName, weaponType);
    }

    [PunRPC]
    private void OnTakeDamageRPC(string tankName, int damage)
    {
        _playerFeedback.DisplayDamageText(tankName, damage);
    }
}

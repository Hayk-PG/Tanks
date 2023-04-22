using System.Collections;
using UnityEngine;

public class OfflinePlayerBomberCaller : MonoBehaviour
{
    [SerializeField]
    protected BasePlayerTankController<BasePlayer> _playerTankController;



    protected virtual void OnEnable() => GameSceneObjectsReferences.BaseRemoteControlTarget.onRemoteControlTarget += OnRemoteControlTarget;

    protected virtual void OnDisable() => GameSceneObjectsReferences.BaseRemoteControlTarget.onRemoteControlTarget -= OnRemoteControlTarget;

    protected virtual void OnRemoteControlTarget(BaseRemoteControlTarget.Mode mode, object[] data)
    {
        if (!IsAllowed(mode))
            return;

        StartCoroutine(Execute(WaitUntil(), data));
    }

    protected virtual bool IsAllowed(BaseRemoteControlTarget.Mode mode)
    {
        return mode == BaseRemoteControlTarget.Mode.Bomber;
    }

    protected virtual IEnumerator Execute(IEnumerator waitUntil, object[] data)
    {
        yield return StartCoroutine(waitUntil);

        DeductScores((int)data[1]);

        CallBomber((BomberType)data[0], (Vector3)data[3]);
    }

    protected virtual IEnumerator WaitUntil()
    {
        yield return new WaitUntil(() => _playerTankController._playerTurn.IsMyTurn);
    }

    protected virtual void DeductScores(int price)
    {
        _playerTankController._scoreController.GetScore(price, null);
    }

    protected virtual void CallBomber(BomberType bomberType, Vector3 dropPosition)
    {
        GameSceneObjectsReferences.AirSupport.Call(_playerTankController._playerTurn, _playerTankController._iScore, dropPosition);
    }
}

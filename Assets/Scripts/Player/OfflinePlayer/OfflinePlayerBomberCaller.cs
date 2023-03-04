using System.Collections;
using UnityEngine;

public class OfflinePlayerBomberCaller : MonoBehaviour
{
    [SerializeField]
    protected BasePlayerTankController<BasePlayer> _playerTankController;

    protected virtual bool IsAllowed { get; } = true;



    protected virtual void OnEnable()
    {
        GameSceneObjectsReferences.BaseRemoteControlTarget.onBomberTargetSet += OnRemoteControlTargetSet;
    }

    protected virtual void OnDisable()
    {
        GameSceneObjectsReferences.BaseRemoteControlTarget.onBomberTargetSet -= OnRemoteControlTargetSet;
    }

    protected virtual void OnRemoteControlTargetSet(object[] targetData)
    {
        if (!IsAllowed)
            return;

        StartCoroutine(Execute(WaitUntil(), targetData));
    }

    protected virtual IEnumerator Execute(IEnumerator waitUntil, object[] targetData)
    {
        yield return StartCoroutine(waitUntil);

        DeductScores((int)targetData[1]);

        CallBomber((BomberType)targetData[0], (Vector3)targetData[3]);
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

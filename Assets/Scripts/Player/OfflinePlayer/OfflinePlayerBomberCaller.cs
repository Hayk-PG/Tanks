using System.Collections;
using UnityEngine;

public class OfflinePlayerBomberCaller : MonoBehaviour
{
    [SerializeField]
    protected BasePlayerTankController<BasePlayer> _playerTankController;

    protected virtual bool IsAllowed { get; } = true;



    protected virtual void OnEnable()
    {
        GameSceneObjectsReferences.BaseRemoteControlTarget.onBomberTargetSet += OnBomberTargetSet;
    }

    protected virtual void OnDisable()
    {
        GameSceneObjectsReferences.BaseRemoteControlTarget.onBomberTargetSet -= OnBomberTargetSet;
    }

    protected virtual void OnBomberTargetSet(object[] targetData)
    {
        if (!IsAllowed)
            return;

        StartCoroutine(Execute(targetData));
    }

    protected virtual IEnumerator Execute(object[] targetData)
    {
        yield return new WaitUntil(() => _playerTankController._playerTurn.IsMyTurn);

        CallBomber((BomberType)targetData[0], (Vector3)targetData[3]);

        DeductScores((int)targetData[1]);
    }

    protected virtual void CallBomber(BomberType bomberType, Vector3 dropPosition)
    {
        GameSceneObjectsReferences.AirSupport.Call(_playerTankController._playerTurn, _playerTankController._iScore, dropPosition);
    }

    protected virtual void DeductScores(int price)
    {
        _playerTankController._scoreController.GetScore(price, null);
    }
}

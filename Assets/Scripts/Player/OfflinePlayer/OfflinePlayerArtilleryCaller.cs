using System.Collections;
using UnityEngine;

public class OfflinePlayerArtilleryCaller : OfflinePlayerBomberCaller
{
    protected override void OnEnable()
    {
        GameSceneObjectsReferences.BaseRemoteControlTarget.onArtilleryTargetSet += OnRemoteControlTargetSet;
    }

    protected override void OnDisable()
    {
        GameSceneObjectsReferences.BaseRemoteControlTarget.onArtilleryTargetSet -= OnRemoteControlTargetSet;
    }

    protected override IEnumerator Execute(IEnumerator waitUntil, object[] targetData)
    {
        yield return StartCoroutine(waitUntil);

        DeductScores((int)targetData[1]);

        CallArtillery(targetData);
    }

    protected virtual void CallArtillery(object[] targetData)
    {
        GameSceneObjectsReferences.ArtillerySupport.Call(Data(targetData));
    }

    protected virtual object[] Data(object[] targetData)
    {
        return new object[]
        {
            _playerTankController._playerTurn,
            _playerTankController._iScore,

            RandomShellSpreadValues((int)targetData[2], (float)targetData[0]),

            (Vector3)targetData[3]
        };
    }

    protected virtual float[] RandomShellSpreadValues(int shellsCount, float spreadValue)
    {
        float[] randomShellsSpreadValues = new float[shellsCount];

        for (int i = 0; i < randomShellsSpreadValues.Length; i++)
        {
            randomShellsSpreadValues[i] = Random.Range(-spreadValue, spreadValue);
        }

        return randomShellsSpreadValues;
    }
}

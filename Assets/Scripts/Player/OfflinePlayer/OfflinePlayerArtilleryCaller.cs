using System.Collections;
using UnityEngine;

public class OfflinePlayerArtilleryCaller : OfflinePlayerBomberCaller
{
    protected override bool IsAllowed(BaseRemoteControlTarget.Mode mode)
    {
        return mode == BaseRemoteControlTarget.Mode.Artillery;
    }

    protected override IEnumerator Execute(IEnumerator waitUntil, object[] data)
    {
        yield return StartCoroutine(waitUntil);

        DeductScores((int)data[1]);

        CallArtillery(data);
    }

    protected virtual void CallArtillery(object[] data)
    {
        GameSceneObjectsReferences.ArtillerySupport.Call(Data(data));
    }

    protected virtual object[] Data(object[] data)
    {
        return new object[]
        {
            _playerTankController._playerTurn,
            _playerTankController._iScore,

            RandomShellSpreadValues((int)data[2], (float)data[0]),

            (Vector3)data[3]
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

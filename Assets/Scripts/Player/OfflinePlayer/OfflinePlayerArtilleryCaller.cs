using System.Collections;
using System.Collections.Generic;
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

    }
}

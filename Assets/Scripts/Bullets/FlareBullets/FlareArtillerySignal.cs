using UnityEngine;

public class FlareArtillerySignal : FlareSignal<ArtillerySupport>
{
    protected override void OnFlareSignal(IScore ownerScore, PlayerTurn ownerTurn, Vector3 point)
    {
        _signalReceiver.Call(point, ownerScore, ownerTurn);
    }
}

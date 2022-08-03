using UnityEngine;

public class FlareBomberSignal : FlareSignal<AirSupport>
{
    protected override void OnFlareSignal(IScore ownerScore, PlayerTurn ownerTurn, Vector3 point)
    {
        _signalReceiver.Call(out Bomber bomber, ownerTurn, ownerScore, point);
    }
}

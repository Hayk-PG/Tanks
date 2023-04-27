using System;
using UnityEngine;

public enum BuffDebuffType { Xp2, Xp3, Ability }

public class BuffDebuffHandler : MonoBehaviour
{
    public static event Action<BuffDebuffType, TurnState, IBuffDebuffUIElementController, object[]> onBuffDebuffIndicatorActivity;




    public static void RaiseEvent(BuffDebuffType buffDebuffType, TurnState turnState, IBuffDebuffUIElementController buffDebuffUIElement = null, object[] data = null)
    {
        onBuffDebuffIndicatorActivity?.Invoke(buffDebuffType, turnState, buffDebuffUIElement, data);
    }
}

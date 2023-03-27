using System;
using UnityEngine;

public enum BuffDebuffType { Xp2, Xp3 }

public class BuffDebuffHandler : MonoBehaviour
{
    public static event Action<BuffDebuffType, TurnState, object[]> onBuffDebuffIndicatorActivity;




    public static void RaiseEvent(BuffDebuffType buffDebuffType, TurnState turnState, object[] data = null)
    {
        onBuffDebuffIndicatorActivity?.Invoke(buffDebuffType, turnState, data);
    }
}

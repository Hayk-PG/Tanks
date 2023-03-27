using System;
using UnityEngine;

public enum BuffDebuffType { Xp2, Xp3 }

public class BuffDebuffHandler : MonoBehaviour
{
    public static event Action<BuffDebuffType, int, object[]> onBuffDebuffIndicatorActivity;




    public static void RaiseEvent(BuffDebuffType buffDebuffType, int hudComponentIndex, object[] data = null)
    {
        onBuffDebuffIndicatorActivity?.Invoke(buffDebuffType, hudComponentIndex, data);
    }
}

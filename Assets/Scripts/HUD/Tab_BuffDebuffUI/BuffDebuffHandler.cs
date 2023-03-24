using System;
using UnityEngine;

public class BuffDebuffHandler : MonoBehaviour
{
    public enum BuffDebuffType { DoubleXp}

    public static event Action<IBuffDebuffHandler, BuffDebuffType, object[]> onActivateBuffDebuffIcon;




    public static void RaiseEvent(IBuffDebuffHandler buffDebuffHandler, BuffDebuffType buffDebuffType, object[] data = null)
    {
        onActivateBuffDebuffIcon?.Invoke(buffDebuffHandler, buffDebuffType, data);
    }
}

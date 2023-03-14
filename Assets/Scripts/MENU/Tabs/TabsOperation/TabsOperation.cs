using System;
using UnityEngine;

public class TabsOperation : MonoBehaviour
{
    public static TabsOperation Handler { get; private set; }

    public enum Operation { Authenticate, Start, PlayOnline, PlayOffline, UserProfile, UserStats}

    public event Action<ITabOperation, Operation, object[]> onOperationSubmitted;




    private void Awake() => Handler = this;

    public void SubmitOperation(ITabOperation handler, Operation operation, object[] data = null)
    {
        onOperationSubmitted?.Invoke(handler, operation, data);
    }
}

using UnityEngine;

public class BaseTab_Tanks<T> : Tab_Base<T> where T: MonoBehaviour
{
    public CustomScrollRect CustomScrollRect { get; protected set; }

    protected override void Awake()
    {
        base.Awake();

        CustomScrollRect = Get<CustomScrollRect>.FromChild(gameObject);
    }
}

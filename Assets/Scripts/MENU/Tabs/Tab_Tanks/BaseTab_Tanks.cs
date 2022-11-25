public class BaseTab_Tanks: Tab_Base  
{
    public CustomScrollRect CustomScrollRect { get; protected set; }

    protected override void Awake()
    {
        base.Awake();
        CustomScrollRect = Get<CustomScrollRect>.FromChild(gameObject);
    }
}

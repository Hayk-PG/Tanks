public class BaseTab_Tanks: Tab_Base
{
    protected ITab_Base _previousTab;
    
    public CustomScrollRect CustomScrollRect { get; protected set; }



    protected override void Awake()
    {
        base.Awake();
        CustomScrollRect = Get<CustomScrollRect>.FromChild(gameObject);
    }

    protected virtual void CachePreviousTab(ITab_Base previousTab)
    {
        _previousTab = previousTab;
        base.OpenTab();
    }
}

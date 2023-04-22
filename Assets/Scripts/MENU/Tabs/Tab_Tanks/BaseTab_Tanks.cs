using UnityEngine;

public class BaseTab_Tanks: Tab_Base
{
    [SerializeField] [Space]
    protected SubTabUnlock _subTabUnlock;

    [SerializeField] [Space]
    protected SubTabRepair _subTabRepair;

    protected ITab_Base _previousTab;
    
    public CustomScrollRect CustomScrollRect { get; protected set; }

    public SubTabUnlock SubTabUnlock => _subTabUnlock;

    public SubTabRepair SubTabRepair => _subTabRepair;




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

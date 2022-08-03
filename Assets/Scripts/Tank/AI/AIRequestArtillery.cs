public class AIRequestArtillery : AISupportAndProps
{
    private ArtillerySupport _artillerySupport;


    private new void Awake()
    {
        base.Awake();
        _artillerySupport = FindObjectOfType<ArtillerySupport>();
    }

    private void Start()
    {
        CacheRelatedTypeButton(Names.LightMortarSupport);
    }

    protected override void OnUse()
    {
        //_artillerySupport.Call(_player.position, _iScore);
    }
}

public class AIRequestBomber : AISupportAndProps
{
    private AirSupport _airSupport;    
    private Bomber _bomber;
    private bool _isBomberCalled;
    private bool _hasBombDropped;
    private bool IsTimeToDropBomb
    {
        get
        {
            return _bomber.transform.position.x <= _player.position.x + 0.1f && _bomber.transform.position.x >= _player.position.x - 0.1f;
        }
    }

      
    protected override void Awake()
    {
        base.Awake();
        _airSupport = FindObjectOfType<AirSupport>();
    }

    private void Start()
    {
        CacheRelatedTypeButton(Names.AirSupport);
    }

    private void Update()
    {
        if (_isBomberCalled && _bomber != null && !_hasBombDropped && IsTimeToDropBomb)
        {
            //_bomber.DropBomb(_iScore);
            _hasBombDropped = true;
            _isBomberCalled = false;
        }
    }

    protected override void OnUse()
    {
        //_airSupport.Call(out Bomber bomber, _playerTurn);
        //_bomber = bomber;

        _isBomberCalled = true;
        _hasBombDropped = false;
    }
}

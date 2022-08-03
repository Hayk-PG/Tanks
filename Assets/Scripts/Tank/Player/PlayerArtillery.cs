using UnityEngine;

public class PlayerArtillery : PlayerDeployProps
{
    private IScore _iScore;
    private RemoteControlArtilleryTarget _remoteControlArtilleryTargets;
    private ArtillerySupport _artillerySupport;


    protected override void Awake()
    {
        base.Awake();

        _iScore = Get<IScore>.From(gameObject);
        _remoteControlArtilleryTargets = FindObjectOfType<RemoteControlArtilleryTarget>();
        _artillerySupport = FindObjectOfType<ArtillerySupport>();
    }

    protected override void Start()
    {
        InitializeRelatedPropsButton(Names.LightMortarSupport);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        _tankController.OnInitialize -= OnInitialize;
        _propsTabCustomization.OnArtillery -= OnArtillery;
        _remoteControlArtilleryTargets.OnSet -= OnGiveCoordinates;
    }

    protected override void OnInitialize()
    {
        _propsTabCustomization.OnArtillery += OnArtillery;
        _remoteControlArtilleryTargets.OnSet += OnGiveCoordinates;
    }

    private void OnArtillery()
    {
        if (_playerTurn.IsMyTurn)
        {
            _remoteControlArtilleryTargets.RemoteControlTargetActivity(true);
            _propsTabCustomization.OnSupportOrPropsChanged?.Invoke(_relatedPropsTypeButton);
        }         
    }

    private void OnGiveCoordinates(Vector3 coordinate)
    {
        //_artillerySupport.Call(coordinate, _iScore);
    }
}

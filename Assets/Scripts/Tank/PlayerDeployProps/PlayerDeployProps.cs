using UnityEngine;

public partial class PlayerDeployProps : MonoBehaviour
{
    protected TankController _tankController;
    protected PlayerTurn _playerTurn;
    protected TurnController _turnController;
    protected PropsTabCustomization _propsTabCustomization;
    protected TilesData _tilesData;
    protected IPlayerDeployProps _iPlayerDeployProps;
    protected PhotonPlayerDeployPropsRPC _photonPlayerDeployRPC;
    protected AmmoTypeButton _relatedPropsTypeButton;



    protected virtual void Awake()
    {
        _tankController = Get<TankController>.From(gameObject);
        _playerTurn = Get<PlayerTurn>.From(gameObject);
        _turnController = FindObjectOfType<TurnController>();
        _propsTabCustomization = FindObjectOfType<PropsTabCustomization>();
        _tilesData = FindObjectOfType<TilesData>();       
    }

    protected virtual void Start()
    {
        InitializeRelatedPropsButton(Names.Sandbags);
    }

    protected virtual void OnEnable()
    {
        _tankController.OnInitialize += OnInitialize;
    }

    protected virtual void OnDisable()
    {
        _tankController.OnInitialize -= OnInitialize;
        _propsTabCustomization.OnInstantiateSandbags -= OnInstantiate;
    }

    protected virtual void GetIPlayerDeployProps()
    {
        if (_tankController.BasePlayer != null)
            _iPlayerDeployProps = Get<IPlayerDeployProps>.From(_tankController.BasePlayer.gameObject);
    }

    protected virtual void InitializeRelatedPropsButton(string propsName)
    {
        _relatedPropsTypeButton = GlobalFunctions.ObjectsOfType<AmmoTypeButton>.Find(button => button._properties.SupportOrPropsType == propsName);
    }

    protected virtual void SubscribeToPropsEvent()
    {
        _propsTabCustomization.OnInstantiateSandbags += OnInstantiate;
    }

    protected virtual void OnInitialize()
    {
        GetIPlayerDeployProps();
        SubscribeToPropsEvent();
    }
 
    protected virtual bool IsTileFound(float tilePosX, bool isPlayer1, Vector3 transformPosition)
    {
        return isPlayer1 ? tilePosX >= transformPosition.x + 0.5f && tilePosX <= transformPosition.x + 1.5f :
                           tilePosX <= transformPosition.x - 0.5f && tilePosX >= transformPosition.x - 1.5f;
    }

    protected virtual void ActivateTileProps(TileProps tileProps, bool isPlayer1)
    {
        tileProps?.ActiveProps(global::TileProps.PropsType.Sandbags, true, isPlayer1);
    }

    protected virtual void InstantiateHelper(out bool isPlayer1, out Vector3 transformPosition)
    {
        isPlayer1 = _playerTurn.MyTurn == TurnState.Player1;
        transformPosition = transform.position;
    }
}

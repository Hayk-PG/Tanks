using UnityEngine;

public partial class PlayerDeployProps : MonoBehaviour
{
    protected TankController _tankController;
    protected PlayerTurn _playerTurn;
    protected PropsTabCustomization _propsTabCustomization;
    private TilesData _tilesData;
    protected PhotonPlayerDeployPropsRPC _photonPlayerDeployRPC;



    protected virtual void Awake()
    {
        _tankController = Get<TankController>.From(gameObject);
        _playerTurn = Get<PlayerTurn>.From(gameObject);
        _propsTabCustomization = FindObjectOfType<PropsTabCustomization>();
        _tilesData = FindObjectOfType<TilesData>();
    }

    protected virtual void OnEnable()
    {
        _tankController.OnInitialize += OnInitialize;
    }

    protected virtual void OnDisable()
    {
        _tankController.OnInitialize -= OnInitialize;
        _propsTabCustomization.OnInstantiateSandbags -= OnInstantiateSandbags;
    }

    protected virtual void OnInitialize()
    {
        _propsTabCustomization.OnInstantiateSandbags += OnInstantiateSandbags;
    }
}

using UnityEngine;

public partial class PlayerDeployProps : MonoBehaviour
{
    private TankController _tankController;
    private PlayerTurn _playerTurn;
    private PropsTabCustomization _propsTabCustomization;
    private TilesData _tilesData;
    private PhotonPlayerDeployPropsRPC _photonPlayerDeployRPC;



    private void Awake()
    {
        _tankController = Get<TankController>.From(gameObject);
        _playerTurn = Get<PlayerTurn>.From(gameObject);
        _propsTabCustomization = FindObjectOfType<PropsTabCustomization>();
        _tilesData = FindObjectOfType<TilesData>();
    }

    private void OnEnable()
    {
        _tankController.OnInitialize += OnInitialize;
    }

    private void OnDisable()
    {
        _tankController.OnInitialize -= OnInitialize;
        _propsTabCustomization.OnInstantiateSandbags -= OnInstantiateSandbags;
    }

    private void OnInitialize()
    {
        _propsTabCustomization.OnInstantiateSandbags += OnInstantiateSandbags;
    }
}

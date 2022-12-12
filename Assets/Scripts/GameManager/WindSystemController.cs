using Photon.Pun;
using System;

public class WindSystemController : MonoBehaviourPun
{
    private GameManager _gameManager;
    private TurnController _turnController;

    private int _minWindForce = -5;
    private int _maxWindForce = 5;
    private bool _isWindEnabled;

    public int CurrentWindForce { get; set; }

    public event Action<int> onWindForce;




    private void Awake()
    {
        _gameManager = GetComponent<GameManager>();
        _turnController = Get<TurnController>.From(gameObject);
    }

    private void OnEnable()
    {
        _gameManager.OnGameStarted += WindActivity;
        _turnController.OnTurnChanged += GetTurnChanges;
    }
   
    private void OnDisable()
    {
        _gameManager.OnGameStarted -= WindActivity;
        _turnController.OnTurnChanged -= GetTurnChanges;
    }

    public void WindActivity()
    {
        if (MyPhotonNetwork.IsOfflineMode && Data.Manager.IsWindOn || !MyPhotonNetwork.IsOfflineMode && (bool)MyPhotonNetwork.CurrentRoom.CustomProperties[Keys.MapWind])
            _isWindEnabled = true;
    }

    private void GetTurnChanges(TurnState turnState)
    {
        if (turnState == TurnState.Player1 || turnState == TurnState.Player2)
        {
            if (_isWindEnabled)
                AssignWindValues();
        }
    }

    private void AssignWindValues()
    {
        if (!MyPhotonNetwork.IsOfflineMode && MyPhotonNetwork.AmPhotonViewOwner(photonView))
        {
            WindValues();
            photonView.RPC("ShareWindForceValue", RpcTarget.AllViaServer, CurrentWindForce);
        }
            
        if (MyPhotonNetwork.IsOfflineMode)
        {
            WindValues();
            ShareWindForceValue(CurrentWindForce);
        }           
    }

    private void WindValues() => CurrentWindForce = UnityEngine.Random.Range(_minWindForce, _maxWindForce);

    [PunRPC]
    private void ShareWindForceValue(int currentWindForce)
    {
        onWindForce?.Invoke(currentWindForce);
    }
}

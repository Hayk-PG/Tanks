using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class TurnTimer : MonoBehaviourPun
{
    private TurnController _turnController;
    private MyPlugins _myPlugins;

    [SerializeField] private Text _textTimer;
    [SerializeField] private CanvasGroup _iconPlayer1;
    [SerializeField] private CanvasGroup _iconPlayer2;

    public int Timer
    {
        get => int.Parse(_textTimer.text);
        internal set => _textTimer.text = value.ToString();
    }
    public float IconPlayer1Alpha
    {
        get => _iconPlayer1.alpha;
        internal set => _iconPlayer1.alpha = value;
    }
    public float IconPlayer2Alpha
    {
        get => _iconPlayer2.alpha;
        internal set => _iconPlayer2.alpha = value;
    }
    public bool IsTurnChanged { get; internal set; }


    private void Awake()
    {
        _turnController = Get<TurnController>.From(gameObject);
        _myPlugins = FindObjectOfType<MyPlugins>();
    }

    private void OnEnable()
    {
        _turnController.OnTurnChanged += OnTurnChanged;
    }

    private void OnDisable()
    {
        _turnController.OnTurnChanged -= OnTurnChanged;
        UnsubscribeFromPluginService();
    }

    private void UnsubscribeFromPluginService()
    {
        _myPlugins.OnPluginService -= OnPluginService;
    }

    private void SubscribeToPluginService()
    {
        _myPlugins.OnPluginService += OnPluginService;
    }

    private void OnTurnChanged(TurnState currentState, CameraMovement cameraMovement)
    {
        Timer = 0;
        UnsubscribeFromPluginService();

        if (currentState == TurnState.Player1 || currentState == TurnState.Player2)
        {
            IsTurnChanged = false;
            SubscribeToPluginService();
        }           
    }

    private void OnPluginService()
    {
        if (MyPhotonNetwork.IsOfflineMode || MyPhotonNetwork.AmPhotonViewOwner(photonView))
        {
            PlayersTurnIcons(_turnController._turnState == TurnState.Player1 ? 1: 0, _turnController._turnState == TurnState.Player2 ? 1: 0);
            Conditions<int>.Compare(Timer, 30, SetNextTurn, null, RunTimer);
        }
    } 

    private void RunTimer()
    {
        Timer++;
    }

    private void SetNextTurn()
    {
        if (!IsTurnChanged && _turnController._turnState == TurnState.Player1)
        {
            _turnController.SetNextTurn(TurnState.Player2);
            IsTurnChanged = true;
        }
        else if (!IsTurnChanged && _turnController._turnState == TurnState.Player2)
        {
            _turnController.SetNextTurn(TurnState.Player1);
            IsTurnChanged = true;
        }

        UnsubscribeFromPluginService();
    }
    
    private void PlayersTurnIcons(float iconPlayer1Alpha, float iconPlayer2Alpha)
    {
        if (IconPlayer1Alpha != iconPlayer1Alpha) IconPlayer1Alpha = iconPlayer1Alpha;
        if (IconPlayer2Alpha != iconPlayer2Alpha) IconPlayer2Alpha = iconPlayer2Alpha;
    }
}

using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TurnTimer : MonoBehaviourPun
{
    [SerializeField] private Text _textTimer;
    private TurnController _turnController;
    private MyPlugins _myPlugins;

    public int Seconds { get; set; }
    public bool IsTurnChanged { get; internal set; }
    public Action<TurnState, int> OnTurnTimer { get; set; }


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

    private void OnTurnChanged(TurnState currentState)
    {
        Seconds = currentState == TurnState.Player1 || currentState == TurnState.Player2 ? 30 : 0;
        UnsubscribeFromPluginService();

        if (currentState == TurnState.Player1 || currentState == TurnState.Player2)
        {
            IsTurnChanged = false;
            SubscribeToPluginService();
        }           
    }

#if UNITY_EDITOR
    private void Start()
    {
        StartCoroutine(RunTimerOnEditorMode());
    }

    private IEnumerator RunTimerOnEditorMode()
    {
        while (true)
        {
            RunTimer();
            yield return new WaitForSeconds(1);
        }
    }
#endif

    private void OnPluginService()
    {
        if (MyPhotonNetwork.IsOfflineMode || MyPhotonNetwork.AmPhotonViewOwner(photonView))
        {
            RunTimer();
        }
    } 

    private void Timer()
    {
        Seconds--;       
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

    private void RunTimer()
    {        
        Conditions<int>.Compare(Seconds, 0, SetNextTurn, Timer, null);
        OnTurnTimer?.Invoke(_turnController._turnState, Seconds);
        _textTimer.text = Seconds.ToString("D2");
    }
}

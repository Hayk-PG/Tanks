using UnityEngine;

public class PlayerShields : PlayerDeployProps
{
    protected Shields _shields;
    private GlobalActivityTimer _globalActivityTimer;
    private int _endTime = 60;
    public bool IsShieldActive { get; set; }
  

    protected override void Awake()
    {
        base.Awake();
        _shields = Get<Shields>.FromChild(gameObject);
        _globalActivityTimer = FindObjectOfType<GlobalActivityTimer>();
    }

    protected override void Start()
    {
        InitializeRelatedPropsButton(Names.Shield);
    }

    protected override void OnDisable()
    {
        _tankController.OnInitialize -= OnInitialize;
        _propsTabCustomization.OnActivateShields -= OnActivateShields;
    }

    protected override void OnInitialize()
    {
        _propsTabCustomization.OnActivateShields += OnActivateShields;
    }

    public void ActivateShields(int index)
    {
        IsShieldActive = true;
        _shields.Activity(0, IsShieldActive);
        Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode, () => RunTimerDirectly(), () => RunTimerViaMasterClient(index)); 
    }

    public void DeactivateShields()
    {
        IsShieldActive = false;

        for (int i = 0; i < 2; i++)
        {
            _shields.Activity(i, IsShieldActive);
        }
    }

    private void RunTimerViaMasterClient(int index)
    {
        _globalActivityTimer._playersActiveShieldsTimer[index] = _endTime;
    }

    private void RunTimerDirectly()
    {
        _shields.RunTimerCoroutine(_endTime);
    }

    private void ShieldActivityRPC(int index, bool isActive)
    {
        if (_photonPlayerDeployRPC == null)
            _photonPlayerDeployRPC = Get<PhotonPlayerDeployPropsRPC>.From(_tankController.BasePlayer.gameObject);

        _photonPlayerDeployRPC.CalShieldsActivityRPC(index);
    }

    private void OnActivateShields()
    {
        if (_playerTurn.IsMyTurn)
        {
            int index = _playerTurn.MyTurn == TurnState.Player1 ? 0 : 1;
            Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode, ()=> ActivateShields(0), ()=> ShieldActivityRPC(index, true));
            _propsTabCustomization.OnSupportOrPropsChanged?.Invoke(_relatedPropsTypeButton);
        }
    }
}

using UnityEngine;

public class PlayerTankSkipTurn : PlayerDeployProps
{
    private ScoreController _scoreController;


    protected override void Awake()
    {
        base.Awake();
        _scoreController = Get<ScoreController>.From(gameObject);
    }

    protected override void Start()
    {
        InitializeRelatedPropsButton(Names.SkipTurn);
    }

    protected override void OnDisable()
    {
        _tankController.OnInitialize -= OnInitialize;
        _propsTabCustomization.OnSkipTurn -= OnSkipTurn;
    }

    protected override void SubscribeToPropsEvent()
    {
        _propsTabCustomization.OnSkipTurn += OnSkipTurn;
    }

    private void OnSkipTurn()
    {
        if (_playerTurn.IsMyTurn)
        {
            _iPlayerDeployProps.SkipTurn(_playerTurn.MyTurn == TurnState.Player1 ? TurnState.Player2 : TurnState.Player1);
            _scoreController.GetScore(100, null);
        }
    }

    public void Skip(TurnState turnState)
    {
        _turnController.SetNextTurn(turnState);
    }
}

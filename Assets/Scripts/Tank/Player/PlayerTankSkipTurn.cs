using UnityEngine;

public class PlayerTankSkipTurn : PlayerDeployProps
{
    private ISkipTurn _iSkipTurn;
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

    protected override void OnInitialize()
    {
        _propsTabCustomization.OnSkipTurn += OnSkipTurn;
        _iSkipTurn = Get<ISkipTurn>.From(_tankController.BasePlayer.gameObject);
    }

    private void OnSkipTurn()
    {
        if (_playerTurn.IsMyTurn)
        {
            _iSkipTurn.SkipTurn(_playerTurn.MyTurn == TurnState.Player1 ? TurnState.Player2 : TurnState.Player1);
            _scoreController.GetScore(100, null);
        }
    }

    public void Skip(TurnState turnState)
    {
        _turnController.SetNextTurn(turnState);
    }
}

using System;
using UnityEngine;

public class AIActionPlanner : MonoBehaviour
{
    private enum AINextAction { Move, CallBomber}
    [SerializeField] private AINextAction _aINextAction;

    protected TurnController _turnController;
    protected ChangeTiles _changeTiles;
    protected TilesData _tilesGenerator;
    protected PlayerTurn _playerTurn;
    protected Raycasts _rayCasts;
    protected AiMovementPlanner _aiMovementPlanner;
    protected AIRequestBomber _aIRequestBomber;

    protected class InitializedValues
    {
        internal int _stepsLength;
        internal int _direction;
        internal Vector3 _destination;

        internal InitializedValues(int stepsLength, int direction, Vector3 destination)
        {
            _stepsLength = stepsLength;
            _direction = direction;
            _destination = destination;
        }
    }
    protected class UpdatedValues
    {
        internal Vector3 _currentTilePos;
        internal Vector3 _nextTilePos;
        internal Vector3 _aboveNextTilePos;
        internal float _desiredDirection;
        internal float _y;

        internal UpdatedValues(Vector3 currentTilePos, float desiredDirection, float y)
        {
            _currentTilePos = currentTilePos;
            _aboveNextTilePos = new Vector3(0, 0.5f, 0);
            _desiredDirection = desiredDirection;
            _y = y;
        }
    }

    protected InitializedValues _initializedValues;
    protected UpdatedValues _updatedValues;


    protected virtual void Awake()
    {
        _rayCasts = Get<Raycasts>.FromChild(gameObject);
        _turnController = FindObjectOfType<TurnController>();
        _changeTiles = FindObjectOfType<ChangeTiles>();
        _tilesGenerator = FindObjectOfType<TilesData>();
        _playerTurn = GetComponent<PlayerTurn>();
        _aiMovementPlanner = Get<AiMovementPlanner>.From(gameObject);
        _aIRequestBomber = Get<AIRequestBomber>.From(gameObject);
    }

    protected virtual void OnEnable()
    {
        _turnController.OnTurnChanged += OnTurnChanged;    
    }

    protected virtual void OnDisable()
    {
        _turnController.OnTurnChanged -= OnTurnChanged;
    }

    protected virtual void OnTurnChanged(TurnState turnState)
    {
        if(turnState == _playerTurn.MyTurn)
        {
            int randomAction = UnityEngine.Random.Range(0, Enum.GetValues(typeof(AINextAction)).Length);
            _aINextAction = (AINextAction)randomAction;

            switch (_aINextAction)
            {
                case AINextAction.Move: _aiMovementPlanner.MovementPlanner();break;
                case AINextAction.CallBomber:
                    _aIRequestBomber.CallBomber(out bool canCallBomber);
                    if(!canCallBomber) _aiMovementPlanner.MovementPlanner(); break;
            } 
        }
    }   
}

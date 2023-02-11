using System;
using UnityEngine;

public class AIActionPlanner : MonoBehaviour
{
    private enum AINextAction { Move, CallBomber, CallArtillery}

    [SerializeField] 
    private AINextAction _aINextAction;

    private TurnController _turnController;
    private PlayerTurn _playerTurn;
    private AiMovementPlanner _aiMovementPlanner;
    private AIRequestBomber _aIRequestBomber;
    private AIRequestArtillery _aiRequestArtillery;

    private class InitializedValues
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
    private class UpdatedValues
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

    private InitializedValues _initializedValues;
    private UpdatedValues _updatedValues;


    //private void Awake()
    //{
    //    _turnController = FindObjectOfType<TurnController>();
    //    _playerTurn = GetComponent<PlayerTurn>();
    //    _aiMovementPlanner = Get<AiMovementPlanner>.From(gameObject);
    //    _aIRequestBomber = Get<AIRequestBomber>.From(gameObject);
    //    _aiRequestArtillery = Get<AIRequestArtillery>.From(gameObject);
    //}

    //private void OnEnable()
    //{
    //    //_turnController.OnTurnChanged += OnTurnChanged;    
    //}

    //private void OnDisable()
    //{
    //    //_turnController.OnTurnChanged -= OnTurnChanged;
    //}

    //private void OnTurnChanged(TurnState turnState)
    //{
    //    if(turnState == _playerTurn.MyTurn)
    //    {
    //        int randomAction = UnityEngine.Random.Range(0, Enum.GetValues(typeof(AINextAction)).Length);
    //        //_aINextAction = (AINextAction)randomAction;
    //        _aINextAction = AINextAction.Move;

    //        switch (_aINextAction)
    //        {
    //            case AINextAction.Move:

    //                StartCoroutine(_aiMovementPlanner.Execute());

    //                break;

    //            case AINextAction.CallBomber:

    //                _aIRequestBomber.Use(out bool canCallArSupport);

    //                if(!canCallArSupport)
    //                    StartCoroutine(_aiMovementPlanner.Execute()); 
                    
    //                break;

    //            case AINextAction.CallArtillery:

    //                _aiRequestArtillery.Use(out bool canCallArtillery);

    //                if (!canCallArtillery)
    //                    StartCoroutine(_aiMovementPlanner.Execute()); 
                    
    //                break;
    //        } 
    //    }
    //}   
}

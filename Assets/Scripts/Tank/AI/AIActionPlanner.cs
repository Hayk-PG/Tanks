using System;
using System.Collections;
using UnityEngine;

public class AIActionPlanner : MonoBehaviour
{
    private TurnController _turnController;
    private ChangeTiles _changeTiles;
    private TilesData _tilesGenerator;
    private PlayerTurn _playerTurn;
    private Raycasts _rayCasts;

    public event Action<Vector3, int> OnActionPlanner;

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


    private void Awake()
    {
        _rayCasts = Get<Raycasts>.FromChild(gameObject);
        _turnController = FindObjectOfType<TurnController>();
        _changeTiles = FindObjectOfType<ChangeTiles>();
        _tilesGenerator = FindObjectOfType<TilesData>();
        _playerTurn = GetComponent<PlayerTurn>();
    }

    private void OnEnable()
    {
        _turnController.OnTurnChanged += OnTurnChanged;    
    }
   
    private void OnDisable()
    {
        _turnController.OnTurnChanged -= OnTurnChanged;
    }
   
    private void OnTurnChanged(TurnState arg1, CameraMovement arg2)
    {
        if(arg1 == _playerTurn.MyTurn)
        {
            _rayCasts.CastRays(Vector3.zero, Vector3.zero);
            _initializedValues = new InitializedValues(UnityEngine.Random.Range(1, 10), 0, Vector3.zero);

            if (_rayCasts.DownHit.collider?.tag == Tags.Tile)
            {
                _updatedValues = new UpdatedValues(_rayCasts.DownHit.collider.transform.position,
                    _rayCasts.DownHit.collider.name == Names.RS ? 1:
                    _rayCasts.DownHit.collider.name == Names.LS ? -1 : UnityEngine.Random.Range(-1, 2), 0);

                for (float i = 0.5f; i < _initializedValues._stepsLength; i += 0.5f)
                {
                    _updatedValues._nextTilePos = _updatedValues._currentTilePos - new Vector3(_updatedValues._desiredDirection > 0 ? i : -i , _updatedValues._y, 0);

                    if(_updatedValues._desiredDirection > 0)
                    {
                        Conditions<bool>.Compare(Slope(_updatedValues._nextTilePos + _updatedValues._aboveNextTilePos, Names.LS), Slope(_updatedValues._nextTilePos, Names.RS),
                            delegate { IncreaseY(i); },
                            delegate { DecreaseY(); },
                            null,
                            null);
                    }
                    else
                    {
                        Conditions<bool>.Compare(Slope(_updatedValues._nextTilePos, Names.LS), Slope(_updatedValues._nextTilePos + _updatedValues._aboveNextTilePos, Names.RS),
                            delegate { DecreaseY(); },
                            delegate { IncreaseY(i); },
                            null,
                            null);
                    }

                    if (_changeTiles.HasTile(_updatedValues._nextTilePos) && IsNextPositionAvailable(_tilesGenerator.TilesDict[_updatedValues._nextTilePos].name))
                    {
                        _initializedValues._destination = _updatedValues._nextTilePos;
                        _initializedValues._direction = _updatedValues._desiredDirection > 0 ? 1 : -1;
                    }
                    
                    else
                    {
                        break;
                    }
                }
            }

            StartCoroutine(InvokeOnActionPlanner(_initializedValues._destination, _initializedValues._direction));
        }
    }

    private void IncreaseY(float i)
    {
        _updatedValues._y += -0.5f;
        _updatedValues._nextTilePos = _updatedValues._currentTilePos - new Vector3(_updatedValues._desiredDirection > 0 ? i : -i, _updatedValues._y, 0);
    }

    private void DecreaseY()
    {
        _updatedValues._y -= -0.5f;
    }

    IEnumerator InvokeOnActionPlanner(Vector3 destination, int direction)
    {
        yield return new WaitForSeconds(3);

        OnActionPlanner?.Invoke(destination, direction);
    }

    private bool Slope(Vector3 nextTilePos, string name)
    {
        return _tilesGenerator.TilesDict.ContainsKey(nextTilePos) && _tilesGenerator.TilesDict[nextTilePos].gameObject != null && _tilesGenerator.TilesDict[nextTilePos].name == name;
    }

    private bool IsNextPositionAvailable(string name)
    {
        return name == Names.LS || name == Names.RS || name == Names.T;
    }
}

using System.Collections;
using UnityEngine;

public class AiMovementPlanner : MonoBehaviour
{
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

    [SerializeField]
    private Raycasts _rayCasts;

    [SerializeField]
    private AIState _aiState;

    private InitializedValues _initializedValues;
    private UpdatedValues _updatedValues;

    public event System.Action<Vector3, int> onAiMovementPlanner;




    private void OnEnable()
    {
        _aiState.onMove += (int stepsLength, int direction) => { StartCoroutine(Execute(stepsLength, direction)); };
    }

    private void OnDisable()
    {
        _aiState.onMove -= (int stepsLength, int direction) => { StartCoroutine(Execute(stepsLength, direction)); };
    }


    internal IEnumerator Execute(int stepsLength, int direction)
    {
        yield return StartCoroutine(CastRayAndInitialize());

        yield return null;

        if(_rayCasts.DownHit.collider?.tag == Tags.Tile)
        {
            yield return StartCoroutine(InitializeUpdatedValues());

            yield return null;

            for (float i = 0.5f; i < stepsLength; i += 0.5f)
            {
                GetNextTilePosition(i);
                OnPositiveDirection(i);
                OnNegativeDirection(i);

                bool isNextPositionAvailable = GameSceneObjectsReferences.ChangeTiles.HasTile(_updatedValues._nextTilePos) && 
                                               IsNextPositionAvailable(GameSceneObjectsReferences.TilesData.TilesDict[_updatedValues._nextTilePos].name);

                if (isNextPositionAvailable)
                    SetDestinationAndDirection();
                else
                    break;
            }
        }

        yield return null;

        onAiMovementPlanner?.Invoke(_initializedValues._destination, direction);
    }

    private IEnumerator CastRayAndInitialize()
    {
        yield return null;

        _rayCasts.CastRays(Vector3.zero, Vector3.zero);
        _initializedValues = new InitializedValues(Random.Range(0, 10), 0, Vector3.zero);

        //GlobalFunctions.DebugLog("CastRayAndInitialize: ");
    }

    private IEnumerator InitializeUpdatedValues()
    {
        yield return null;

        _updatedValues = new UpdatedValues(_rayCasts.DownHit.collider.transform.position,
                                           _rayCasts.DownHit.collider.name == Names.RS ? 1 :
                                            _rayCasts.DownHit.collider.name == Names.LS ? -1 : Random.Range(-1, 2), 0);

        //GlobalFunctions.DebugLog("InitializeUpdatedValues: ");
    }

    private void GetNextTilePosition(float i)
    {
        _updatedValues._nextTilePos = _updatedValues._currentTilePos - new Vector3(_updatedValues._desiredDirection > 0 ? i : -i, _updatedValues._y, 0);

        //GlobalFunctions.DebugLog("GetNextTilePosition: " + _updatedValues._nextTilePos);
    }

    private void OnPositiveDirection(float i)
    {
        if (_updatedValues._desiredDirection > 0)
        {
            Conditions<bool>.Compare(Slope(_updatedValues._nextTilePos + _updatedValues._aboveNextTilePos, Names.LS), Slope(_updatedValues._nextTilePos, Names.RS),
                delegate { IncreaseY(i); },
                delegate { DecreaseY(); },
                null,
                null);

            //GlobalFunctions.DebugLog("OnPositiveDirection: " + _updatedValues._desiredDirection);
        }
    }

    private void OnNegativeDirection(float i)
    {
        if (_updatedValues._desiredDirection <= 0)
        {
            Conditions<bool>.Compare(Slope(_updatedValues._nextTilePos, Names.LS), Slope(_updatedValues._nextTilePos + _updatedValues._aboveNextTilePos, Names.RS),
                        delegate { DecreaseY(); },
                        delegate { IncreaseY(i); },
                        null,
                        null);

            //GlobalFunctions.DebugLog("OnNegativeDirection: " + _updatedValues._desiredDirection);
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

    private void SetDestinationAndDirection()
    {
        _initializedValues._destination = _updatedValues._nextTilePos;
        _initializedValues._direction = _updatedValues._desiredDirection > 0 ? 1 : -1;

        //GlobalFunctions.DebugLog("SetDestinationAndDirection: " + _initializedValues._destination + "/" + _initializedValues._direction);
    }

    private bool Slope(Vector3 nextTilePos, string name)
    {
        return GameSceneObjectsReferences.TilesData.TilesDict.ContainsKey(nextTilePos) &&
               GameSceneObjectsReferences.TilesData.TilesDict[nextTilePos].gameObject != null && 
               GameSceneObjectsReferences.TilesData.TilesDict[nextTilePos].name == name;
    }

    private bool IsNextPositionAvailable(string name)
    {
        return name == Names.LS || name == Names.RS || name == Names.T;
    }
}

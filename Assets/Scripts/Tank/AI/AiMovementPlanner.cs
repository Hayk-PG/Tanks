using System.Collections;
using UnityEngine;

public class AiMovementPlanner : AIActionPlanner
{
    public System.Action<Vector3, int> OnAIMovementPlanner { get; set; }

    protected override void OnEnable()
    {
        
    }

    protected override void OnDisable()
    {
        
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

    private bool Slope(Vector3 nextTilePos, string name)
    {
        return _tilesGenerator.TilesDict.ContainsKey(nextTilePos) && _tilesGenerator.TilesDict[nextTilePos].gameObject != null && _tilesGenerator.TilesDict[nextTilePos].name == name;
    }

    private bool IsNextPositionAvailable(string name)
    {
        return name == Names.LS || name == Names.RS || name == Names.T;
    }

    private IEnumerator InvokeOnActionPlanner(Vector3 destination, int direction)
    {
        yield return new WaitForSeconds(3);

        OnAIMovementPlanner?.Invoke(destination, direction);
    }

    internal void MovementPlanner()
    {
        _rayCasts.CastRays(Vector3.zero, Vector3.zero);
        _initializedValues = new InitializedValues(Random.Range(1, 10), 0, Vector3.zero);

        if (_rayCasts.DownHit.collider?.tag == Tags.Tile)
        {
            _updatedValues = new UpdatedValues(_rayCasts.DownHit.collider.transform.position,
                _rayCasts.DownHit.collider.name == Names.RS ? 1 :
                _rayCasts.DownHit.collider.name == Names.LS ? -1 : Random.Range(-1, 2), 0);

            for (float i = 0.5f; i < _initializedValues._stepsLength; i += 0.5f)
            {
                _updatedValues._nextTilePos = _updatedValues._currentTilePos - new Vector3(_updatedValues._desiredDirection > 0 ? i : -i, _updatedValues._y, 0);

                if (_updatedValues._desiredDirection > 0)
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

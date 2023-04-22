using System.Collections;
using UnityEngine;

public class AiMovementPlanner : MonoBehaviour
{
    [SerializeField] [Space]
    private Raycasts _rayCasts;

    [SerializeField] [Space]
    private AIState _aiState;

    private Vector3 _nextTilePosition;

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
        _rayCasts.CastRays(Vector3.zero, Vector3.zero);

        _nextTilePosition = transform.position;

        if (_rayCasts.DownHit.collider?.tag == Tags.Tile)
        {
            _nextTilePosition = _rayCasts.DownHit.collider.transform.position;

            for (float i = 0; i < stepsLength; i += 0.5f)
            {
                Vector3 horTile = new Vector3(direction < 0 ? 0.5f : -0.5f, 0, 0);
                Vector3 vertTile = new Vector3(0, 0.5f, 0);

                yield return StartCoroutine(CheckNextTilePosition(horTile, vertTile));
                yield return StartCoroutine(CheckNextTilePositionVertically(horTile, vertTile));
            }
        }

        Vector3 destination = new Vector3(direction == -1 ? _nextTilePosition.x - 0.5f : direction == 1 ? _nextTilePosition.x + 0.5f : 0, _nextTilePosition.y, _nextTilePosition.z);

        onAiMovementPlanner?.Invoke(destination, direction);
    }

    private IEnumerator CheckNextTilePosition(Vector3 horTile, Vector3 vertTile)
    {
        Vector3 nextTilePosition = _nextTilePosition + horTile;

        if (!GameSceneObjectsReferences.ChangeTiles.HasTile(nextTilePosition))
            yield break;

        bool hasTileAbove = GameSceneObjectsReferences.ChangeTiles.HasTile(nextTilePosition + vertTile);

        bool isSlope = hasTileAbove && GameSceneObjectsReferences.TilesData.TilesDict[nextTilePosition + vertTile].name == Names.RS ||
                       hasTileAbove && GameSceneObjectsReferences.TilesData.TilesDict[nextTilePosition + vertTile].name == Names.LS;

        bool shouldAvoid = Get<TileProps>.From(GameSceneObjectsReferences.TilesData.TilesDict[nextTilePosition])?.ShouldAvoid ?? false;

        if (!shouldAvoid)
            if (isSlope || !hasTileAbove)
                _nextTilePosition = nextTilePosition;

        yield return null;
    }

    private IEnumerator CheckNextTilePositionVertically(Vector3 horTile, Vector3 vertTile)
    {
        Vector3 nextTilePosition = _nextTilePosition + horTile;

        bool hasTile = GameSceneObjectsReferences.ChangeTiles.HasTile(nextTilePosition);
        bool hasTileBottom = GameSceneObjectsReferences.ChangeTiles.HasTile(nextTilePosition - vertTile);

        if (!hasTile)
        {
            for (int y = 0; y < 5; y++)
            {
                if (hasTileBottom)
                {
                    _nextTilePosition = nextTilePosition;

                    yield return null;

                    yield break;
                }
            }
        }
    }
}

using System;
using System.Collections;
using UnityEngine;

public class AIActionPlanner : MonoBehaviour
{
    private TurnController _turnController;
    private ChangeTiles _changeTiles;
    private TilesData _tilesGenerator;
    private PlayerTurn _playerTurn;
    
    [SerializeField]
    private Raycasts _rayCasts;

    public event Action<Vector3, int> OnActionPlanner;


    private void Awake()
    {
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

    private void InitializeAIMovementParameters(out int stepsLength, out int direction, out Vector3 destination)
    {
        stepsLength = UnityEngine.Random.Range(5, 15);
        destination = Vector3.zero;
        direction = 0;
    }

    private void OnTurnChanged(TurnState arg1, CameraMovement arg2)
    {
        if(arg1 == _playerTurn.MyTurn)
        {
            _rayCasts.CastRays(Vector3.zero, Vector3.zero);

            int stepsLength = UnityEngine.Random.Range(5, 15);
            Vector3 destination = Vector3.zero;
            int direction = 0;


            if (_rayCasts.DownHit.collider?.tag == Tags.Tile)
            {              
                Vector3 currentTilePos = _rayCasts.DownHit.collider.transform.position;
                Vector3 nextTilePos;
                float desiredDirection = UnityEngine.Random.Range(-1, 2);
                float y = 0;

                for (float i = 0.5f; i < stepsLength; i += 0.5f)
                {
                    nextTilePos = currentTilePos - new Vector3(desiredDirection > 0 ? i : -i , y, 0);

                    if(desiredDirection > 0)
                    {
                        if (LS(new Vector3(nextTilePos.x, nextTilePos.y + 0.5f, nextTilePos.z)))
                        {
                            print("LS");
                            y += -0.5f;
                            nextTilePos = currentTilePos - new Vector3(desiredDirection > 0 ? i : -i, y, 0);
                        }

                        if (RS(nextTilePos))
                        {
                            print("RS");
                            y -= -0.5f;
                        }
                    }
                    else
                    {
                        if (LS(nextTilePos))
                        {
                            print("LS");
                            y -= -0.5f;
                        }

                        if (RS(new Vector3(nextTilePos.x, nextTilePos.y + 0.5f, nextTilePos.z)))
                        {
                            y += -0.5f;
                            nextTilePos = currentTilePos - new Vector3(desiredDirection > 0 ? i : -i, y, 0);
                        }
                    }

                    print(nextTilePos);

                    if (_changeTiles.HasTile(nextTilePos) && IsNextPositionAvailable(_tilesGenerator.TilesDict[nextTilePos].name))
                    {
                        destination = nextTilePos;
                        direction = desiredDirection > 0 ? 1 : -1;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            StartCoroutine(InvokeOnActionPlanner(destination, direction));
        }
    }

    IEnumerator InvokeOnActionPlanner(Vector3 destination, int direction)
    {
        yield return new WaitForSeconds(3);

        OnActionPlanner?.Invoke(destination, direction);
    }

    private bool LS(Vector3 nextTilePos)
    {
        return _tilesGenerator.TilesDict.ContainsKey(nextTilePos) && _tilesGenerator.TilesDict[nextTilePos].name == Names.LS;
    }

    private bool RS(Vector3 nextTilePos)
    {
        return _tilesGenerator.TilesDict.ContainsKey(nextTilePos) && _tilesGenerator.TilesDict[nextTilePos].name == Names.RS;
    }

    private bool IsNextPositionAvailable(string name)
    {
        return name == Names.LS || name == Names.RS || name == Names.T;
    }
}

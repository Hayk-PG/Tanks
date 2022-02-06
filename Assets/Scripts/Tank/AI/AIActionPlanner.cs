using System;
using System.Collections;
using UnityEngine;

public class AIActionPlanner : MonoBehaviour
{
    private TurnController _turnController;
    private ChangeTiles _changeTiles;
    private TilesGenerator _tilesGenerator;
    private PlayerTurn _playerTurn;
    
    [SerializeField]
    private Raycasts _rayCasts;

    public event Action<Vector3, int> OnActionPlanner;


    private void Awake()
    {
        _turnController = FindObjectOfType<TurnController>();
        _changeTiles = FindObjectOfType<ChangeTiles>();
        _tilesGenerator = FindObjectOfType<TilesGenerator>();
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

            Vector3 destination = Vector3.zero;
            int direction = 0;


            if (_rayCasts.DownHit.collider?.tag == Tags.Tile)
            {              
                Vector3 currentTilePos = _rayCasts.DownHit.collider.transform.position;
                Vector3 nextTilePos;
                float desiredDirection = UnityEngine.Random.Range(-1, 2);

                for (float i = 0.5f; i < 5; i += 0.5f)
                {
                    nextTilePos = currentTilePos - new Vector3(desiredDirection > 0 ? i : -i , 0, 0);

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

    private bool IsNextPositionAvailable(string name)
    {
        return name == Names.LS || name == Names.RS || name == Names.T;
    }
}

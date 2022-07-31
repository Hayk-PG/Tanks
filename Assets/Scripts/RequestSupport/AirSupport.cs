using UnityEngine;

public class AirSupport : MonoBehaviour, ITurnController
{
    [SerializeField] private Bomber _bomber;

    private LevelGenerator _levelGenerator;
    private MainCameraController _mainCameraController;
    private float StartPointX
    {
        get
        {
            return _levelGenerator.MapHorizontalStartPoint - 2;
        }
    }
    private float EndPointX
    {
        get
        {
            return _levelGenerator.MapHorizontalEndPoint + 2;
        }
    }
    public TurnController TurnController { get; set; }


    private void Awake()
    {
        _levelGenerator = FindObjectOfType<LevelGenerator>();
        _mainCameraController = FindObjectOfType<MainCameraController>();
        TurnController = FindObjectOfType<TurnController>();
    }

    private Vector3 StartPosition(PlayerTurn playerTurn)
    {
        return playerTurn.MyTurn == TurnState.Player1 ? 
            new Vector3(StartPointX, 5, 0) :
            new Vector3(EndPointX, 5, 0);
    }

    private Quaternion StartRotation(PlayerTurn playerTurn)
    {
        return playerTurn.MyTurn == TurnState.Player1 ? 
            Quaternion.Euler(-90, 90, 0) : Quaternion.Euler(-90, -90, 0);
    }

    public void Call(out Bomber bomber, PlayerTurn playerTurn)
    {
        _bomber.transform.position = StartPosition(playerTurn);
        _bomber.transform.rotation = StartRotation(playerTurn);
        _bomber.MinX = StartPointX;
        _bomber.MaxX = EndPointX;
        bomber = _bomber;
        TurnController.SetNextTurn(TurnState.Other);       
        _bomber.gameObject.SetActive(true);
        _mainCameraController.SetTarget(playerTurn, _bomber.transform);
    }
}

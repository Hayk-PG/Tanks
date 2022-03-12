using System;
using UnityEngine;

public class PlayerTurn : MonoBehaviour
{
    [SerializeField]
    private TurnState _myTurn;
    public TurnState MyTurn
    {
        get => _myTurn;
        set => _myTurn = value;
    }
    public TurnController TurnController { get; private set; }
    public bool IsMyTurn{ get; private set; }

    public event Action<TurnState, TurnState> OnTurnChangeEventReceived;


    private void Awake()
    {
        TurnController = FindObjectOfType<TurnController>();
    }

    private void OnEnable()
    {
        TurnController.OnTurnChanged += OnTurnChanged;
    }
   
    private void OnDisable()
    {
        TurnController.OnTurnChanged -= OnTurnChanged;
    }

    private void OnTurnChanged(TurnState arg1, CameraMovement arg2)
    {
        OnTurnChangeEventReceived?.Invoke(arg1, MyTurn);

        if (arg1 == MyTurn)
        {
            arg2.SetCameraTarget(transform, 5, 2);
            IsMyTurn = true;
        }
        else
        {
            IsMyTurn = false;
        }
    }
}

using System;
using UnityEngine;

public class PlayerTurn : MonoBehaviour
{
    [SerializeField]
    private TurnState _myTurn;
    private TurnController _turnController;

    public TurnState MyTurn
    {
        get => _myTurn;
        set => _myTurn = value;
    }  
    public bool IsMyTurn{ get; set; }


    private void Awake()
    {
        _turnController = FindObjectOfType<TurnController>();
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

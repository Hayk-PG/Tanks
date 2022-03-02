using System;
using UnityEngine;

public class BombController : MonoBehaviour, IBulletCollision, ITurnController
{
    public Action<Collision> OnCollision { get; set; }
    public Action<IScore> OnExplodeOnCollision { get; set; }
    public TurnController TurnController { get; set; }
    public CameraMovement CameraMovement { get; set; }

    public IScore _iScore;


    private void Awake()
    {
        TurnController = FindObjectOfType<TurnController>();
        CameraMovement = FindObjectOfType<CameraMovement>();
    }

    private void Start()
    {
        TurnController.SetNextTurn(TurnState.Other);
        CameraMovement.SetCameraTarget(transform.transform, 10, 2);
    }

    private void OnCollisionEnter(Collision collision)
    {
        OnCollision?.Invoke(collision);
        OnExplodeOnCollision?.Invoke(_iScore);
    }
}

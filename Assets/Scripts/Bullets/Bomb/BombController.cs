using UnityEngine;

public class BombController : BulletController
{
    protected override void Awake()
    {
        RigidBody = GetComponent<Rigidbody>();
        TurnController = FindObjectOfType<TurnController>();
        CameraMovement = FindObjectOfType<CameraMovement>();
    }

    protected override void Start()
    {
        TurnController.SetNextTurn(TurnState.Other);
        CameraMovement.SetCameraTarget(transform.transform, 10, 2);
    }

    private void Update()
    {
        base.ExplodeOnLimit();
    }
}

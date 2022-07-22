﻿using UnityEngine;

public class BombController : BulletController
{
    protected override void Awake()
    {
        RigidBody = GetComponent<Rigidbody>();
        TurnController = FindObjectOfType<TurnController>();
    }

    protected override void Start()
    {
        TurnController.SetNextTurn(TurnState.Other);
    }

    private void Update()
    {
        base.ExplodeOnLimit();
    }
}

using System;
using UnityEngine;

public class AirSupport : MonoBehaviour, ITurnController
{
    [SerializeField]
    private Bomber _bomber;
    public TurnController TurnController { get; set; }
    public CameraMovement CameraMovement { get; set; }



    private void Awake()
    {        
        TurnController = FindObjectOfType<TurnController>();
        CameraMovement = FindObjectOfType<CameraMovement>();
    }

    public void Call(out Bomber bomber, Vector3 position, Quaternion rotation, float distanceX)
    {
        _bomber.transform.position = position;
        _bomber.transform.rotation = rotation;
        _bomber.distanceX = distanceX;
        bomber = _bomber;
        TurnController.SetNextTurn(TurnState.Other);
        CameraMovement.SetCameraTarget(_bomber.transform, 10, 5);
        _bomber.gameObject.SetActive(true);
    }
}

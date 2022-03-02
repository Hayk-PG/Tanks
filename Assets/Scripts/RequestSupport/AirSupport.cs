using System;
using UnityEngine;

public class AirSupport : MonoBehaviour, ITurnController
{
    [SerializeField]
    private Bomber _bomber;
    public Bomber Bomber => _bomber;

    public TurnController TurnController { get; set; }
    public CameraMovement CameraMovement { get; set; }

    public struct InstantiateProperties
    {
        public Vector3 _position;
        public Quaternion _rotation;

        public InstantiateProperties(Vector3 position, Quaternion rotation)
        {
            _position = position;
            _rotation = rotation;
        }
    }

    public event Action<Bomber, Action<InstantiateProperties>> OnRequestAirSupport;



    private void Awake()
    {        
        TurnController = FindObjectOfType<TurnController>();
        CameraMovement = FindObjectOfType<CameraMovement>();
    }

    public void CallOnRequestAirSupport()
    {
        OnRequestAirSupport?.Invoke(Bomber, ActivateBomber);
    }

    public void ActivateBomber(InstantiateProperties properties)
    {
        _bomber.transform.position = properties._position;
        _bomber.transform.rotation = properties._rotation;
        TurnController.SetNextTurn(TurnState.Other);
        CameraMovement.SetCameraTarget(_bomber.transform, 10, 5);
        _bomber.gameObject.SetActive(true);
    }
}

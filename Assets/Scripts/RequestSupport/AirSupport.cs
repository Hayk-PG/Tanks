using System;
using UnityEngine;

public class AirSupport : MonoBehaviour
{
    [SerializeField]
    private Bomber _bomber;
    public Bomber Bomber => _bomber;

    private CameraMovement _cameraMovement;
    private TurnController _turnController;

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
        _cameraMovement = FindObjectOfType<CameraMovement>();
        _turnController = FindObjectOfType<TurnController>();
    }

    public void CallOnRequestAirSupport()
    {
        OnRequestAirSupport?.Invoke(Bomber, ActivateBomber);
    }

    public void ActivateBomber(InstantiateProperties properties)
    {
        _bomber.transform.position = properties._position;
        _bomber.transform.rotation = properties._rotation;
        _bomber.gameObject.SetActive(true);
        _turnController.SetNextTurn(TurnState.Other);
        _cameraMovement.SetCameraTarget(_bomber.transform, 10, 5);
    }
}

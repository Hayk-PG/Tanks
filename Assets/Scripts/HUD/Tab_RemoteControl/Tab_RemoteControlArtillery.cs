using UnityEngine;

public class Tab_RemoteControlArtillery : MonoBehaviour
{
    private PropsTabCustomization _propsTabCustomization;
    private CameraMovement _cameraMovement;
    [SerializeField] private RemoteControlArtilleryTarget _remoteControlArtilleryTarget;


    private void Awake()
    {
        _propsTabCustomization = FindObjectOfType<PropsTabCustomization>();
        _cameraMovement = FindObjectOfType<CameraMovement>();
    }

    private void OnEnable()
    {
        _propsTabCustomization.OnArtillery += OnArtillery;
        _remoteControlArtilleryTarget.OnSet += DeactivateRemoteControl;
    }

    private void OnDisable()
    {
        _propsTabCustomization.OnArtillery -= OnArtillery;
        _remoteControlArtilleryTarget.OnSet -= DeactivateRemoteControl;
    }

    private void OnArtillery()
    {
        Transform enemy = GlobalFunctions.ObjectsOfType<PlayerTurn>.Find(player => player.MyTurn == TurnState.Player2).transform;
        _cameraMovement.SetCameraTarget(enemy, 5, 1.5f);
        _remoteControlArtilleryTarget.RemoteControlTargetActivity(true);
    }

    private void DeactivateRemoteControl(Vector3 coordinates)
    {
        _remoteControlArtilleryTarget.RemoteControlTargetActivity(false);
    }
}

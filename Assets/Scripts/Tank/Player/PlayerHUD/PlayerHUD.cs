using UnityEngine;

public class PlayerHUD : MonoBehaviour
{ 
    [SerializeField] 
    protected Canvas _canvas;

    [SerializeField]
    protected CanvasGroup _mainCanvasGroup;

    [SerializeField] 
    protected CanvasGroup _canvasGroupShootValues;

    [SerializeField]
    protected TankController _tankController;

    [SerializeField]
    protected TankMovement _tankMovement;

    protected PhotonPlayerEnableHUDRPC _photonPlayerEnableHUDRPC;



    protected virtual void Start()
    {
        _canvas.worldCamera = Camera.main;
    }

    protected virtual void OnEnable()
    {
        if (_tankMovement != null)
            _tankMovement.OnDirectionValue += OnMovementDirectionValue;
    }

    protected virtual void OnDisable()
    {
        if (_tankMovement != null)
            _tankMovement.OnDirectionValue -= OnMovementDirectionValue;
    }

    public virtual void MainCanvasGroupActivity(bool isActive)
    {
        GlobalFunctions.CanvasGroupActivity(_mainCanvasGroup, isActive);
    }

    protected virtual void MainCanvasGroupActivityInOnlineMode()
    {
        if (_tankController.BasePlayer != null && _photonPlayerEnableHUDRPC == null)
            _photonPlayerEnableHUDRPC = Get<PhotonPlayerEnableHUDRPC>.From(_tankController.BasePlayer.gameObject);

        _photonPlayerEnableHUDRPC?.CallHUDRPC(true);
    }

    protected virtual void OnMovementDirectionValue(float direction)
    {
        if(_tankController?.BasePlayer != null)
        {
            _canvasGroupShootValues.alpha = direction == 0 ? 1 : 0;
        }
    }

    protected virtual void EnablePlayerHUD()
    {
        Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode, () => MainCanvasGroupActivity(true), MainCanvasGroupActivityInOnlineMode);
    }
}

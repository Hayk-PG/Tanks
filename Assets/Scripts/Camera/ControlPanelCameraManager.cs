using UnityEngine;

public class ControlPanelCameraManager : MonoBehaviour
{
    [SerializeField]
    private Camera _cameraControlPanel, _cameraNoPP, _cameraHud, _cameraMain;

    [SerializeField] [Space]
    private MeshRenderer _meshRendererBackground;

    private bool _cameraControlDefaultState, _cameraNoPPDefaultState, _cameraHudDefaultState;

    private int _cameraMainDefaultCullingMask, _backgroundDefaultLayer;




    private void Start() => CacheDefaultStates();

    //private void OnEnable() => GameSceneObjectsReferences.HudTabsHandler.onRequestTabActivityPermission += OnRocketController;

    //private void OnDisable() => GameSceneObjectsReferences.HudTabsHandler.onRequestTabActivityPermission -= OnRocketController;

    private void CacheDefaultStates()
    {
        _cameraControlDefaultState = _cameraControlPanel.enabled;
        _cameraNoPPDefaultState = _cameraNoPP.enabled;
        _cameraHudDefaultState = _cameraHud.enabled;

        _cameraMainDefaultCullingMask = _cameraMain.cullingMask;
        _backgroundDefaultLayer = _meshRendererBackground.gameObject.layer;
    }

    private void OnRocketController(IHudTabsObserver observer, HudTabsHandler.HudTab currentActiveTab, HudTabsHandler.HudTab requestedTab, bool isActive)
    {
        if (requestedTab != HudTabsHandler.HudTab.TabRocketController && requestedTab != HudTabsHandler.HudTab.TabModify)
            return;

        SetActive(isActive);
    }

    private void SetActive(bool isActive)
    {
        if (isActive)
            ControlStates(true, false, false, 0, 11);
        else
            ControlStates(_cameraControlDefaultState, _cameraNoPPDefaultState, _cameraHudDefaultState, _cameraMainDefaultCullingMask, _backgroundDefaultLayer);
    }

    private void ControlStates(bool isCameraControlPanelEnables, bool isCameraNoPPEnabled, bool isCameraHudEnabled, int cameraMainCullingMask, int backgroundLayer)
    {
        _cameraControlPanel.enabled = isCameraControlPanelEnables;
        _cameraNoPP.enabled = isCameraNoPPEnabled;
        _cameraHud.enabled = isCameraHudEnabled;

        _cameraMain.cullingMask = cameraMainCullingMask;

        _meshRendererBackground.gameObject.layer = backgroundLayer;
    }
}

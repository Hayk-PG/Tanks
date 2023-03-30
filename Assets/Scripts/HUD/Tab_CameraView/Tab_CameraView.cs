using UnityEngine;

public class Tab_CameraView : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup _canvasGroup;

    [SerializeField] [Space]
    private Animator _animator;



    private void OnEnable() => GameSceneObjectsReferences.HudTabsHandler.onRequestTabActivityPermission += HudTabsHandler_onRequestTabActivityPermission;

    private void OnDisable() => GameSceneObjectsReferences.HudTabsHandler.onRequestTabActivityPermission -= HudTabsHandler_onRequestTabActivityPermission;

    private void HudTabsHandler_onRequestTabActivityPermission(IHudTabsObserver observer, HudTabsHandler.HudTab currentActiveTab, HudTabsHandler.HudTab requestedTab, bool isActive)
    {
        if (requestedTab != HudTabsHandler.HudTab.TabRocketController)
            return;

        GlobalFunctions.CanvasGroupActivity(_canvasGroup, isActive);

        PlayAnimation(isActive);
    }

    private void PlayAnimation(bool play)
    {
        if (play)
            _animator.Play("FadeAnim", 0, 0);
    }
}

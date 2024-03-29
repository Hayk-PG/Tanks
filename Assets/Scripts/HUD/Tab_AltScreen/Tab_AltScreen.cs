using UnityEngine;

public class Tab_AltScreen : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup _canvasGroup;

    [SerializeField] [Space]
    private Animator _animator;



    private void OnEnable() => GameSceneObjectsReferences.HudTabsHandler.onRequestTabActivityPermission += HudTabsHandler_onRequestTabActivityPermission;

    private void OnDisable() => GameSceneObjectsReferences.HudTabsHandler.onRequestTabActivityPermission -= HudTabsHandler_onRequestTabActivityPermission;

    private void HudTabsHandler_onRequestTabActivityPermission(IHudTabsObserver observer, HudTabsHandler.HudTab currentActiveTab, HudTabsHandler.HudTab requestedTab, bool isActive)
    {
        if (requestedTab != HudTabsHandler.HudTab.TabRocketController && requestedTab != HudTabsHandler.HudTab.TabRemoteControl)
            return;

        GlobalFunctions.CanvasGroupActivity(_canvasGroup, isActive);

        PlayAnimation(isActive);

        PlaySoundFx(isActive);
    }

    private void PlayAnimation(bool isActive)
    {
        if (!isActive)
            return;

        _animator.Play("FadeAnim", 0, 0);
    }

    private void PlaySoundFx(bool isActive)
    {
        if (!isActive)
            return;

        UISoundController.PlaySound(1, 4);
    }
}

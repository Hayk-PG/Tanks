using UnityEngine;

public class HudMainTab : MonoBehaviour, IHudTabsObserver
{
    [SerializeField]
    private CanvasGroup _canvasGroup;



    public void Execute(bool isActive)
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, isActive);
    }
}

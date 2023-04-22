using UnityEngine;

public class AmmoTab : MonoBehaviour, IHudTabsObserver
{
    [SerializeField]
    private CanvasGroup _canvasGroup;


    public void Execute(bool isActive) => GlobalFunctions.CanvasGroupActivity(_canvasGroup, isActive);
}

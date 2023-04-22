using UnityEngine;

public class Tab_PopUp : MonoBehaviour
{
    public enum PopUpType { OfflineModeReminder }

    [SerializeField]
    private CanvasGroup _tabCanvasGroup;

    [SerializeField] [Space]
    private CanvasGroup[] _canvasGroups;



    public void Display(PopUpType popUpType)
    {
        CloseAllCanvasGroups();

        ControlCanvasGroup(_tabCanvasGroup, true);

        switch (popUpType)
        {
            case PopUpType.OfflineModeReminder: ControlCanvasGroup(_canvasGroups[(int)popUpType], true);  break;
        }
    }

    private void ControlCanvasGroup(CanvasGroup canvasGroup, bool isActive)
    {
        GlobalFunctions.CanvasGroupActivity(canvasGroup, isActive);
    }

    private void CloseAllCanvasGroups()
    {
        GlobalFunctions.Loop<CanvasGroup>.Foreach(_canvasGroups,

            canvasGroup =>
            {
                GlobalFunctions.CanvasGroupActivity(canvasGroup, false);
            });
    }
}

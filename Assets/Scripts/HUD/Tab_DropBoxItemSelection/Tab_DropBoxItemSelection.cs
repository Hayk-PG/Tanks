using UnityEngine;

public class Tab_DropBoxItemSelection :MonoBehaviour
{
    [SerializeField]
    private CanvasGroup _canvasGroup;

    [SerializeField] [Space]
    private DropBoxItemSelectionPanel _dropBoxItemsSelectionPanel;



    public void Execute()
    {

    }

    public void SetActivity(bool isActive)
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, isActive);
    }
}

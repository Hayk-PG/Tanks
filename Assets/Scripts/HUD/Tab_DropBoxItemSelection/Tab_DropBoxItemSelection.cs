using UnityEngine;
using System;

public class Tab_DropBoxItemSelection :MonoBehaviour
{
    [SerializeField] [Space]
    private CanvasGroup _canvasGroup;

    [SerializeField] [Space]
    private DropBoxItemSelectionPanel _dropBoxItemsSelectionPanel;

    public event Action<bool> onDropBoxItemSelectionTabActivity;



    public void SetActivity(bool isActive)
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, isActive);

        onDropBoxItemSelectionTabActivity?.Invoke(isActive);
    }
}

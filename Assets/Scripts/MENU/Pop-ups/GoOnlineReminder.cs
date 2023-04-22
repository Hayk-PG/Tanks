using UnityEngine;
using UnityEngine.UI;

public class GoOnlineReminder : MonoBehaviour
{
    [SerializeField] [Space]
    private CanvasGroup _canvasGroup, _canvasGroupPopUpTab;

    [SerializeField] [Space]
    private Btn _btn;

    [SerializeField] [Space]
    private Toggle _toggle;


    private void OnEnable() => _btn.onSelect += OnSelect;

    private void OnDisable() => _btn.onSelect -= OnSelect;

    private void OnSelect()
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, false);
        GlobalFunctions.CanvasGroupActivity(_canvasGroupPopUpTab, false);
    }

    public void OnToggle()
    {
        int value = !_toggle.isOn ? 0 : 1;

        Data.Manager.SetData(new Data.NewData { GoOnlineReminder = value });
    }
}

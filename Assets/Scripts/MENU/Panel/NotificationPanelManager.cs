using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class NotificationPanelManager : MonoBehaviour
{
    public enum InteractionType { Execute, Close, ExecuteAndClose}

    [SerializeField]
    private InteractionType _interactionTypeMain, _interactionTypeSecond;

    [SerializeField] [Space]
    private CanvasGroup _canvasGroup;

    [SerializeField] [Space]
    private TMP_Text _txt;

    [SerializeField] [Space]
    private Btn _btnMain, _btnSecond;

    [SerializeField] [Space]
    private BtnTxt _btnTxtMain, _btnTxtSecond;

    [SerializeField] [Space]
    private UnityEvent _unityEventMain, _unityEventSecond;

    public bool IsActive => _canvasGroup.interactable;





    private void OnEnable()
    {
        _btnMain.onSelect += delegate { OnSelect(_interactionTypeMain, _unityEventMain, false); };

        if (_btnSecond != null)
            _btnSecond.onSelect += delegate { OnSelect(_interactionTypeSecond, _unityEventSecond, false); };
    }

    public void Set(InteractionType? mainInteractionType = null, InteractionType? secondInteractionType = null, string text = "", string btnTxtMain = "", string btnTxtSecond = "")
    {
        if(mainInteractionType == null)
        {
            SetActive(false);

            return;
        }

        _interactionTypeMain = mainInteractionType.Value;

        _interactionTypeSecond = secondInteractionType ?? InteractionType.Close;

        _txt.text = text;

        _btnTxtMain?.SetButtonTitle(btnTxtMain);

        _btnTxtSecond?.SetButtonTitle(btnTxtSecond);

        SetActive(true);
    }

    private void OnSelect(InteractionType interactType, UnityEvent unityEvent, bool isActive)
    {
        switch (interactType)
        {
            case InteractionType.Execute:
                Execute(unityEvent);
                break;

            case InteractionType.Close:
                SetActive(isActive);
                break;

            case InteractionType.ExecuteAndClose:
                Execute(unityEvent);
                SetActive(isActive);
                break;

        }
    }

    private void Execute(UnityEvent unityEvent)
    {
        unityEvent?.Invoke();
    }

    private void SetActive(bool isActive)
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, isActive);
    }
}

using UnityEngine;
using UnityEngine.UI;


public abstract class OptionsController : MonoBehaviour, IReset, ITabOperation
{
    [SerializeField]
    protected Btn _btn;

    [SerializeField]
    protected Image _icon;

    [SerializeField]
    protected BtnTxt _btnTxt;

    [SerializeField]
    protected Options _options;

    [SerializeField]
    protected TabLoading _tabLoading;

    protected bool _isSelected;

    public ITabOperation This { get; set; }
    public ITabOperation OperationHandler { get; set; }



    protected virtual void OnEnable()
    {
        _btn.onSelect += Select;

        _btn.onDeselect += Deselect;

        _options.onOptionsActivity += GetOptionsActivity;

        if(TabsOperation.Handler != null)
            TabsOperation.Handler.onOperationSubmitted += OnOperationSubmitted;
    }

    protected virtual void OnDisable()
    {
        _btn.onSelect -= Select;

        _btn.onDeselect -= Deselect;

        _options.onOptionsActivity -= GetOptionsActivity;

        if (TabsOperation.Handler != null)
            TabsOperation.Handler.onOperationSubmitted -= OnOperationSubmitted;
    }

    protected abstract void Select();

    protected virtual void Deselect()
    {

    }

    protected virtual void GetOptionsActivity(bool isActive)
    {

    }

    protected virtual void OnOperationSubmitted(ITabOperation handler, TabsOperation.Operation operation, object[] data)
    {

    }

    protected virtual void SetOptionsActivity(bool isActive) => _options.Activity(isActive);

    protected virtual void OpenTabLoad() => _tabLoading?.Open();

    public virtual void SetDefault() => _tabLoading?.Close();

    public virtual void OnOperationSucceded()
    {

    }

    public virtual void OnOperationFailed()
    {
        
    }
}

using UnityEngine;
using UnityEngine.UI;

public abstract class OptionsController : MonoBehaviour, IReset
{
    [SerializeField]
    protected Btn _btn;

    [SerializeField]
    protected Image _icon;

    [SerializeField]
    protected Options _options;

    [SerializeField]
    protected TabLoading _tabLoading;

    protected bool _isSelected;



    protected virtual void OnEnable()
    {
        _btn.onSelect += Select;
        _btn.onDeselect += Deselect;
        _options.onOptionsActivity += GetOptionsActivity;
    }

    protected virtual void OnDisable()
    {
        _btn.onSelect -= Select;
        _btn.onDeselect -= Deselect;
        _options.onOptionsActivity -= GetOptionsActivity;
    }

    protected abstract void Select();

    protected virtual void Deselect()
    {

    }

    protected virtual void GetOptionsActivity(bool isActive)
    {

    }

    protected virtual void SetOptionsActivity(bool isActive) => _options.Activity(isActive);

    protected virtual void OpenTabLoad() => _tabLoading?.Open();

    public virtual void SetDefault() => _tabLoading?.Close();
}

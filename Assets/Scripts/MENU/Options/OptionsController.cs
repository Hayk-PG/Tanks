using UnityEngine;
using UnityEngine.UI;

public abstract class OptionsController : MonoBehaviour, IReset
{
    protected Btn _btn;
    protected Image _icon;
    protected Options _options;
    protected MyPhotonCallbacks _myPhotonCallbacks;
    protected TabLoading _tabLoading;

    protected bool _isSelected;



    protected virtual void Awake()
    {
        _btn = Get<Btn>.From(gameObject);
        _icon = Get<Image>.From(Get<Btn_Icon>.FromChild(gameObject).gameObject);
        _options = Get<Options>.From(gameObject);
        _myPhotonCallbacks = FindObjectOfType<MyPhotonCallbacks>();
        _tabLoading = Get<TabLoading>.FromChild(FindObjectOfType<Tab_Options>().gameObject);
    }

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

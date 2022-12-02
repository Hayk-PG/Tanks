using UnityEngine;
using UnityEngine.UI;

public abstract class OptionsController : MonoBehaviour, IReset
{
    protected Btn _btn;
    protected Image _icon;
    protected Options _options;


    protected virtual void Awake()
    {
        _btn = Get<Btn>.From(gameObject);
        _icon = Get<Image>.From(Get<Btn_Icon>.FromChild(gameObject).gameObject);
        _options = Get<Options>.From(gameObject);
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

    public virtual void SetDefault()
    {
        
    }
}

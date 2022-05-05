using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BaseButtonWithUnityEvent : MonoBehaviour
{
    protected Button _button;
    [SerializeField] protected UnityEvent _onClickEvent;


    protected virtual void Awake()
    {
        _button = Get<Button>.From(gameObject);
    }

    public virtual void OnClick()
    {
        _onClickEvent?.Invoke();
    }
}

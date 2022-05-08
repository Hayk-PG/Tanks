using System;
using UnityEngine;

public class Tab_Base<T> : MonoBehaviour where T: MonoBehaviour
{
    public CanvasGroup CanvasGroup { get; private set; }
    public Action OnTabOpened { get; set; }
    protected T _object;


    protected virtual void Awake()
    {
        CanvasGroup = Get<CanvasGroup>.From(gameObject);
        _object = FindObjectOfType<T>();
    }

    public virtual void OpenTab()
    {
        MenuTabs.Activity(CanvasGroup);
        OnTabOpened?.Invoke();
    }
}

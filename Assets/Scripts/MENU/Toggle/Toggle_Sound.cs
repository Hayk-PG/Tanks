using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Toggle_Sound : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] protected int _listIndex;
    [SerializeField] protected int[] _clipsIndexes;
    private Toggle _toggle;


    protected virtual void Awake()
    {
        _toggle = Get<Toggle>.From(gameObject);
    }
    
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        int index = _toggle.isOn ? 1 : 0;
        UISoundController.PlaySound(_listIndex, index);
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        
    }
}

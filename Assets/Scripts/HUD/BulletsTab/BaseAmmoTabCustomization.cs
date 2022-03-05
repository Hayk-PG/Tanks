using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseAmmoTabCustomization<T, T1> : MonoBehaviour
{
    public CanvasGroup _container;

    [SerializeField]
    protected T _buttonPrefab;

    protected List<T> _instantiatedButtons = new List<T>();

    [SerializeField]
    protected Sprite _clicked, _released;

    [SerializeField]
    protected T1[] _parameters;

    public Action OnAmmoTypeController { get; set; }
}

using UnityEngine;
using TMPro;
using System;


[RequireComponent(typeof(Btn))]

public class BtnTxt : MonoBehaviour, IReset
{
    private TMP_Text _txt;
    private Btn _btn;

    [SerializeField] private string _btnTitle;
    [SerializeField] private Color _clrPressed;
    private Color _clrReleased;


    private void Awake()
    {
        _txt = Get<TMP_Text>.FromChild(gameObject);
        _btn = Get<Btn>.From(gameObject);
        SetButtonTitle();
        CacheTextDefaultLook();
    }

    private void OnEnable()
    {
        _btn._onClick += delegate { ChangeTextColor(_clrPressed); };
    }

    private void OnDisable()
    {
        _btn._onClick -= delegate { ChangeTextColor(_clrPressed); };
    }

    private void SetButtonTitle()
    {
        _txt.text = String.IsNullOrEmpty(_btnTitle) ? "button" : _btnTitle;
    }

    private void CacheTextDefaultLook()
    {
        _clrReleased = _txt.color;
    }

    private void ChangeTextColor(Color color)
    {
        _txt.color = color;
    }

    public void SetDefault()
    {
        ChangeTextColor(_clrReleased);
    }
}

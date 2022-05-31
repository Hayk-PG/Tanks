using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RequiredItemsBar : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private Text _textInfo;
    [SerializeField] private Toggle _toggleChecked;

    public Sprite Icon
    {
        get => _icon.sprite;
        set => _icon.sprite = value;
    }
    public string Info
    {
        get => _textInfo.text;
        set => _textInfo.text = value;
    }
    public bool IsChecked
    {
        get => _toggleChecked.isOn;
        set => _toggleChecked.isOn = value;
    }
}

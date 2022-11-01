using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SubTabsButton : MonoBehaviour
{
    private Button _button;
    private Image _buttonImage;
    private TMP_Text _buttonText;

    private SubTabsButton[] _siblings;

    [SerializeField] private string _title;
    [SerializeField] private Sprite _selectedSprite;
    private Sprite _releasedSprite;

    public bool IsSelected { get; private set; }
    public bool IsInteractable { get => _button.interactable; set => _button.interactable = value; }

    public event Action onSelect;
    public event Action onDeselect;


    private void Awake()
    {
        _button = Get<Button>.From(gameObject);
        _buttonImage = Get<Image>.From(gameObject);
        _buttonText = Get<TMP_Text>.FromChild(gameObject);

        _releasedSprite = _buttonImage.sprite;
    }

    private void Start()
    {
        GetSiblings();
        SetTitle();
    }

    private void GetSiblings()
    {
        _siblings = new SubTabsButton[transform.parent.childCount - 1];

        for (int i = 0, j = 0;  i < transform.parent.childCount; i++)
        {
            if(Get<SubTabsButton>.From(transform.parent.GetChild(i).gameObject) != this)
            {
                _siblings[j] = Get<SubTabsButton>.From(transform.parent.GetChild(i).gameObject);
                j++;
            }
        }
    }

    private void SetTitle()
    {
        _buttonText.text = String.IsNullOrEmpty(_title) ? "Unspecified" : _title;
    }

    public void Click()
    {
        SetActivity(true);
        DeselectSiblings();
    }

    public void SetActivity(bool isSelected)
    {
        if (isSelected == IsSelected)
            return;

        IsSelected = isSelected;
        _buttonImage.sprite = isSelected ? _selectedSprite : _releasedSprite;

        if(isSelected)
            onSelect?.Invoke();
        else
            onDeselect?.Invoke();
    }

    private void DeselectSiblings()
    {
        GlobalFunctions.Loop<SubTabsButton>.Foreach(_siblings, sibling => { sibling.SetActivity(false); });
    }
}

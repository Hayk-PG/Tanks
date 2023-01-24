using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Btn_Tank : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Button _button;
    [SerializeField] private Image _imgTank;
    [SerializeField] private Image _imgLock;
    [SerializeField] private TMP_Text _txtName;
    [SerializeField] private TMP_Text _txtLevel;
    [SerializeField] private Stars _stars;

    private int _relatedTankIndex;
    public Button Button { get => _button; }
    public Image ImageTank { get => _imgTank; }
    public Sprite SpriteButton { get => _button.image.sprite; internal set => _button.image.sprite = value; }


    public event Action<int, int> _onAutoSelect;
    public event Action<int> _onClick;


    private void Start()
    {
        _button.onClick.AddListener(() => _onClick?.Invoke(_relatedTankIndex));
    }

    public void SetActivity(bool isActive)
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, isActive);
    }

    public void SetPicture(Sprite sprite)
    {
        _imgTank.sprite = sprite;
    }

    public void SetName(string name)
    {
        _txtName.text = name;
    }

    public void SetStars(int stars)
    {
        _stars.Display(stars);
    }

    public void SetRelatedTankIndex(int relatedtankIndex)
    {
        _relatedTankIndex = relatedtankIndex;
    }

    public void SetLevel(int level)
    {
        _txtLevel.text = level.ToString();
    }

    public void AutoSelect(int relatedTankIndex, int horizontalGroupsLength)
    {
        if (_relatedTankIndex == relatedTankIndex)
            _onAutoSelect?.Invoke(relatedTankIndex, horizontalGroupsLength);
    }
}

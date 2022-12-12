using UnityEngine;
using UnityEngine.UI;

public class Button_Long : MonoBehaviour, IReset
{
    private SubTabsButton _subTabButton;

    [SerializeField] private Image _imageLeft, _imageRight;
    [SerializeField] private Sprite _imageLeftSpriteVariation, _imageRightSpriteVariation;
    private Sprite _imageLeftSpriteDefault, _imageRightSpriteDefault;


    private void Awake()
    {
        _subTabButton = Get<SubTabsButton>.From(gameObject);

        _imageLeftSpriteDefault = _imageLeft.sprite;
        _imageRightSpriteDefault = _imageRight.sprite;
    }

    private void OnEnable()
    {
        if (_subTabButton == null)
            return;

        _subTabButton.onSelect += Select;
        _subTabButton.onDeselect += Deselect;
    }

    private void OnDisable()
    {
        if (_subTabButton == null)
            return;

        _subTabButton.onSelect -= Select;
        _subTabButton.onDeselect -= Deselect;
    }

    private void Select()
    {
        ChangeLeftImageSprite(_imageLeftSpriteVariation);
        ChangeRightImageSprite(_imageRightSpriteVariation);
    }

    private void Deselect()
    {
        ChangeLeftImageSprite(_imageLeftSpriteDefault);
        ChangeRightImageSprite(_imageRightSpriteDefault);
    }

    private void ChangeLeftImageSprite(Sprite sprite)
    {
        if (_imageLeft == null || _imageLeftSpriteVariation == null || _imageLeftSpriteDefault == null)
            return;

        _imageLeft.sprite = sprite;
    }

    private void ChangeRightImageSprite(Sprite sprite)
    {
        if (_imageRight == null || _imageRightSpriteVariation == null || _imageRightSpriteDefault == null)
            return;

        _imageRight.sprite = sprite;
    }

    public void SetDefault()
    {
        ChangeLeftImageSprite(_imageLeftSpriteDefault);
        ChangeRightImageSprite(_imageRightSpriteDefault);
        _subTabButton.Deselect();
    }
}

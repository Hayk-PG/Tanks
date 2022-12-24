using UnityEngine;
using UnityEngine.UI;

public class Btn_Icon : MonoBehaviour
{
    private Image _imgIcon;
    private Btn _btn;

    [SerializeField] private Sprite _sprtPressed;
    [SerializeField] private Color _clrPressed;
    private Sprite _sprtReleased;
    private Color _clrReleased;



    private void Awake()
    {
        _imgIcon = Get<Image>.From(gameObject);
        _btn = Get<Btn>.From(gameObject);
        _sprtReleased = _imgIcon.sprite;
        _clrReleased = _imgIcon.color;

        CacheIconDefaultLook();
    }

    private void OnEnable()
    {
        _btn.onSelect += delegate { ChangeIconLook(_btn._buttonClickType, _sprtPressed, _clrPressed); };
        _btn.onDeselect += delegate { ChangeIconLook(_btn._buttonClickType, _sprtReleased, _clrReleased); };
    }

    private void OnDisable()
    {
        _btn.onSelect -= delegate { ChangeIconLook(_btn._buttonClickType, _sprtPressed, _clrPressed); };
        _btn.onDeselect -= delegate { ChangeIconLook(_btn._buttonClickType, _sprtReleased, _clrReleased); };
    }

    private void CacheIconDefaultLook()
    {
        _sprtReleased = _imgIcon.sprite;
        _clrReleased = _imgIcon.color;
    }

    private void ChangeIconLook(Btn.ButtonClickType buttonClickType, Sprite sprite, Color color)
    {
        switch (buttonClickType)
        {
            case Btn.ButtonClickType.ChangeSprite: ChangeIconSprite(sprite); break;
            case Btn.ButtonClickType.ChangeColor: ChangeIconColor(color); break;
            case Btn.ButtonClickType.Both: ChangeIconSprite(sprite); ChangeIconColor(color); break;
        }
    }

    private void ChangeIconSprite(Sprite sprite)
    {
        if (sprite == null)
            return;

        _imgIcon.sprite = sprite;
    }

    private void ChangeIconColor(Color color)
    {
        _imgIcon.color = color;
    }
}

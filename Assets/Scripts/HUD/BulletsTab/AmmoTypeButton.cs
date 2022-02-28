using UnityEngine;
using UnityEngine.UI;

public class AmmoTypeButton : MonoBehaviour
{
    public AmmoStars _ammoStars;

    private AmmoTypeController _ammoTypeController;
    private AmmoTabCustomization _ammoTabCustomization;

    public int ammoTypeIndex;

    [Header("Images")]
    [SerializeField]
    private Image _ammoIcon, ammoButtonSprite;

    [Header("Sprites")]
    [SerializeField]
    private Sprite _clickedSprite, _releasedSprite;

    [Header("Text")]
    [SerializeField]
    private Text _ammoTypeText;

    public Sprite AmmoTypeIcon
    {
        get => _ammoIcon.sprite;
        set => _ammoIcon.sprite = value;
    }
    private Sprite AmmoButtonSprite
    {
        get => ammoButtonSprite.sprite;
        set => ammoButtonSprite.sprite = value;
    }
    public string AmmoTypeName
    {
        get => _ammoTypeText.text;
        set => _ammoTypeText.text = value;
    }

    private bool _isClicked;



    private void Awake()
    {
        _ammoTypeController = FindObjectOfType<AmmoTypeController>();
        _ammoTabCustomization = FindObjectOfType<AmmoTabCustomization>();
    }

    private void Start()
    {
        if (ammoTypeIndex == 0)
            Interact(true);
    }

    public void OnClickButton()
    {
        GlobalFunctions.OnClick(() => _ammoTypeController.OnClickAmmoTypeButton(ammoTypeIndex));
        _ammoTypeController.OnAmmoTabActivity();
        ButtonIsClicked();
    }

    private void ButtonIsClicked()
    {
        if (!_isClicked)
        {
            foreach (var button in _ammoTabCustomization._instantiatedAmmoTypeButtons)
            {
                button.Interact(false);
            }

            Interact(true);
        }
    }

    public void Interact(bool isClicked)
    {
        _isClicked = isClicked;

        if (_isClicked)
            AmmoButtonSprite = _clickedSprite;
        else
            AmmoButtonSprite = _releasedSprite;
    }
}

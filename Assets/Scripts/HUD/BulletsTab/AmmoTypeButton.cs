using UnityEngine;
using UnityEngine.UI;

public class AmmoTypeButton : MonoBehaviour
{
    private AmmoTypeController _ammoTypeController;
    private AmmoTabCustomization _ammoTabCustomization;

    public int ammoTypeIndex;

    private RectTransform _rectTransform;
    private Outline _outline;

    private Vector2 _unclickedSize;
    private Vector2 _clickedSize;

    private bool _isClicked;

    private void Awake()
    {
        _ammoTypeController = FindObjectOfType<AmmoTypeController>();
        _ammoTabCustomization = FindObjectOfType<AmmoTabCustomization>();

        _rectTransform = GetComponent<RectTransform>();
        _outline = Get<Outline>.From(gameObject);

        _unclickedSize = _rectTransform.sizeDelta;
        _clickedSize = new Vector2(_unclickedSize.x / 1.1f, _unclickedSize.y / 1.1f);
    }

    private void Start()
    {
        if (ammoTypeIndex == 0)
            Interact(_clickedSize, true);
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
                button.Interact(_unclickedSize, false);
            }

            Interact(_clickedSize, true);
        }
    }

    public void Interact(Vector2 size, bool isClicked)
    {
        _rectTransform.sizeDelta = size;
        _isClicked = isClicked;

        if (_outline != null) _outline.enabled = isClicked;
    }
}

using UnityEngine;

public class AmmoTypeController : MonoBehaviour
{
    private AmmoTabButton _ammoTabButton;
    private Animator _animator;
    private RectTransform _rectTransform;

    private const string _play = "play";
    private const string _direction = "speed";

    private float _animatorSpeed;

    private AmmoTabCustomization _ammoTabCustomization;
    private SupportsTabCustomization _supportTabCustomization;


    private void Awake()
    {
        _ammoTabButton = FindObjectOfType<AmmoTabButton>();
        _animator = GetComponent<Animator>();
        _rectTransform = GetComponent<RectTransform>();

        _ammoTabCustomization = Get<AmmoTabCustomization>.From(gameObject);
        _supportTabCustomization = Get<SupportsTabCustomization>.From(gameObject);
    }

    private void OnEnable()
    {
        _ammoTabButton.OnAmmoTabActivity += OnAmmoTabActivity;
        _ammoTabCustomization.OnAmmoTypeController += OnAmmoTabActivity;
        _supportTabCustomization.OnAmmoTypeController += OnAmmoTabActivity;
    }

    private void OnDisable()
    {
        _ammoTabButton.OnAmmoTabActivity -= OnAmmoTabActivity;
        _ammoTabCustomization.OnAmmoTypeController -= OnAmmoTabActivity;
        _supportTabCustomization.OnAmmoTypeController -= OnAmmoTabActivity;
    }

    public void OnAmmoTabActivity()
    {
        _animatorSpeed = _rectTransform.anchoredPosition.x > 0 ? 1 : -1;
        _animator.SetFloat(_direction, _animatorSpeed);
        _animator.SetTrigger(_play);
    }
}

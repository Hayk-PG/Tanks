using System;
using UnityEngine;
using UnityEngine.UI;

public class CurrentWeaponStatus : MonoBehaviour
{
    private AmmoTabCustomization _ammoTabCustomization;
    private Animator _animator;

    [Serializable]
    private struct Properties
    {
        [SerializeField]
        private Image _currentWeaponIcon;

        [SerializeField]
        private Text _bulletsLeftText;

        public Sprite CurrentWeaponIcon
        {
            get => _currentWeaponIcon.sprite;
            set => _currentWeaponIcon.sprite = value;
        }
        public string Text
        {
            get => _bulletsLeftText.text;
            set => _bulletsLeftText.text = value;
        }
    }

    [SerializeField]
    private Properties _properties;

   

    private void Awake()
    {
        _ammoTabCustomization = FindObjectOfType<AmmoTabCustomization>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        _ammoTabCustomization.OnUpdateDisplayedWeapon += OnUpdateDisplayedWeapon;
    }

    private void OnDisable()
    {
        _ammoTabCustomization.OnUpdateDisplayedWeapon -= OnUpdateDisplayedWeapon;
    }

    private void OnUpdateDisplayedWeapon(WeaponProperties weaponProperty, int bulletsLeft)
    {
        _properties.CurrentWeaponIcon = weaponProperty._icon;
        _properties.Text = bulletsLeft < 100 ? bulletsLeft.ToString() : "∞";

        if (_animator != null)
            _animator.SetTrigger(Names.Play);
    }
}

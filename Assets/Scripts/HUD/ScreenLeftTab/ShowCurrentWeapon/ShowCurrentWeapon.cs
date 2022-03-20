﻿using System;
using UnityEngine;
using UnityEngine.UI;

public class ShowCurrentWeapon : MonoBehaviour
{
    private AmmoTabCustomization _ammoTabCustomization;

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
        public int BulletsLeft
        {
            get
            {
                if (int.TryParse(_bulletsLeftText.text, out int bulletsLeft)) return bulletsLeft;
                else return 0;
            }
            set
            {
                _bulletsLeftText.text = value.ToString();
            }
        }
    }

    [SerializeField]
    private Properties _properties;

   

    private void Awake()
    {
        _ammoTabCustomization = FindObjectOfType<AmmoTabCustomization>();
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
        _properties.BulletsLeft = bulletsLeft;
    }
}

using UnityEngine;
using System;

public class PlayerAmmoType : MonoBehaviour
{
    private ShootController _shootController;
    private AmmoTabCustomization _ammoTabCustomization;

    public BulletController[] _bulletsPrefab;


    private void Awake()
    {
        _shootController = Get<ShootController>.From(gameObject);
        _ammoTabCustomization = FindObjectOfType<AmmoTabCustomization>();
    }

    private void Start()
    {
        _ammoTabCustomization.CallOnInstantiateAmmoTypeButton();
    }

    private void OnEnable()
    {
        _ammoTabCustomization.OnInstantiateAmmoTypeButton += OnInstantiateAmmoTypeButton;
        _ammoTabCustomization.OnSendActiveAmmoToPlayer += OnGetActiveAmmoType;
    }
   
    private void OnDisable()
    {
        _ammoTabCustomization.OnInstantiateAmmoTypeButton -= OnInstantiateAmmoTypeButton;
        _ammoTabCustomization.OnSendActiveAmmoToPlayer -= OnGetActiveAmmoType;
    }

    private void OnInstantiateAmmoTypeButton(Action<int> obj)
    {
        for (int i = 0; i < _bulletsPrefab.Length; i++)
        {
            obj?.Invoke(i);
        }
    }

    private void OnGetActiveAmmoType(AmmoTypeButton ammoTypeButton)
    {
        _shootController.ActiveAmmoIndex = ammoTypeButton._properties.Index < _bulletsPrefab.Length ?
                                           ammoTypeButton._properties.Index : _bulletsPrefab.Length - 1;
    }
}

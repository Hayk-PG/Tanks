using UnityEngine;
using System;

public class PlayerAmmoType : MonoBehaviour
{
    private ShootController _shootController;
    private AmmoTabCustomization _ammoTabCustomization;
    private AmmoTypeController _ammoTypeController;

    public BulletController[] _bulletsPrefab;


    private void Awake()
    {
        _shootController = Get<ShootController>.From(gameObject);
        _ammoTabCustomization = FindObjectOfType<AmmoTabCustomization>();
        _ammoTypeController = FindObjectOfType<AmmoTypeController>();
    }

    private void Start()
    {
        _ammoTabCustomization.CallOnInstantiateAmmoTypeButton();
    }

    private void OnEnable()
    {
        _ammoTabCustomization.OnInstantiateAmmoTypeButton += OnInstantiateAmmoTypeButton;
        _ammoTypeController.OnGetActiveAmmoType += OnGetActiveAmmoType;
    }
   
    private void OnDisable()
    {
        _ammoTabCustomization.OnInstantiateAmmoTypeButton -= OnInstantiateAmmoTypeButton;
        _ammoTypeController.OnGetActiveAmmoType -= OnGetActiveAmmoType;
    }

    private void OnInstantiateAmmoTypeButton(Action<int> obj)
    {
        for (int i = 0; i < _bulletsPrefab.Length; i++)
        {
            obj?.Invoke(i);
        }
    }

    private void OnGetActiveAmmoType(int index)
    {
        _shootController.ActiveAmmoIndex = index < _bulletsPrefab.Length ? index : _bulletsPrefab.Length - 1;
    }
}

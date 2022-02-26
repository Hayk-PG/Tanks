using System;
using System.Collections.Generic;
using UnityEngine;

public class AmmoTabCustomization : MonoBehaviour
{
    [SerializeField]
    private Transform _container;

    [SerializeField]
    private AmmoTypeButton _ammoTypeButtonPrefab;

    [HideInInspector]
    public List<AmmoTypeButton> _instantiatedAmmoTypeButtons; 
    public event Action<Action<int>> OnInstantiateAmmoTypeButton;


    public void CallOnInstantiateAmmoTypeButton()
    {
        OnInstantiateAmmoTypeButton?.Invoke(delegate (int index) { InstantiateAmmoTypeButton(index); });
    }

    public void InstantiateAmmoTypeButton(int index)
    {
        AmmoTypeButton button = Instantiate(_ammoTypeButtonPrefab, _container);        
        button.ammoTypeIndex = index;
        CacheAmmoTypeButtons(button);
    }

    private void CacheAmmoTypeButtons(AmmoTypeButton button)
    {
        Conditions<AmmoTypeButton[]>.CheckNull(_instantiatedAmmoTypeButtons, ()=> InitializeList(button), () => AddToList(button));
    }
    
    private void InitializeList(AmmoTypeButton button)
    {
        _instantiatedAmmoTypeButtons = new List<AmmoTypeButton>(1) { button };
    }

    private void AddToList(AmmoTypeButton button)
    {
        _instantiatedAmmoTypeButtons.Add(button);
    }
}

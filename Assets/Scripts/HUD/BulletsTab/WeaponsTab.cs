using System;
using UnityEngine;

public class WeaponsTab : MonoBehaviour
{
    [SerializeField]
    private WeaponsTabButton[] _weaponsTabButtons;

    [SerializeField] [Space]
    private CanvasGroup[] _tabs;

    [SerializeField] [Space]
    private AmmoTabCustomization _ammoTabCustomization;

    [SerializeField] [Space]
    private SupportsTabCustomization _supportsTabCustomization;
    
    [SerializeField] [Space]
    private Color _pressedColor, _releasedColor;
    
    private CanvasGroup[] _containers;

    private const int _length = 2;
    private const string WeaponsTabButton = "Weapons";
    private const string SupportTabButton = "Support";
  
    private string[] _weaponsTabButtonsName;
    
    


    private void Awake()
    {
        _weaponsTabButtonsName = new string[_length] 
        {
            WeaponsTabButton,
            SupportTabButton
        };
        _containers = new CanvasGroup[_length]
        {
            _ammoTabCustomization._container,
            _supportsTabCustomization._container
        };
    }

    private void Start()
    {
        ActivateWeaponsTabButtons();
    }

    private void OnDisable()
    {
        ActiveWeaponsTabButtonsLength(Index => { UnsuscribeFromWeaponsTabButtonEvents(Index); });
    }

    private void ActiveWeaponsTabButtonsLength(Action<int> OnLoop)
    {
        for (int i = 0; i < _length; i++)
        {
            OnLoop?.Invoke(i);
        }
    }

    private void ActivateWeaponsTabButtons()
    {
        ActiveWeaponsTabButtonsLength(Index => 
        {
            _weaponsTabButtons[Index].gameObject.SetActive(true);
            _weaponsTabButtons[Index].ButtonName = _weaponsTabButtonsName[Index];
            _weaponsTabButtons[Index].RelatedContainer = _containers[Index];

            SubscribeToWeaponsTabButtonEvents(Index);
        });
    }

    private void SubscribeToWeaponsTabButtonEvents(int index)
    {
        _weaponsTabButtons[index].OnWeaponsTabButtonClicked += OnWeaponsTabButtonClicked;
    }

    private void UnsuscribeFromWeaponsTabButtonEvents(int index)
    {
        if (_weaponsTabButtons[index] != null)
            _weaponsTabButtons[index].OnWeaponsTabButtonClicked -= OnWeaponsTabButtonClicked;
    }

    private void OnWeaponsTabButtonClicked(WeaponsTabButton weaponsTabButton)
    {
        ActiveWeaponsTabButtonsLength(Index => 
        {
            _weaponsTabButtons[Index].Color = _releasedColor;
            GlobalFunctions.CanvasGroupActivity(_containers[Index], false);

        });

        weaponsTabButton.Color = _pressedColor;
        GlobalFunctions.CanvasGroupActivity(weaponsTabButton.RelatedContainer, true);
    }
}

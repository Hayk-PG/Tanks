using System;
using UnityEngine;

public class NewWeaponFromWoodBox : MonoBehaviour
{
    [SerializeField]
    private AmmoTabCustomization _ammoTabCustomization;

    [SerializeField] [Space]
    private AmmoTabButtonNotification _ammoTabButtonNotification;

    [SerializeField] [Space]
    private GameManager _gameManager;

    private ScoreController _localPlayerScoreController;
    private GameObject _localPlayerGameObject;
    private AddNewWeaponContent _addNewWeaponContent;

    public Action<WeaponProperties> OnAddNewWeaponFromWoodBox { get; set; }




    private void OnEnable()
    {
        _gameManager.OnGameStarted += OnGameStarted;
    }

    private void OnDisable()
    {
        _gameManager.OnGameStarted -= OnGameStarted;

        if (_addNewWeaponContent != null)
            _addNewWeaponContent.OnNewWeaponTaken -= OnNewWeaponTaken;
    }

    private void OnGameStarted()
    {
        _localPlayerGameObject = GlobalFunctions.ObjectsOfType<TankController>.Find(t => t.BasePlayer != null).gameObject;

        if (_localPlayerGameObject != null)
            _localPlayerScoreController = Get<ScoreController>.From(_localPlayerGameObject);
    }

    public void SubscribeToWoodBoxEvent(AddNewWeaponContent addNewWeaponContent)
    {
        _addNewWeaponContent = addNewWeaponContent;
        addNewWeaponContent.OnNewWeaponTaken += OnNewWeaponTaken;
    }

    private void SetNewWeaponIndex(AmmoTypeButton ammoTypeButton) => ammoTypeButton._properties.Index = _ammoTabCustomization._instantiatedButtons.Count;

    public void InstantiateNewWeapon(WeaponProperties newWeapon)
    {
        _ammoTabCustomization.InstantiatedButton(out AmmoTypeButton button);
        _ammoTabCustomization.UnhideInstantiatedButtonWeaponsTab(button);
        _ammoTabCustomization.AssignProperties(button, newWeapon);
        _ammoTabCustomization.CacheAmmoTypeButtons(button);
        SetNewWeaponIndex(button);

        _ammoTabButtonNotification.CacheWeaponsPointsToUnlock(button);
        _ammoTabButtonNotification.PlayerGetsPoints(_localPlayerScoreController.Score);

        OnAddNewWeaponFromWoodBox?.Invoke(newWeapon);
    }

    private void OnNewWeaponTaken(WeaponProperties newWeapon) => InstantiateNewWeapon(newWeapon);
}

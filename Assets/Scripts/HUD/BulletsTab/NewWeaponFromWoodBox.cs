using System;
using UnityEngine;

public class NewWeaponFromWoodBox : MonoBehaviour
{
    private AmmoTabCustomization _ammoTabCustomization;
    private AmmoTabButtonNotification _ammoTabButtonNotification;
    private GameManager _gameManager;
    private ScoreController _localPlayerScoreController;
    private GameObject _localPlayerGameObject;
    private WoodBox _woodBox;

    public Action<WeaponProperties> OnAddNewWeaponFromWoodBox { get; set; }


    private void Awake()
    {
        _ammoTabCustomization = Get<AmmoTabCustomization>.From(gameObject);
        _ammoTabButtonNotification = FindObjectOfType<AmmoTabButtonNotification>();
        _gameManager = FindObjectOfType<GameManager>();
    }

    private void OnEnable()
    {
        _gameManager.OnGameStarted += OnGameStarted;
    }

    private void OnDisable()
    {
        _gameManager.OnGameStarted -= OnGameStarted;

        if(_woodBox != null)
            _woodBox.OnNewWeaponTaken += OnNewWeaponTaken;
    }

    private void OnGameStarted()
    {
        _localPlayerGameObject = GlobalFunctions.ObjectsOfType<TankController>.Find(t => t.BasePlayer != null).gameObject;

        if (_localPlayerGameObject != null)
            _localPlayerScoreController = Get<ScoreController>.From(_localPlayerGameObject);
    }

    public void SubscribeToWoodBoxEvent(WoodBox woodBox)
    {
        _woodBox = woodBox;
        _woodBox.OnNewWeaponTaken += OnNewWeaponTaken;
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

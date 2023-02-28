using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerScoreAndAmmoCombined : MonoBehaviour
{
    private TankController _tankController;

    private PlayerAmmoType _playerAmmoType;

    private ScoreController _scoreController;

    private IGetPointsAndAmmoDataFromPlayer[] _iGet;


    private void Awake()
    {
        _tankController = Get<TankController>.From(gameObject);

        _playerAmmoType = Get<PlayerAmmoType>.From(gameObject);

        _scoreController = Get<ScoreController>.From(gameObject);

        _iGet = FindObjectsOfType<MonoBehaviour>().OfType<IGetPointsAndAmmoDataFromPlayer>().ToArray();
    }

    private void OnEnable()
    {
        _tankController.OnInitialize += OnInitialize;
    }

    private void OnDisable()
    {
        _tankController.OnInitialize -= OnInitialize;

        if (_iGet != null)
            GlobalFunctions.Loop<IGetPointsAndAmmoDataFromPlayer>.Foreach(_iGet, get => { get.OnGetPointsAndAmmoDataFromPlayer -= GetPointsAndAmmoDataFromPlayer; });
    }

    private void OnInitialize()
    {
        if (_iGet != null)
            GlobalFunctions.Loop<IGetPointsAndAmmoDataFromPlayer>.Foreach(_iGet, get => { get.OnGetPointsAndAmmoDataFromPlayer += GetPointsAndAmmoDataFromPlayer; });
    }

    private void GetPointsAndAmmoDataFromPlayer(Action<int, List<int>> SendPointsAndAmmoData)
    {
        SendPointsAndAmmoData?.Invoke(_scoreController.Score, _playerAmmoType._weaponsBulletsCount);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerScoreAndAmmoCombined : MonoBehaviour
{
    private PlayerAmmoType _playerAmmoType;
    private ScoreController _scoreController;

    private IGetPointsAndAmmoDataFromPlayer[] _iGet;


    private void Awake()
    {
        _playerAmmoType = GetComponent<PlayerAmmoType>();
        _scoreController = GetComponent<ScoreController>();

        _iGet = FindObjectsOfType<MonoBehaviour>().OfType<IGetPointsAndAmmoDataFromPlayer>().ToArray();
    }

    private void OnEnable()
    {
        if(_iGet != null)
        {
            foreach (var get in _iGet)
            {
                get.OnGetPointsAndAmmoDataFromPlayer += GetPointsAndAmmoDataFromPlayer;
            }
        }
    }

    private void OnDisable()
    {
        if (_iGet != null)
        {
            foreach (var get in _iGet)
            {
                get.OnGetPointsAndAmmoDataFromPlayer -= GetPointsAndAmmoDataFromPlayer;
            }
        }
    }

    private void GetPointsAndAmmoDataFromPlayer(Action<int, List<int>> SendPointsAndAmmoData)
    {
        SendPointsAndAmmoData?.Invoke(_scoreController.Score, _playerAmmoType._weaponsBulletsCount);
    }
}

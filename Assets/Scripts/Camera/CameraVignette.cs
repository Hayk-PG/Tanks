using System.Collections;
using UnityEngine;

public class CameraVignette : BaseCameraFX
{
    [SerializeField] 
    private GameplayAnnouncer _gameplayAnnouncer;

    [SerializeField]
    protected PlayerDamageCameraFX _playerDamageCameraFX;

    private float _dmg;


    private void Start() => _pp.VignetteAmount = 1;

    private void OnEnable()
    {
        _gameplayAnnouncer.OnGameStartAnnouncement += delegate { StartCoroutine(ChangeAmount(0.12f, -5)); };

        _playerDamageCameraFX.onDamageFX += OnDamage;

        GameSceneObjectsReferences.BaseEndGame.onEndGame += delegate { _pp.Vignette = false; };
    }

    private void OnDisable()
    {
        _gameplayAnnouncer.OnGameStartAnnouncement -= delegate { StartCoroutine(ChangeAmount(0.12f, -5)); };

        _playerDamageCameraFX.onDamageFX -= OnDamage;
    }

    private void OnDamage(int damage)
    {
        _dmg += damage;

        StartCoroutine(ChangeAmount((_dmg / 100) / 1.2f <= 0.12f ? 0.12f : (_dmg / 100) / 1.2f, -5));
    }

    private IEnumerator ChangeAmount(float targetValue, float lerp)
    {
        float defaultValue = targetValue;

        while (_pp.VignetteAmount > defaultValue)
        {
            _pp.VignetteAmount += lerp * Time.deltaTime;

            yield return null;
        }

        yield return null;

        _pp.VignetteAmount = defaultValue;
    }
}

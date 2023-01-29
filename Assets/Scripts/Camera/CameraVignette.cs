using System.Collections;
using UnityEngine;

public class CameraVignette : BaseCameraFX
{
    [SerializeField] private GameplayAnnouncer _gameplayAnnouncer;


    private void Start() => _pp.VignetteAmount = 1;

    private void OnEnable() => _gameplayAnnouncer.OnGameStartAnnouncement += delegate { StartCoroutine(ChangeAmount()); };

    private void OnDisable() => _gameplayAnnouncer.OnGameStartAnnouncement -= delegate { StartCoroutine(ChangeAmount()); };

    private IEnumerator ChangeAmount()
    {
        float defaultValue = 0.12f;

        while (_pp.VignetteAmount > defaultValue)
        {
            _pp.VignetteAmount -= 5 * Time.deltaTime;
            yield return null;
        }

        yield return null;
        _pp.VignetteAmount = defaultValue;
    }
}

using UnityEngine;
using TMPro;
using System.Collections;

public class StunEffectTimer : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup _canvasGroup;

    [SerializeField] [Space]
    private Animator _animator;

    [SerializeField] [Space]
    private TMP_Text _txtTimer;

    [SerializeField] [Space]
    private GameManager _gameManager;

    private TankController _localTankController;

    private Stun _stun;

    private int _seconds;

    private bool _isCoroutineRunning;




    private void OnEnable()
    {
        _gameManager.OnGameStarted += OnGameStarted;
    }

    private void OnDisable()
    {
        _gameManager.OnGameStarted -= OnGameStarted;

        UnsubscribeFromStunEffectEvent();
    }

    private void OnGameStarted()
    {
        SubscribeToStunEffectEvent();
    }

    private void SubscribeToStunEffectEvent()
    {
        _localTankController = GlobalFunctions.ObjectsOfType<TankController>.Find(tank => tank.BasePlayer != null);

        if (_localTankController == null)
            return;

        _stun = Get<Stun>.FromChild(_localTankController.gameObject);

        if (_stun != null)
            _stun.OnStunEffect += OnStunEffect;
    }

    private void UnsubscribeFromStunEffectEvent()
    {
        if (_stun == null)
            return;

        _stun.OnStunEffect -= OnStunEffect;
    }

    private void OnStunEffect(bool isActive)
    {
        _isCoroutineRunning = isActive;

        GlobalFunctions.CanvasGroupActivity(_canvasGroup, _isCoroutineRunning);

        if (_isCoroutineRunning)
            StartCoroutine(RunCoroutine());
    }

    private IEnumerator RunCoroutine()
    {
        _seconds = 30;

        while (_isCoroutineRunning)
        {
            _animator.SetTrigger("play");

            yield return new WaitForSeconds(1);
        }
    }

    //Animation event
    public void Increment()
    {
        _seconds--;

        _txtTimer.text = _seconds.ToString();
    }
}

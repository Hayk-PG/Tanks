using UnityEngine;
using TMPro;
using System.Collections;

public class StunEffectTimer : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    private Animator _animator;
    private TMP_Text _txtTimer;
    private GameManager _gameManager;
    private TankController[] _tanks;
    private Stun[] _stuns = new Stun[2];

    private int _seconds;
    private bool _isCoroutineRunning;



    private void Awake()
    {
        _canvasGroup = Get<CanvasGroup>.From(gameObject);
        _animator = Get<Animator>.From(gameObject);
        _txtTimer = Get<TMP_Text>.FromChild(gameObject);
        _gameManager = FindObjectOfType<GameManager>();
    }

    private void OnEnable()
    {
        _gameManager.OnGameStarted += OnGameStarted;
    }

    private void OnDisable()
    {
        _gameManager.OnGameStarted -= OnGameStarted;

        GlobalFunctions.Loop<Stun>.Foreach(_stuns, stun => 
        {
            if (stun != null)
                stun.OnStunEffect -= OnStunEffect;
        });
    }

    private void OnGameStarted()
    {
        _tanks = FindObjectsOfType<TankController>(true);

        for (int i = 0; i < _tanks.Length; i++)
        {
            _stuns[i] = Get<Stun>.FromChild(_tanks[i].gameObject);

            if (_stuns[i] != null)
                _stuns[i].OnStunEffect += OnStunEffect;
        }
    }

    private void OnStunEffect(bool isActive)
    {
        _isCoroutineRunning = isActive;
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, _isCoroutineRunning);

        if (_isCoroutineRunning)
        {
            StartCoroutine(RunCoroutine());
        }
    }

    private IEnumerator RunCoroutine()
    {
        _seconds = 0;

        while (_isCoroutineRunning)
        {
            _animator.SetTrigger("play");
            yield return new WaitForSeconds(1);
        }
    }

    //Animation event
    public void Increment()
    {
        _seconds++;
        _txtTimer.text = _seconds.ToString();
    }
}

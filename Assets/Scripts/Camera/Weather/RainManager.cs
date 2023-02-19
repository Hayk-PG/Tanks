using System.Collections;
using UnityEngine;

public class RainManager : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _rain;

    [SerializeField] [Space]
    private GameManager _gameManager;

    [SerializeField] [Space]
    private bool _isRaining, _stop;

    private float _delay;

    public bool IsRaining
    {
        get => _rain.isPlaying;
    }

    public event System.Action<bool> onRainActivity;



    private void OnEnable()
    {
        _gameManager.OnGameStarted += delegate { StartCoroutine(ControlRainActivity()); };
    }

    private void OnDisable()
    {
        _gameManager.OnGameStarted -= delegate { StartCoroutine(ControlRainActivity()); };
    }

    //private void Update()
    //{
    //    if (_gameManager.IsGameStarted)
    //    {
    //        if (_isPlaying)
    //        {
    //            _rain.Play();

    //            onRainActivity?.Invoke(true);

    //            _isPlaying = false;
    //        }

    //        if (_stop)
    //        {
    //            _rain.Stop();

    //            onRainActivity?.Invoke(false);

    //            _stop = false;
    //        }
    //    }
    //}

    private IEnumerator ControlRainActivity()
    {
        yield return null;

        while (!_gameManager.IsGameEnded)
        {
            yield return StartCoroutine(ChangeRainActivity());
            yield return StartCoroutine(ControlIteration());
        }
    }

    private IEnumerator ChangeRainActivity()
    {
        Conditions<bool>.Compare(_isRaining, () => { _rain.Play(); }, () => { _rain.Stop(); });

        onRainActivity?.Invoke(_isRaining);

        yield return null;
    }

    private IEnumerator ControlIteration()
    {
        _isRaining = Random.Range(0, 5) < 2 ? false : true;

        _delay = Random.Range(5, 60);

        print(_isRaining + "/" + _delay);

        yield return new WaitForSeconds(_delay);
    }
}

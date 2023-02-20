using System.Collections;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _rain, _snow;

    [SerializeField] [Space]
    private bool _isRaining, _isSnowing;

    private float _delay;

    public bool IsRaining => _rain.isPlaying;

    public bool IsSnowing => _snow.isPlaying;

    public event System.Action<bool, bool> onWeatherActivity;



    private void OnEnable()
    {
        GameSceneObjectsReferences.GameManager.OnGameStarted += delegate { StartCoroutine(StartCoroutines()); };
    }

    private void OnDisable()
    {
        GameSceneObjectsReferences.GameManager.OnGameStarted -= delegate { StartCoroutine(StartCoroutines()); };
    }

    private IEnumerator StartCoroutines()
    {
        yield return null;

        while (!GameSceneObjectsReferences.GameManager.IsGameEnded)
        {
            yield return StartCoroutine(ControlParticleSystems());
            yield return StartCoroutine(ControlIteration());
        }
    }

    private IEnumerator ControlParticleSystems()
    {
        Conditions<bool>.Compare(_isRaining, () => { _rain.Play(); }, () => { _rain.Stop(); });
        Conditions<bool>.Compare(_isSnowing, () => { _snow.Play(); }, () => { _snow.Stop(); });

        RaiseWeatherActivity();

        yield return null;
    }

    private IEnumerator ControlIteration()
    {
        _isRaining = !_isSnowing && Random.Range(0, 5) < 2 ? true : !_isSnowing && Random.Range(0, 5) >= 2 ? false : false;
        _isSnowing = !_isRaining && Random.Range(0, 5) < 2 ? true : !_isRaining && Random.Range(0, 5) >= 2 ? false : false;

        _delay = Random.Range(5, 60);

        print(_isRaining + "/" + _isSnowing + ": " + _delay);

        yield return new WaitForSeconds(_delay);
    }

    private void RaiseWeatherActivity()
    {
        onWeatherActivity?.Invoke(_isRaining, _isSnowing);
    }
}

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
        Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode, MyPhotonNetwork.IsMasterClient(MyPhotonNetwork.LocalPlayer), RaiseWeatherActivity, PhotonNetworkRaiseWeatherActivity, null, null);

        yield return null;
    }

    private IEnumerator ControlIteration()
    {
        if (!MyPhotonNetwork.IsOfflineMode && !MyPhotonNetwork.IsMasterClient(MyPhotonNetwork.LocalPlayer))
            yield break;

        Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode, MyPhotonNetwork.IsMasterClient(MyPhotonNetwork.LocalPlayer), ChangeWeather, PhotonNetworkChangeWeather, null, null);

        yield return new WaitForSeconds(_delay);
    }

    public void RaiseWeatherActivity()
    {
        Conditions<bool>.Compare(_isRaining, () => { _rain.Play(); }, () => { _rain.Stop(); });
        Conditions<bool>.Compare(_isSnowing, () => { _snow.Play(); }, () => { _snow.Stop(); });

        onWeatherActivity?.Invoke(_isRaining, _isSnowing);
    }

    public void ChangeWeather()
    {
        _isRaining = !_isSnowing && Random.Range(0, 5) < 2 ? true : false;
        _isSnowing = !_isRaining && Random.Range(0, 5) > 2 ? true : false;

        _delay = Random.Range(5, 60);
    }

    private void PhotonNetworkRaiseWeatherActivity() => GameSceneObjectsReferences.PhotonNetworkWeatherManager.RaiseWeatherActivity();
    private void PhotonNetworkChangeWeather() => GameSceneObjectsReferences.PhotonNetworkWeatherManager.ChangeWeather();
}

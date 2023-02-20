using System.Collections;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _rain, _snow;

    private float _delay;

    public bool IsRaining { get; set; }
    public bool IsSnowing { get; set; }

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
        Conditions<bool>.Compare(IsRaining, () => { _rain.Play(); }, () => { _rain.Stop(); });
        Conditions<bool>.Compare(IsSnowing, () => { _snow.Play(); }, () => { _snow.Stop(); });

        onWeatherActivity?.Invoke(IsRaining, IsSnowing);
    }

    public void ChangeWeather()
    {
        IsRaining = !IsSnowing && Random.Range(0, 5) < 2 ? true : false;
        IsSnowing = !IsRaining && Random.Range(0, 5) > 2 ? true : false;

        _delay = Random.Range(5, 100);
    }

    private void PhotonNetworkRaiseWeatherActivity() => GameSceneObjectsReferences.PhotonNetworkWeatherManager.RaiseWeatherActivity();
    private void PhotonNetworkChangeWeather() => GameSceneObjectsReferences.PhotonNetworkWeatherManager.ChangeWeather();
}

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
            yield return StartCoroutine(ChangeWeatherCoroutine());
            yield return StartCoroutine(RaiseWeatherActivityCoroutine());
        }
    }

    private IEnumerator ChangeWeatherCoroutine()
    {
        if (!MyPhotonNetwork.IsOfflineMode && !MyPhotonNetwork.IsMasterClient(MyPhotonNetwork.LocalPlayer))
            yield break;

        else if (MyPhotonNetwork.IsOfflineMode)
            ChangeWeather();

        else if(MyPhotonNetwork.IsMasterClient(MyPhotonNetwork.LocalPlayer))
            PhotonNetworkChangeWeather();

        yield return null;
    }

    private IEnumerator RaiseWeatherActivityCoroutine()
    {
        if (!MyPhotonNetwork.IsOfflineMode && !MyPhotonNetwork.IsMasterClient(MyPhotonNetwork.LocalPlayer))
            yield break;

        else if (MyPhotonNetwork.IsOfflineMode)
            RaiseWeatherActivity();

        else if (MyPhotonNetwork.IsMasterClient(MyPhotonNetwork.LocalPlayer))
            PhotonNetworkRaiseWeatherActivity();

        yield return new WaitForSeconds(_delay);
    }

    public void ChangeWeather()
    {
        IsRaining = !IsSnowing && Random.Range(0, 5) < 2 ? true : false;
        IsSnowing = !IsRaining && Random.Range(0, 5) > 2 ? true : false;

        Conditions<bool>.Compare(IsRaining, () => { _rain.Play(); }, () => { _rain.Stop(); });
        Conditions<bool>.Compare(IsSnowing, () => { _snow.Play(); }, () => { _snow.Stop(); });
    }

    public void RaiseWeatherActivity()
    {
        onWeatherActivity?.Invoke(IsRaining, IsSnowing);

        _delay = Random.Range(5, 100);
    }

    private void PhotonNetworkChangeWeather() => GameSceneObjectsReferences.PhotonNetworkWeatherManager.ChangeWeather();

    private void PhotonNetworkRaiseWeatherActivity() => GameSceneObjectsReferences.PhotonNetworkWeatherManager.RaiseWeatherActivity();
}

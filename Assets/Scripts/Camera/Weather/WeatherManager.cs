using System.Collections;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _rain, _snow;

    private ParticleSystem.ShapeModule _rainShape, _snowShape;

    private float _delay;

    public bool IsRaining { get; set; }
    public bool IsSnowing { get; set; }

    public event System.Action<bool, bool> onWeatherActivity;



    private void Awake()
    {
        _rainShape = _rain.shape;
        _snowShape = _snow.shape;
    }

    private void OnEnable()
    {
        GameSceneObjectsReferences.GameManager.OnGameStarted += delegate { StartCoroutine(StartCoroutines()); };
        GameSceneObjectsReferences.WindSystemController.onWindForce += OnWindForce;
    }

    private void OnDisable()
    {
        GameSceneObjectsReferences.GameManager.OnGameStarted -= delegate { StartCoroutine(StartCoroutines()); };
        GameSceneObjectsReferences.WindSystemController.onWindForce += OnWindForce;
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

    private void OnWindForce(int force)
    {
        _rainShape.rotation = new Vector3(180, -(force * 10), 0);
        _snowShape.rotation = new Vector3(180, -(force * 10), 0);
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

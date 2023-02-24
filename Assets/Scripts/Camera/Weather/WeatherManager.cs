using System.Collections;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _rain, _snow;

    private ParticleSystem.ShapeModule _rainShape, _snowShape;

    private System.Action<bool, bool> SetCurrentWeatherActivityFunction;
    private System.Action<bool, bool> RaiseWeatherActivityFunction;

    public bool IsRaining { get; private set; }
    public bool IsSnowing { get; private set; }

    public event System.Action<bool, bool> onWeatherActivity;



    private void Awake()
    {
        _rainShape = _rain.shape;
        _snowShape = _snow.shape;
    }

    private void Start()
    {
        DefineChangeWeatherFunction();
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

    private void DefineChangeWeatherFunction()
    {
        SetCurrentWeatherActivityFunction = MyPhotonNetwork.IsOfflineMode ? SetCurrentWeatherActivity :
                                MyPhotonNetwork.IsMasterClient(MyPhotonNetwork.LocalPlayer) ? SetCurrentWeatherActivityRPC :
                                null;

        RaiseWeatherActivityFunction = MyPhotonNetwork.IsOfflineMode ? RaiseWeatherActivity :
                                       MyPhotonNetwork.IsMasterClient(MyPhotonNetwork.LocalPlayer) ? RaiseWeatherActivityRPC :
                                       null;
    }

    private void OnWindForce(int force)
    {
        _rainShape.rotation = new Vector3(180, -(force * 10), 0);
        _snowShape.rotation = new Vector3(180, -(force * 10), 0);
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

        IsRaining = !IsSnowing && Random.Range(0, 5) < 2 ? true : false;
        IsSnowing = !IsRaining && Random.Range(0, 5) > 2 ? true : false;

        SetCurrentWeatherActivityFunction?.Invoke(IsRaining, IsSnowing);

        yield return null;
    }

    private IEnumerator RaiseWeatherActivityCoroutine()
    {
        if (!MyPhotonNetwork.IsOfflineMode && !MyPhotonNetwork.IsMasterClient(MyPhotonNetwork.LocalPlayer))
            yield break;

        RaiseWeatherActivityFunction?.Invoke(IsRaining, IsSnowing);

        yield return new WaitForSeconds(Random.Range(5, 100));
    }

    public void SetCurrentWeatherActivity(bool isRaining, bool isSnowing)
    {
        IsRaining = isRaining;
        IsSnowing = isSnowing;

        Conditions<bool>.Compare(IsRaining, () => { _rain.Play(); }, () => { _rain.Stop(); });
        Conditions<bool>.Compare(IsSnowing, () => { _snow.Play(); }, () => { _snow.Stop(); });
    }

    private void SetCurrentWeatherActivityRPC(bool isRaining, bool isSnowing)
    {
        GameSceneObjectsReferences.PhotonNetworkWeatherManager.SetCurrentWeatherActivity(IsRaining, IsSnowing);
    }

    public void RaiseWeatherActivity(bool isRaining, bool isSnowing)
    {
        onWeatherActivity?.Invoke(isRaining, isSnowing);
    }

    private void RaiseWeatherActivityRPC(bool isRaining, bool isSnowing)
    {
        GameSceneObjectsReferences.PhotonNetworkWeatherManager.RaiseWeatherActivity(isRaining, isSnowing);
    }
}

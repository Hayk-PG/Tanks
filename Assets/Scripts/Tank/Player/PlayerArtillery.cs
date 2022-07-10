using System.Collections;
using UnityEngine;

public class PlayerArtillery : MonoBehaviour
{
    private TankController _tankController;
    private IScore _iScore;
    private Tab_RemoteControl _tabRemoteControl;
    [SerializeField] private ArtilleryBulletController _artilleryPrefab;


    private void Awake()
    {
        _tankController = Get<TankController>.From(gameObject);
        _iScore = Get<IScore>.From(gameObject);
        _tabRemoteControl = FindObjectOfType<Tab_RemoteControl>();
    }

    private void OnEnable()
    {
        _tankController.OnInitialize += OnInitialize;
    }

    private void OnDisable()
    {
        _tankController.OnInitialize -= OnInitialize;
        _tabRemoteControl.OnGiveCoordinates -= OnGiveCoordinates;
    }

    private void OnInitialize()
    {
        _tabRemoteControl.OnGiveCoordinates += OnGiveCoordinates;
    }

    private void OnGiveCoordinates(Vector3 coordinate)
    {
        StartCoroutine(InstantiateArtilleryShells(coordinate));
    }

    private IEnumerator InstantiateArtilleryShells(Vector3 coordinate)
    {
        for (int i = 0; i < 5; i++)
        {
            float randomX = i <= 3 ? Random.Range(-2, 2) : 0;
            ArtilleryBulletController artillery = Instantiate(_artilleryPrefab, new Vector3(coordinate.x + randomX, coordinate.y + 10, coordinate.z), Quaternion.identity);
            artillery.OwnerScore = _iScore;
            artillery.IsLastShellOfBarrage = i < 4 ? false : true;
            yield return new WaitForSeconds(0.5f);
        }
    }
}

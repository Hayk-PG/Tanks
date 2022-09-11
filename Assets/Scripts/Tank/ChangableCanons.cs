using UnityEngine;

public class ChangableCanons : MonoBehaviour
{
    private Transform _canonPivotPoint;
    private MeshFilter[] _canons;
    private Transform _shootPoint;
    private float _shootPointDistance;
    private PlayerAmmoType _playerAmmoType;
    private ShootController _shootController;



    private void Awake()
    {
        _playerAmmoType = Get<PlayerAmmoType>.From(gameObject);
        _shootController = Get<ShootController>.From(gameObject);
    }

    private void Start()
    {       
        _canonPivotPoint = _shootController.CanonPivotPoint;
        _canons = _canonPivotPoint.GetComponentsInChildren<MeshFilter>(true);
        _shootPoint = _canonPivotPoint.Find("ShootPoint").transform;
        _shootPointDistance = _shootPoint.transform.localPosition.z - _canons[0].mesh.bounds.size.z;
    }

    private void OnEnable()
    {
        _playerAmmoType.OnWeaponChanged += OnWeaponChanged;
    }

    private void OnDisable()
    {
        _playerAmmoType.OnWeaponChanged -= OnWeaponChanged;
    }

    private void OnWeaponChanged(int weapon)
    {
        CanonsActivity(weapon);
    }

    private void CanonsActivity(int index)
    {
        if (_canons != null)
        {
            GlobalFunctions.Loop<MeshFilter>.Foreach(_canons, canon => { canon.gameObject.SetActive(false); });
            _canons[index].gameObject.SetActive(true);
            _shootPoint.localPosition = new Vector3(0, 0, _canons[index].mesh.bounds.size.z * _canons[index].transform.localScale.x + _shootPointDistance);
        }
    }
}

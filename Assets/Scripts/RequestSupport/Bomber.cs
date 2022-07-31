using UnityEngine;

public class Bomber : MonoBehaviour
{
    [SerializeField] private Transform _propeller, _bombSpwnPoint;
    [SerializeField] private BombController _bombPrefab;

    private GameManagerBulletSerializer _gameManagerBulletSerializer;
    private MainCameraController _mainCameraController;

    public float MinX { get; set; }
    public float MaxX { get; set; }
    private bool isOutOfBoundaries
    {
        get => transform.position.x < MinX || transform.position.x > MaxX;
    }


    private void Awake()
    {
        _gameManagerBulletSerializer = FindObjectOfType<GameManagerBulletSerializer>();
        _mainCameraController = FindObjectOfType<MainCameraController>();
    }

    private void Update()
    {
        Conditions<bool>.Compare(isOutOfBoundaries, Deactivate, null);
    }

    private void FixedUpdate()
    {
        transform.Translate(-transform.forward * 2 * Time.fixedDeltaTime);
        _propeller.Rotate(Vector3.right, -1000 * Time.deltaTime);
    }

    public void DropBomb(IScore _iScore)
    {
        BombController _bomb = Instantiate(_bombPrefab, _bombSpwnPoint.position, Quaternion.identity);
        _gameManagerBulletSerializer.BulletController = _bomb;
        _bomb.OwnerScore = _iScore;
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
        _mainCameraController.ResetTargets();
    }
}

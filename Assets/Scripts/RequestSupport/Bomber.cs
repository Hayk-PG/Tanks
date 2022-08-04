using UnityEngine;

public class Bomber : MonoBehaviour
{
    [SerializeField] private Transform _propeller, _bombSpwnPoint;
    [SerializeField] private BombController _bombPrefab;

    private BombController _bomb;
    private GameManagerBulletSerializer _gameManagerBulletSerializer;
    private MainCameraController _mainCameraController;

    public IScore OwnerScore { get; set; }
    public PlayerTurn OwnerTurn { get; set; }
    public Vector3 DropPoint { get; set; }
    public float MinX { get; set; }
    public float MaxX { get; set; }
    private bool isOutOfBoundaries
    {
        get => transform.position.x < MinX || transform.position.x > MaxX;
    }
    private bool IsBombDropped { get; set; }


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

        if(transform.position.x >= DropPoint.x - 0.1f && transform.position.x <= DropPoint.x + 0.1f)
        {
            DropBomb();
        }
    }

    public void DropBomb()
    {
        if (!IsBombDropped)
        {
            _bomb = Instantiate(_bombPrefab, _bombSpwnPoint.position, Quaternion.identity);
            _gameManagerBulletSerializer.BulletController = _bomb;
            _bomb.OwnerScore = OwnerScore;
            IsBombDropped = true;
        }
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
        IsBombDropped = false;
        _mainCameraController.ResetTargets();
    }
}

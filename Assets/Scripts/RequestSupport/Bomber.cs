using UnityEngine;

public enum BomberType { Light, Medium, Heavy, Nuke }


public class Bomber : MonoBehaviour
{
    [SerializeField] [Space]
    private Transform _bombSpwnPoint;

    [SerializeField] [Space]
    private Rigidbody _rigidbody;

    [SerializeField] [Space]
    private BombController _bombPrefab;

    [SerializeField] [Space]
    private BomberAddressable _bomberAddressable;

    public IScore OwnerScore { get; set; }

    public PlayerTurn OwnerTurn { get; set; }

    public Vector3 DropPoint { get; set; }

    public float MinX { get; set; }
    public float MaxX { get; set; }

    private bool IsOutOfBoundaries
    {
        get => _rigidbody.position.x < MinX || _rigidbody.position.x > MaxX;
    }
    private bool IsBombDropped { get; set; }





    private void OnEnable()
    {
        _bomberAddressable.LoadMeshes();
    }

    private void FixedUpdate()
    {
        DropBomb();

        Conditions<bool>.Compare(IsOutOfBoundaries, Deactivate, null);
    }

    public void DropBomb()
    {
        if (!IsBombDropped)
        {
            if (_rigidbody.position.x >= DropPoint.x - 0.1f && _rigidbody.position.x <= DropPoint.x + 0.1f)
            {
                BaseBulletController bomb = Instantiate(_bombPrefab, _bombSpwnPoint.position, Quaternion.identity);

                GameSceneObjectsReferences.GameManagerBulletSerializer.BaseBulletController = bomb;

                bomb.OwnerScore = OwnerScore;

                IsBombDropped = true;
            }
        }
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);

        IsBombDropped = false;

        GameSceneObjectsReferences.MainCameraController.ResetTargets();
    }
}

using UnityEngine;

public enum BomberType { Light, Medium, Heavy, Nuke }


public class Bomber : MonoBehaviour
{
    [SerializeField] [Space]
    private Transform _propeller, _bombSpwnPoint;

    [SerializeField] [Space]
    private Rigidbody _rigidbody;

    [SerializeField] [Space]
    private BombController _bombPrefab;

    public IScore OwnerScore { get; set; }
    public PlayerTurn OwnerTurn { get; set; }
    public Vector3 DropPoint { get; set; }
    public float MinX { get; set; }
    public float MaxX { get; set; }
    private bool isOutOfBoundaries
    {
        get => _rigidbody.position.x < MinX || _rigidbody.position.x > MaxX;
    }
    private bool IsBombDropped { get; set; }



    private void FixedUpdate()
    {
        Movement();

        DropBomb();

        Conditions<bool>.Compare(isOutOfBoundaries, Deactivate, null);
    }

    private void Movement()
    {
        _propeller.Rotate(Vector3.right, -1000 * Time.deltaTime);
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

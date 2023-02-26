using UnityEngine;

public enum BomberType { Light, Medium, Heavy, Nuke }


public class Bomber : MonoBehaviour
{
    [SerializeField] 
    private Transform _propeller, _bombSpwnPoint;

    [SerializeField] [Space]
    private BombController _bombPrefab;


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



    private void Update()
    {
        Movement();

        if (transform.position.x >= DropPoint.x - 0.1f && transform.position.x <= DropPoint.x + 0.1f)
        {
            DropBomb();
        }

        Conditions<bool>.Compare(isOutOfBoundaries, Deactivate, null);
    }

    private void Movement()
    {
        transform.Translate(-transform.forward * 2 * Time.deltaTime);

        _propeller.Rotate(Vector3.right, -1000 * Time.deltaTime);
    }

    public void DropBomb()
    {
        if (!IsBombDropped)
        {
            BaseBulletController bomb = Instantiate(_bombPrefab, _bombSpwnPoint.position, Quaternion.identity);

            GameSceneObjectsReferences.GameManagerBulletSerializer.BaseBulletController = bomb;

            bomb.OwnerScore = OwnerScore;

            IsBombDropped = true;
        }
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);

        IsBombDropped = false;

        GameSceneObjectsReferences.MainCameraController.ResetTargets();
    }
}

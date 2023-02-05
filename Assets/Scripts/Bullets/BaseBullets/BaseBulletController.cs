using UnityEngine;

public class BaseBulletController : MonoBehaviour
{
    [SerializeField]
    protected Rigidbody _rigidbody;

    public Rigidbody RigidBody => _rigidbody;
    public IScore OwnerScore { get; set; }
    public Vector3 StartPosition { get; protected set; }
    public Vector3 ID { get; protected set; }
    public float Distance => Vector3.Distance(StartPosition, transform.position);
    public bool IsLastShellOfBarrage { get; set; }



    protected virtual void Awake()
    {
        GameSceneObjectsReferences.TurnController.SetNextTurn(TurnState.Other);
    }
}

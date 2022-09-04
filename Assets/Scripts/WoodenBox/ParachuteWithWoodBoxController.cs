using UnityEngine;

public class ParachuteWithWoodBoxController : MonoBehaviour
{    
    [SerializeField] private GameObject _parachute;
    [SerializeField] private GameObject _sparkles;
    [SerializeField] private GameObject _woodBoxExplosion;   
    private Animator _parachuteAnim;
    private WoodBox _woodBox;
    private TankController _tankController;
    private WoodenBoxSerializer _woodenBoxSerializer;
    private delegate float Value();
    private Value _gravity;
    private int _collisionCount = 0;

    public Rigidbody RigidBody;
    public int RandomContent { get; set; }



    private void Awake()
    {
        RigidBody = Get<Rigidbody>.From(gameObject);
        _parachuteAnim = Get<Animator>.From(gameObject);
        _woodBox = Get<WoodBox>.From(gameObject);
        _gravity = delegate { return 30 * Time.fixedDeltaTime; };
        _woodenBoxSerializer = FindObjectOfType<WoodenBoxSerializer>();
    }

    private void FixedUpdate()
    {
        Rigidbody();
    }

    private void OnCollisionEnter(Collision collision)
    {
        OnLand();

        bool isCollidedWithTank = Get<TankController>.From(collision.gameObject) != null;
        bool isCollidedWithBullet = Get<BulletController>.From(collision.gameObject) != null;

        if (_collisionCount < 1)
        {
            if (isCollidedWithTank)
                OnCollisionWithTank(collision);

            if (isCollidedWithBullet)
                OnCollisionWithBullet();
        }
    }

    private void OnCollisionWithTank(Collision collision)
    {
        _tankController = Get<TankController>.From(collision.gameObject);
        _woodBox.OnContent(RandomContent, _tankController);
        _collisionCount++;
        DestroyGameobject();
    }

    private void OnCollisionWithBullet()
    {
        _collisionCount++;
        DestroyGameobject();
    }

    private void Rigidbody()
    {
        RigidBody.velocity = new Vector3(RigidBody.velocity.x, RigidBody.velocity.y * _gravity(), 0);
        RigidBody.position = new Vector3(RigidBody.position.x, RigidBody.position.y, 0);
        RigidBody.rotation = Quaternion.Euler(0, 0, RigidBody.rotation.z);

        if (RigidBody.position.y <= -5)
            DestroyGameobject();
    }

    private void OnLand()
    {
        _parachuteAnim.Play("ParachuteCloseAnim", 0);
        _sparkles.SetActive(true);
    }

    private void DestroyGameobject()
    {
        if (MyPhotonNetwork.IsOfflineMode)
        {
            Explosion();
            Destroy(gameObject);
        }
        else
        {
            _woodenBoxSerializer.DestroyParachuteWithWoodBoxController();
        }       
    }

    public void Explosion()
    {
        _woodBoxExplosion.SetActive(true);
        _woodBoxExplosion.transform.SetParent(null);
    }

    public void OnAnimationEnd()
    {
        _parachute.SetActive(false);
        _gravity = delegate { return 1; };
    }
}

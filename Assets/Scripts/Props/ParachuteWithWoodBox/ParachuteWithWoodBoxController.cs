using UnityEngine;

public class ParachuteWithWoodBoxController : MonoBehaviour
{    
    [SerializeField] private GameObject _parachute;
    [SerializeField] private GameObject _sparkles;
    [SerializeField] private GameObject _woodBoxExplosion;
    private Rigidbody _rigidbody;
    private Animator _parachuteAnim;
    private WoodBox _woodBox;
    private TankController _tankController;
    private delegate float Value();
    private Value _gravity;
    private int _collisionCount = 0;

    public int RandomContent { get; set; }



    private void Awake()
    {
        _rigidbody = Get<Rigidbody>.From(gameObject);
        _parachuteAnim = Get<Animator>.From(gameObject);
        _woodBox = Get<WoodBox>.From(gameObject);
        _gravity = delegate { return 30 * Time.fixedDeltaTime; };
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
        _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, _rigidbody.velocity.y * _gravity(), 0);
        _rigidbody.position = new Vector3(_rigidbody.position.x, _rigidbody.position.y, 0);
        _rigidbody.rotation = Quaternion.Euler(0, 0, _rigidbody.rotation.z);

        if (_rigidbody.position.y <= -5)
            DestroyGameobject();
    }

    private void OnLand()
    {
        _parachuteAnim.Play("ParachuteCloseAnim", 0);
        _sparkles.SetActive(true);
    }

    private void DestroyGameobject()
    {
        _woodBoxExplosion.SetActive(true);
        _woodBoxExplosion.transform.SetParent(null);
        Destroy(gameObject);
    }

    public void OnAnimationEnd()
    {
        _parachute.SetActive(false);
        _gravity = delegate { return 1; };
    }
}

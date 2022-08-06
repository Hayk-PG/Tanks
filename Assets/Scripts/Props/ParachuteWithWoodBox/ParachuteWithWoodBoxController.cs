using UnityEngine;

public class ParachuteWithWoodBoxController : MonoBehaviour
{    
    [SerializeField] private GameObject _parachute;
    [SerializeField] private GameObject _woodBoxExplosion;
    private Rigidbody _rigidbody;
    private Animator _parachuteAnim;
    private WoodBox _woodBox;
    private TankController _tankController;
    private delegate float Value();
    private Value _gravity;



    private void Awake()
    {
        _rigidbody = Get<Rigidbody>.From(gameObject);
        _parachuteAnim = Get<Animator>.From(gameObject);
        _woodBox = Get<WoodBox>.From(gameObject);
        _gravity = delegate { return 20 * Time.deltaTime; };
    }

    private void FixedUpdate()
    {
        Rigidbody();
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlayParachuteAnim();

        if (Get<TankController>.From(collision.gameObject) != null)
        {
            _tankController = Get<TankController>.From(collision.gameObject);
            _woodBox.OnContent(0, _tankController);
            DestroyGameobject();
        }
    }

    private void Rigidbody()
    {
        _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, _rigidbody.velocity.y * _gravity(), 0);
        _rigidbody.position = new Vector3(_rigidbody.position.x, _rigidbody.position.y, 0);
        _rigidbody.transform.eulerAngles = new Vector3(0, _rigidbody.transform.eulerAngles.y, _rigidbody.transform.eulerAngles.z);
    }

    private void PlayParachuteAnim()
    {
        _parachuteAnim.Play("ParachuteCloseAnim", 0);
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

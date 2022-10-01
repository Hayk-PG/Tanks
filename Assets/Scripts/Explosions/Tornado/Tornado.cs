using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : MonoBehaviour
{
    [SerializeField] 
    private float _boxHeight, _force;
    private int _lifeTime;
    private string _iScoreName;  
    private List<string> _colliderNames = new List<string>();

    private GameObject _colliderGameObject;
    [SerializeField] [Header("Fake explosion")]
    private GameObject _fakeExplosion;

    private ParticleSystem _particles;
    private Collider[] _colliders;
    private Explosion _explosion;
    private ExternalSoundSource _externalSoundSource;
    private Rigidbody _rigidBody;
    private GameObject _ownerGameObject;
    private PlayerTurn _playerTurn;
    private TurnController _turnController;
    private GameManagerBulletSerializer _gameManagerBulletSerializer;



    private void Awake()
    {
        _particles = Get<ParticleSystem>.FromChild(gameObject);
        _explosion = Get<Explosion>.From(gameObject);
        _externalSoundSource = Get<ExternalSoundSource>.FromChild(gameObject);
        _turnController = FindObjectOfType<TurnController>();
        _gameManagerBulletSerializer = FindObjectOfType<GameManagerBulletSerializer>();
    }

    private void Start()
    {       
        StartCoroutine(Damage());
    }

    private void FixedUpdate()
    {
        _colliders = Physics.OverlapBox(transform.position + new Vector3(0, _boxHeight, 0), new Vector3(3, _boxHeight, 3), Quaternion.identity);

        GlobalFunctions.Loop<Collider>.Foreach(_colliders, collider => 
        {
            if (Get<GlobalRigidbody>.From(collider.gameObject) != null)
            {
                _rigidBody = Get<GlobalRigidbody>.From(collider.gameObject).RigidBody;
                _rigidBody.position = Vector3.Lerp(_rigidBody.position, transform.position, _force * Time.fixedDeltaTime);
            }
        });
    }

    public void DestroyTornado()
    {
        _turnController.SetNextTurn(_playerTurn.MyTurn == TurnState.Player1 ? TurnState.Player2 : TurnState.Player2);
        _particles.Stop();
        _particles.transform.SetParent(null);
        _externalSoundSource?.Stop(true);
        Destroy(transform.parent.gameObject);
    }

    private IEnumerator Damage()
    {
        _ownerGameObject = GlobalFunctions.ObjectsOfType<ScoreController>.Find(score => Get<IScore>.From(score.gameObject) == _explosion.OwnerScore).gameObject;
        _iScoreName = _ownerGameObject.name;
        _playerTurn = Get<PlayerTurn>.From(_ownerGameObject);

        while (true)
        {
            _lifeTime++;
            _colliderNames.Clear();

            GlobalFunctions.Loop<Collider>.Foreach(_colliders, collider =>
            {                
                if (Get<IDamage>.From(collider.gameObject) != null)
                {
                    _colliderGameObject = Get<HealthController>.From(collider.gameObject).gameObject;

                    if (!_colliderNames.Contains(_colliderGameObject.name) && Vector3.Distance(transform.position, _colliderGameObject.transform.position) <= _explosion.RadiusValue)
                    {
                        _fakeExplosion.SetActive(true);
                        _gameManagerBulletSerializer.TornadoDamage(_colliderGameObject.name, Mathf.RoundToInt(_explosion.DamageValue), _iScoreName);
                        _colliderNames.Add(_colliderGameObject.name);
                    }
                }
            });

            if (_lifeTime >= 6)
            {
                Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode, DestroyTornado, () => _gameManagerBulletSerializer.DestroyTornado(name));
            }

            yield return new WaitForSeconds(1.5f);
        }
    }
}

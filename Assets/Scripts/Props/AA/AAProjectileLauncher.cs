using System.Collections.Generic;
using UnityEngine;

public class AAProjectileLauncher : MonoBehaviour
{
    private Animator _animator;
    private TurnController _turnController;
    private PhotonNetworkAALauncher _photonNetworkAALauncher;
    private IScore _ownerScore;
    private BulletController _enemyBullet;

    [SerializeField] private AAProjectileArsenal[] _aaProjectileArsenals;
    [SerializeField] private ParticleSystem _gameobjectSpawnEffect;
    [SerializeField] private List<ParticleSystem> _gameobjectDespawnEffects;
    [SerializeField] private float _force;
    private int _index = 0;
    private bool _launched;
    private string _animationLaunch = "Launch";

    public Vector3 ID { get; private set; }



    private void Awake()
    {
        _animator = Get<Animator>.From(gameObject);
        _turnController = FindObjectOfType<TurnController>();
        _photonNetworkAALauncher = FindObjectOfType<PhotonNetworkAALauncher>();
    }

    private void OnEnable()
    {
        PlayGameobjectSpawnEffect();
        _turnController.OnTurnChanged += OnTurnChanged;
    }

    private void OnDisable()
    {
        _turnController.OnTurnChanged -= OnTurnChanged;
    }

    private void FixedUpdate() => LaunchMissile();

    private void PlayGameobjectSpawnEffect() => _gameobjectSpawnEffect.Play(true);

    private void PlayGameobjectDespawnEffect()
    {
        if(_gameobjectDespawnEffects.Count > 0)
        {
            _gameobjectDespawnEffects[0].Play(true);           
            _gameobjectDespawnEffects[0].transform.SetParent(null);
            _gameobjectDespawnEffects.RemoveAt(0);
        }
    }

    public void Init(TankController ownerTankController)
    {
        if (ownerTankController == null)
            return;

        ID = transform.position;
        _index = 0;
        _ownerScore = Get<IScore>.From(ownerTankController.gameObject);
        print(_ownerScore);
    }

    private void LaunchMissile()
    {
        if (IsTargetDetected())
        {
            if (!_launched)
            {
                if (_index < _aaProjectileArsenals.Length)
                {
                    Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode, InitMissile, () => InitMissile(_photonNetworkAALauncher));
                    _launched = true;
                }
                else
                {
                    Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode, Deactivate, () => Deactivate(_photonNetworkAALauncher));
                }
            }
        }
    }

    public void InitMissile()
    {
        if(_aaProjectileArsenals[_index].Missiles.Count > 0)
        {
            _aaProjectileArsenals[_index].Missiles[0].gameObject.SetActive(true);
            _aaProjectileArsenals[_index].Missiles[0].OwnerScore = _ownerScore;
            _aaProjectileArsenals[_index].Missiles[0].RigidBody.velocity = _aaProjectileArsenals[_index].transform.TransformDirection(Vector3.forward) * _force;
            _aaProjectileArsenals[_index].Missiles.RemoveAt(0);
            _animator.Play(_animationLaunch, 0, 0);
            _index++;
        }
    }

    private void InitMissile(PhotonNetworkAALauncher photonNetworkAALauncher)
    {
        photonNetworkAALauncher.InitMissile(ID);
    }

    public void Deactivate()
    {
        PlayGameobjectDespawnEffect();
        gameObject.SetActive(false);
    }

    private void Deactivate(PhotonNetworkAALauncher photonNetworkAALauncher)
    {
        photonNetworkAALauncher.Deactivate(ID);
    }

    private void OnTurnChanged(TurnState turnState)
    {
        if(turnState == TurnState.Other)
        {
            _enemyBullet = GlobalFunctions.ObjectsOfType<BulletController>.Find(bullet => bullet.OwnerScore != _ownerScore);
            _launched = false;
        }
    }

    private bool IsTargetDetected()
    {
        return _enemyBullet != null && Vector3.Distance(_enemyBullet.transform.position, transform.position) <= 10 ? true : false;
    }
}

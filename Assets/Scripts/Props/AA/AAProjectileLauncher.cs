using System.Collections;
using UnityEngine;

public class AAProjectileLauncher : MonoBehaviour
{
    [SerializeField] private BulletController[] _missiles;

    private TurnController _turnController;
    private TankController _ownerTankController;
    private ScoreController _ownerScoreController;
    private PlayerTurn _ownerPlayerTurn;
    
    [SerializeField] private float _force;
    private int index = 0;
    private bool _launched;
    private bool _isLauncherReady;
    private bool _isTargetDetected;
    
    public Vector3 ID { get; private set; }


    private void Awake() => _turnController = FindObjectOfType<TurnController>();

    private void Start() => StartCoroutine(KeepLauncherReady());

    private void OnEnable() => _turnController.OnTurnChanged += OnTurnChanged;

    private void OnDisable() => _turnController.OnTurnChanged -= OnTurnChanged;

    private void FixedUpdate() => LaunchMissile();

    public void Init(TankController ownerTankController)
    {
        ID = transform.position;
        _ownerTankController = ownerTankController;

        if (_ownerTankController == null)
            return;

        _ownerScoreController = Get<ScoreController>.From(_ownerTankController.gameObject);
        _ownerPlayerTurn = Get<PlayerTurn>.From(_ownerTankController.gameObject);
    }

    private void OnTurnChanged(TurnState turnState)
    {
        if (turnState == TurnState.Player1 || turnState == TurnState.Player2)
        {
            _isLauncherReady = turnState != _ownerPlayerTurn?.MyTurn;
            _launched = false;
        }
    }

    private void LaunchMissile()
    {
        if (_isLauncherReady && _isTargetDetected && !_launched)
        {
            if (index < _missiles.Length)
            {
                _missiles[index].gameObject.SetActive(true);
                _missiles[index].OwnerScore = _ownerScoreController;
                _missiles[index].RigidBody.velocity = _missiles[index].transform.TransformDirection(Vector3.forward) * _force;
                index++;
            }

            _launched = true;
        }
    }

    private IEnumerator KeepLauncherReady()
    {
        while (true)
        {
            _isTargetDetected = FindObjectOfType<BulletController>();
            yield return new WaitForSeconds(1);
        }
    }
}

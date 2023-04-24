using System.Collections;
using UnityEngine;

public class SmokeCloud : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem[] _particleSystems;

    [SerializeField] [Space]
    private int _turns;
    private int _turnsCount;

    private bool _isParticlesStopped;

    private TurnState? _startingTurn;

    private PlayerSmokeScreenDetector _playerSmokeScreenDetector;

    public Vector3 Id { get; private set; }





    private void Awake() => Id = transform.position;

    private void OnEnable() => GameSceneObjectsReferences.TurnController.OnTurnChanged += OnTurnChanged;

    private void OnTriggerEnter(Collider other)
    {
        if (_playerSmokeScreenDetector != null)
            return;

        DetectSmokeScreenImpact(true, false, Get<PlayerSmokeScreenDetector>.From(other.gameObject));
    }

    private void OnTriggerExit(Collider other)
    {
        bool isValidExitCollision = _playerSmokeScreenDetector != null && _playerSmokeScreenDetector == Get<PlayerSmokeScreenDetector>.From(other.gameObject);

        if (isValidExitCollision)
            DetectSmokeScreenImpact(false, true);
    }

    private void OnTurnChanged(TurnState turnState)
    {
        if (!_startingTurn.HasValue)
        {
            bool isPlayerTurn = turnState == TurnState.Player1 || turnState == TurnState.Player2;

            if (isPlayerTurn)
            {
                _startingTurn = turnState;

                _turnsCount++;
            }

            return;
        }

        bool isTurnMatched = _startingTurn.Value == turnState;

        if (isTurnMatched)
        {
            _turnsCount++;

            if (_turnsCount < _turns)
                return;

            if (!_isParticlesStopped)
                StartCoroutine(StopParticlesAndDestroy());
        }
    }

    private IEnumerator StopParticlesAndDestroy()
    {
        GlobalFunctions.Loop<ParticleSystem>.Foreach(_particleSystems, particle => particle.Stop());

        _isParticlesStopped = true;

        DetectSmokeScreenImpact(false);

        yield return new WaitUntil(() => _particleSystems[0].isStopped);

        SelfDestruct();
    }

    private void DetectSmokeScreenImpact(bool isImpacted = false, bool release = false, PlayerSmokeScreenDetector playerSmokeScreenDetector = null)
    {
        if (playerSmokeScreenDetector != null)
            _playerSmokeScreenDetector = playerSmokeScreenDetector;

        _playerSmokeScreenDetector?.DetectPlayerSmokeScreenImpact(isImpacted);

        if (release)
            _playerSmokeScreenDetector = null;
    }

    private void SelfDestruct()
    {
        // Called for the local player only
        DestroySmokeCloud();

        // Called for all other players
        // And should only be called on the server side
        GameSceneObjectsReferences.PhotonNetworkSmokeScreenDetector.DestroySmokeCloud(Id);
    }

    public void DestroySmokeCloud() => Destroy(gameObject);
}

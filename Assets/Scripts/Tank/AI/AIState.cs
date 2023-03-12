using System.Collections;
using UnityEngine;

public class AIState : MonoBehaviour
{
    [SerializeField]
    private PlayerTurn _playerTurn;

    [SerializeField] [Space]
    private HealthController _healthController;

    [SerializeField] [Space]
    private AIShootController _aiShootController;

    [SerializeField] [Space]
    private AICanonRaycast _aiCanonRayCast;

    public enum MovementStates { MoveForward, MoveBackward, DoNotMove }
    public MovementStates AiMovementStates { get; private set; }

    public int MissedShotsCount { get; private set; }
    public int SuccessfulShotsCount { get; private set; }
    public int ReceivedHitCounts { get; private set; }

    public float Distance { get; private set; }
    public float ShotDistance { get; private set; }
    public float RaycastHitDistance { get; private set; }
    public float WoodBoxDistance { get; private set; }

    public bool IsHit { get; private set; }
    public bool IsRaycastHit { get; private set; }
    public bool IsLastShotMissed { get; private set; }

    public Vector3 PreviousPosition { get; private set; }
    public Vector3 CurrentPosition { get; private set; }
    public Vector3 NextPosition { get; private set; }
    public Vector3 EnemyCurrentPosition { get; private set; }
    public Vector3 EnemyPreviousPosition { get; private set; }
    public Vector3 HitPosition { get; private set; }
    public Vector3 EnemyHitPosition { get; private set; }
    public Vector3 MissileLandingPosition { get; private set; }
    public Vector3 MissilePosition { get; private set; }

    public GameObject MissileObj { get; private set; }
    public GameObject EnemyTankObj { get; private set; }

    public HealthController HealthController { get; private set; }

    public event System.Action<int, int> onMove;




    private void OnEnable()
    {
        GameSceneObjectsReferences.GameManager.OnGameStarted += OnGameStart;

        GameSceneObjectsReferences.TurnController.OnTurnChanged += OnTurnController;

        _aiShootController.onAiShoot += OnShoot;

        _healthController.OnTakeDamage += OnTakeDamage;

        _healthController.onUpdateArmorBar += (int damage) => { OnTakeDamage(null, damage); };

        _aiCanonRayCast.onRayCast += OnRayCast;
    }

    private void OnDisable()
    {
        GameSceneObjectsReferences.GameManager.OnGameStarted -= OnGameStart;

        GameSceneObjectsReferences.TurnController.OnTurnChanged -= OnTurnController;

        _aiShootController.onAiShoot -= OnShoot;

        _healthController.OnTakeDamage -= OnTakeDamage;

        _healthController.onUpdateArmorBar -= (int damage) => { OnTakeDamage(null, damage); };

        _aiCanonRayCast.onRayCast -= OnRayCast;
    }

    private void FixedUpdate()
    {
        GetMissilePosition();
    }

    private void OnGameStart()
    {
        EnemyTankObj = GlobalFunctions.ObjectsOfType<TankController>.Find(tankController => tankController.gameObject.name != Names.Tank_SecondPlayer).gameObject;

        HealthController = Get<HealthController>.From(EnemyTankObj);
    }

    private void OnTurnController(TurnState turnState)
    {
        GetTurnState(turnState);
    }

    private void OnShoot(GameObject gameObject)
    {
        MissileObj = gameObject;
    }

    private void OnTakeDamage(BasePlayer basePlayer, int damage)
    {
        if (damage > 0)
            IsHit = true;
    }

    private void OnRayCast(bool isRayCastHit, float distance)
    {
        IsRaycastHit = isRayCastHit;

        RaycastHitDistance = distance;
    }

    private void GetMissilePosition()
    {
        if (MissileObj != null)
        {
            MissilePosition = MissileObj.transform.position;
        }
    }

    private void GetTurnState(TurnState turnState)
    {
        if (turnState == _playerTurn.MyTurn)
            StartCoroutine(ExecuteOnAITurn());

        if (turnState == TurnState.Player1)
            StartCoroutine(ExecuteOnEnemyTurn());
    }

    private IEnumerator ExecuteOnAITurn()
    {
        yield return StartCoroutine(CalculateEnemyCurrentPosition());
        yield return StartCoroutine(CalculateEnemyHitPositions());
        yield return StartCoroutine(CalculateCurrentPosition());
        yield return StartCoroutine(CalculateWoodBoxDistance());
        yield return StartCoroutine(CalculateTanksDistance());
        yield return StartCoroutine(CalculateShootControllerTargetFixingValue());
        yield return StartCoroutine(CalculateNextPosition());
    }

    private IEnumerator ExecuteOnEnemyTurn()
    {
        IsHit = false;

        WoodBoxDistance = 0;

        yield return StartCoroutine(CalculateEnemyPreviousPosition());
    }

    private IEnumerator CalculateWoodBoxDistance()
    {
        yield return null;

        ParachuteWithWoodBoxController parachuteWithWoodBoxController = FindObjectOfType<ParachuteWithWoodBoxController>();

        if (parachuteWithWoodBoxController != null)
            WoodBoxDistance = (parachuteWithWoodBoxController.transform.position - CurrentPosition).x;
    }

    private IEnumerator CalculateEnemyPreviousPosition()
    {
        yield return null;

        EnemyPreviousPosition = EnemyTankObj.transform.position;

        //GlobalFunctions.DebugLog("EnemyPreviousPosition: " + EnemyPreviousPosition);
    }

    private IEnumerator CalculateEnemyCurrentPosition()
    {
        yield return null;

        EnemyCurrentPosition = EnemyTankObj.transform.position;

        //GlobalFunctions.DebugLog("EnemyCurrentPosition: " + EnemyCurrentPosition);
    }

    private IEnumerator CalculateTanksDistance()
    {
        yield return null;

        Distance = Vector3.Distance(CurrentPosition, EnemyCurrentPosition);

        //GlobalFunctions.DebugLog("Distance: " + Distance);
    }

    private IEnumerator CalculateEnemyHitPositions()
    {
        yield return null;

        if (MissilePosition != Vector3.zero)
        {
            MissileLandingPosition = MissilePosition;

            ShotDistance = (float)Converter.Double(Vector3.Distance(EnemyPreviousPosition, MissileLandingPosition));

            print(Vector3.Cross(EnemyCurrentPosition, MissileLandingPosition));

            IsLastShotMissed = ShotDistance <= 1 ? false : true;

            //GlobalFunctions.DebugLog($"Hit distance: {ShotDistance}");
        }
        else
        {
            IsLastShotMissed = true;
        }

        //print(EnemyPreviousPosition);
    }

    private IEnumerator CalculateShootControllerTargetFixingValue()
    {
        yield return null;

        _aiShootController.ControlTargetFixingValue(Distance > 6 ? 0.5f : 0);
    }

    private IEnumerator CalculateCurrentPosition()
    {
        yield return null;

        CurrentPosition = transform.position;

        //GlobalFunctions.DebugLog("CurrentPosition: " + CurrentPosition);
    }

    private IEnumerator CalculateNextPosition()
    {
        yield return null;

        int stepsLenth = Random.Range(0, 10);
        int direction = Random.Range(-1, 2) <= 0 ? -1 : 1;

        if (WoodBoxDistance != 0)
        {
            stepsLenth = Mathf.Abs(Mathf.RoundToInt(WoodBoxDistance));

            direction = WoodBoxDistance < 0 ? 1 : -1;

            RaiseOnMoveEvent(stepsLenth, direction);

            //GlobalFunctions.DebugLog("Going for the wood box: " + stepsLenth + "/" + direction);

            yield break;
        }

        if (IsHit)
        {
            stepsLenth = Random.Range(7, 15);

            RaiseOnMoveEvent(stepsLenth, direction);

            //GlobalFunctions.DebugLog("Taking damage, move by: " + stepsLenth + "/" + direction);

            yield break;
        }

        if (IsRaycastHit)
        {
            stepsLenth = Mathf.RoundToInt(RaycastHitDistance);

            direction = -1;

            RaiseOnMoveEvent(stepsLenth, direction);

            //GlobalFunctions.DebugLog("Can't shoot, move by: " + stepsLenth + "/" + direction);

            yield break;
        }

        RaiseOnMoveEvent(stepsLenth, direction);

        //GlobalFunctions.DebugLog("Don't move");
    }

    private void RaiseOnMoveEvent(int stepsLength, int direction)
    {
        onMove?.Invoke(stepsLength, direction);
    }
}
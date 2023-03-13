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

    public int AccurateShot { get; private set; }
    public int MissedShot { get; private set; }
    public int PlayerPreviousHealth { get; private set; }
    public int PlayerCurrentHealth { get; private set; }

    public float Distance { get; private set; }
    public float ShotDistance { get; private set; }
    public float PreviousShotDistance { get; private set; }
    public float RaycastHitDistance { get; private set; }
    public float WoodBoxDistance { get; private set; }

    public bool IsTakingDamage { get; private set; }
    public bool IsRayCollided { get; private set; }
    public bool IsEnemyTakingDamage { get; private set; }

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

    public HealthController EnemyHealthController { get; private set; }

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

        if(EnemyHealthController != null)
        {
            EnemyHealthController.OnUpdateHealthBar += OnEnemyHit;
            EnemyHealthController.onUpdateArmorBar += OnEnemyHit;
        }
    }

    private void FixedUpdate()
    {
        GetMissilePosition();
    }

    private void OnGameStart()
    {
        EnemyTankObj = GlobalFunctions.ObjectsOfType<TankController>.Find(tankController => tankController.gameObject.name != Names.Tank_SecondPlayer).gameObject;

        EnemyHealthController = Get<HealthController>.From(EnemyTankObj);

        EnemyHealthController.OnUpdateHealthBar += OnEnemyHit;
        EnemyHealthController.onUpdateArmorBar += OnEnemyHit;
    }

    private void OnTurnController(TurnState turnState)
    {
        GetTurnState(turnState);
    }

    private void OnShoot(GameObject gameObject)
    {
        MissileObj = gameObject;

        IsEnemyTakingDamage = false;
    }

    private void OnTakeDamage(BasePlayer basePlayer, int damage)
    {
        if (damage > 0)
            IsTakingDamage = true;
    }

    private void OnEnemyHit(int health)
    {
        PlayerPreviousHealth = PlayerCurrentHealth;
        PlayerCurrentHealth = health;

        CalculateShotsResult(PlayerCurrentHealth != PlayerPreviousHealth);
    }

    private void CalculateShotsResult(bool isEnemyTakingDamage)
    {
        IsEnemyTakingDamage = isEnemyTakingDamage;

        AccurateShot = IsEnemyTakingDamage ? AccurateShot + 1 : 0;

        MissedShot = IsEnemyTakingDamage ? 0 : MissedShot + 1;
    }

    private void OnRayCast(bool isRayCastHit, float distance)
    {
        IsRayCollided = isRayCastHit;

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
        IsTakingDamage = false;

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
    }

    private IEnumerator CalculateEnemyCurrentPosition()
    {
        yield return null;

        EnemyCurrentPosition = EnemyTankObj.transform.position;
    }

    private IEnumerator CalculateTanksDistance()
    {
        yield return null;

        Distance = Vector3.Distance(CurrentPosition, EnemyCurrentPosition);
    }

    private IEnumerator CalculateEnemyHitPositions()
    {
        yield return null;

        if (IsEnemyTakingDamage)
        {
            yield break;
        }

        if (MissilePosition == Vector3.zero)
        {
            CalculateShotsResult(false);

            yield break;
        }

        MissileLandingPosition = MissilePosition;

        PreviousShotDistance = ShotDistance;

        ShotDistance = (float)Converter.Double(EnemyPreviousPosition.x - MissileLandingPosition.x);

        yield return null;

        if (Mathf.Abs(ShotDistance) >= 1.5f)
            ShotDistance = 0;

        yield return null;

        if (PreviousShotDistance == ShotDistance)
        {
            float random = Mathf.Abs(PreviousShotDistance) > 0.2f ? Random.Range(-Mathf.Abs(PreviousShotDistance), Mathf.Abs(PreviousShotDistance)) : Random.Range(-0.2f, 0.2f);

            ShotDistance = random;
        }

        yield return null;

        CalculateShotsResult(false);
    }

    private IEnumerator CalculateShootControllerTargetFixingValue()
    {
        yield return null;

        _aiShootController.ControlTargetFixingValue(ShotDistance);
    }

    private IEnumerator CalculateCurrentPosition()
    {
        yield return null;

        CurrentPosition = transform.position;
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

            yield break;
        }

        if (IsTakingDamage)
        {
            stepsLenth = Random.Range(7, 15);

            RaiseOnMoveEvent(stepsLenth, direction);

            yield break;
        }

        if (IsRayCollided)
        {
            stepsLenth = Mathf.RoundToInt(RaycastHitDistance);

            direction = -1;

            RaiseOnMoveEvent(stepsLenth, direction);

            yield break;
        }

        RaiseOnMoveEvent(stepsLenth, direction);
    }

    private void RaiseOnMoveEvent(int stepsLength, int direction)
    {
        onMove?.Invoke(stepsLength, direction);
    }
}
using UnityEngine;

public class HUDComponentAmmoAndXP : MonoBehaviour
{
    [SerializeField] private TurnState _turnState;
    [SerializeField] private HUDScore _hudScore;
    [SerializeField] private CurrentWeaponStatus _currentWeaponStatus;
    private TurnController _turnController;
    private bool _isAssigned;


    private void Awake() => _turnController = FindObjectOfType<TurnController>();

    private void OnEnable() => _turnController.OnTurnChanged += OnTurnChanged;

    private void OnDisable() => _turnController.OnTurnChanged -= OnTurnChanged;

    private void OnTurnChanged(TurnState turnState)
    {
        if (!_isAssigned)
        {
            Initialize();
            _isAssigned = true;
        }
    }

    private void Initialize()
    {
        PlayerTurn playerTurn = GlobalFunctions.ObjectsOfType<PlayerTurn>.Find(turn => turn.MyTurn == _turnState);

        if (playerTurn != null)
        {
            if (Get<TankController>.From(playerTurn.gameObject).BasePlayer != null)
            {
                GlobalFunctions.CanvasGroupActivity(_hudScore.CanvasGroup, true);
                GlobalFunctions.CanvasGroupActivity(_currentWeaponStatus.CanvasGroup, true);       
            }
            else
            {
                _hudScore.gameObject.SetActive(false);
                _currentWeaponStatus.gameObject.SetActive(false);
            }
        }
    }
}

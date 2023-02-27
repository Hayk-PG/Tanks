using UnityEngine;

public class HUDComponentAmmoAndXP : MonoBehaviour
{
    [SerializeField] 
    private TurnState _turnState;

    [SerializeField] [Space]
    private HUDScore _hudScore;

    [SerializeField] [Space]
    private CurrentWeaponStatus _currentWeaponStatus;

    private bool _isAssigned;




    private void OnEnable() => GameSceneObjectsReferences.TurnController.OnTurnChanged += OnTurnChanged;

    private void OnDisable() => GameSceneObjectsReferences.TurnController.OnTurnChanged -= OnTurnChanged;

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

                GlobalFunctions.CanvasGroupActivity(_currentWeaponStatus.CanvasGroup, false);       
            }
            else
            {
                _hudScore.gameObject.SetActive(false);

                _currentWeaponStatus.gameObject.SetActive(false);
            }
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

public class TimerIcon : MonoBehaviour
{
    [SerializeField]
    private Image _img; 
    
    [SerializeField] [Space]
    private Sprite[] _sprtIcons;
    
    [SerializeField] [Space] 
    TurnState _corespondentIcon;

    [SerializeField] [Space]
    private TurnTimer _turnTimer;





    private void OnEnable() => _turnTimer.OnTurnTimer += OnTurnTimer;

    private void OnDisable() => _turnTimer.OnTurnTimer -= OnTurnTimer;

    private void OnTurnTimer(TurnState turnState, int seconds) => ChangeIcon(turnState == _corespondentIcon ? _sprtIcons[0] : _sprtIcons[1]);

    private void ChangeIcon(Sprite sprite) => _img.sprite = sprite;
}

using UnityEngine;
using UnityEngine.UI;

public class TimerIcon : MonoBehaviour
{
    [SerializeField] TurnState _corespondentIcon;
    [SerializeField] private Sprite[] _sprtIcons;
    private Image _img;    
    private TurnTimer _turnTimer;



    private void Awake()
    {
        _img = Get<Image>.From(gameObject);
        _turnTimer = FindObjectOfType<TurnTimer>();       
    }

    private void OnEnable() => _turnTimer.OnTurnTimer += OnTurnTimer;

    private void OnDisable() => _turnTimer.OnTurnTimer -= OnTurnTimer;

    private void OnTurnTimer(TurnState turnState, int seconds) => ChangeIcon(turnState == _corespondentIcon ? _sprtIcons[0] : _sprtIcons[1]);

    private void ChangeIcon(Sprite sprite) => _img.sprite = sprite;
}

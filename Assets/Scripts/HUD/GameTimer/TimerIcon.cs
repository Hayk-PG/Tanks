using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TimerIcon : MonoBehaviour
{
    [SerializeField] TurnState _corespondentIcon;
    [SerializeField] private Sprite[] _icons;
    private Image _image;    
    private CanvasGroup _canvasGroup;
    private TurnTimer _turnTimer;
    private bool _isTurnStarted;



    private void Awake()
    {
        _image = Get<Image>.From(gameObject);
        _canvasGroup = Get<CanvasGroup>.From(gameObject);
        _turnTimer = FindObjectOfType<TurnTimer>();       
    }

    private void OnEnable()
    {
        _turnTimer.OnTurnTimer += OnTurnTimer;
    }

    private void OnDisable()
    {
        _turnTimer.OnTurnTimer -= OnTurnTimer;
    }

    private void OnTurnTimer(TurnState turnState, int seconds)
    {
        if(turnState == _corespondentIcon)
        {           
            _canvasGroup.alpha = 1;

            if(!_isTurnStarted && _image.transform.localEulerAngles.z == Converter.AngleConverter(180))
            {
                _image.sprite = _icons[0];
                StartCoroutine(RotateIcon());
                _isTurnStarted = true;
            }

            if(seconds <= 15 && seconds > 1)
            {
                _image.sprite = _icons[1];
            }

            if(seconds <= 1)
            {
                _image.transform.localEulerAngles = new Vector3(0, 0, 180);
                _image.sprite = _icons[0];
            }
        }
        else
        {
            _canvasGroup.alpha = 0;
            _isTurnStarted = false;
            _image.transform.localEulerAngles = new Vector3(0, 0, 180);
        }
    }
    
    private IEnumerator RotateIcon()
    {
        float z = Converter.AngleConverter(_image.transform.localEulerAngles.z);

        while (Converter.AngleConverter(_image.transform.localEulerAngles.z) > 0)
        {
            z -= 1000 * Time.deltaTime;

            if (z <= 0)
                z = 0;

            _image.transform.localEulerAngles = new Vector3(0, 0, z);
            yield return null;
        }
    }
}

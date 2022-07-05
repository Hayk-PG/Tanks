using System;
using UnityEngine;
using UnityEngine.UI;

public class Tab_EndgameRematchButton : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    private Button _button;
    private Tab_EndgameTimer _tabEndgameTimer;

    [SerializeField] private Color _clickedColor;
    private bool IsClicked
    {
        get => !_button.interactable;
        set => _button.interactable = !value;
    }
    private Color ButtonTextColor
    {
        get => Get<Text>.FromChild(_button.gameObject).color;
        set => Get<Text>.FromChild(_button.gameObject).color = value;
    }
    private string ButtonText
    {
        get => Get<Text>.FromChild(_button.gameObject).text;
        set => Get<Text>.FromChild(_button.gameObject).text = value;
    }
    
    public Action OnRematch { get; set; }


    private void Awake()
    {
        _canvasGroup = Get<CanvasGroup>.From(gameObject);
        _button = Get<Button>.From(gameObject);
        _tabEndgameTimer = FindObjectOfType<Tab_EndgameTimer>();
    }

    private void OnEnable()
    {
        _tabEndgameTimer.OnTimerEnd += OnTimerEnd;
    }

    private void OnDisable()
    {
        _tabEndgameTimer.OnTimerEnd -= OnTimerEnd;
    }

    private void Update()
    {
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(delegate 
        {
            if (!IsClicked)
            {
                OnRematch?.Invoke();
                IsClicked = true;
                ButtonTextColor = _clickedColor;
            }
        });
    }

    private void OnTimerEnd(string buttonText)
    {
        ButtonText = buttonText;
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, true);
    }
}

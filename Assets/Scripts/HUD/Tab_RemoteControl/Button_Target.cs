using UnityEngine;
using UnityEngine.UI;

public class Button_Target : MonoBehaviour
{
    private Button _button;
    private Button_TargetAnchoredPosition _buttonAnchoredPosition;


    private void Awake()
    {
        _button = Get<Button>.From(gameObject);
        _buttonAnchoredPosition = Get<Button_TargetAnchoredPosition>.From(gameObject);
    }

    private void Update()
    {
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        _buttonAnchoredPosition.OnGiveCoordinates?.Invoke(_buttonAnchoredPosition.Coordinates);
    }
}

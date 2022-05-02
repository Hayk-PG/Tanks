using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectTankButton : MonoBehaviour
{
    [SerializeField] private int _index;
    [SerializeField] private Image _iconTank;
    private Button _button;
    private Data _data;


    private void Awake()
    {
        _button = Get<Button>.From(gameObject);
        _data = FindObjectOfType<Data>();            
    }

    private void Start()
    {
        InitializeButton();
        Conditions<bool>.Compare(_index == _data.SelectedTankIndex, SimulateButtonClick, null);
    }

    private bool IsIndexCorrect()
    {
        return _index < _data.AvailableTanks.Length;
    }

    private void InitializeButton()
    {
        if (IsIndexCorrect()) _iconTank.sprite = _data.AvailableTanks[_index]._iconTank;
    }

    private void SimulateButtonClick()
    {
        _button.OnPointerDown(new PointerEventData(EventSystem.current));
        _button.OnSelect(new PointerEventData(EventSystem.current));
        _button.OnPointerUp(new PointerEventData(EventSystem.current));

        print("Player selected tank button is auto highlighted at the game start/ " + _data.SelectedTankIndex);
    }

    public void OnClickTankButton()
    {
        if (IsIndexCorrect()) _data.SetData(new Data.NewData { SelectedTankIndex = _data.AvailableTanks[_index]._tankIndex });        
    }
}

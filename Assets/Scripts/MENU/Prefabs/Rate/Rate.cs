using UnityEngine;
using TMPro;

public class Rate : MonoBehaviour, IReset, IMatchmakeTextResult
{
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private RateButton _decrementButton;
    [SerializeField] private RateButton _incrementButton;

    [SerializeField] private Item _item;

    public int Number
    {
        get
        {
            return int.Parse(_inputField.text);
        }
        set
        {
            _inputField.text = value.ToString();
        }
    }


    private void OnEnable()
    {
        _decrementButton._onSendNumber += GetNumber;
        _incrementButton._onSendNumber += GetNumber;
    }

    private void OnDisable()
    {
        _decrementButton._onSendNumber -= GetNumber;
        _incrementButton._onSendNumber -= GetNumber;
    }

    private void GetNumber(int obj)
    {
        Number += obj;
        _inputField.text = Number.ToString();
    }

    public void SetDefault()
    {
        Number = 0;
    }

    public string TextResultOnline()
    {
        string number = Number <= 0 ? GlobalFunctions.RedColorText(Number.ToString()) : GlobalFunctions.BlueColorText(Number.ToString());
        return _item.Name + number + "\n";
    }

    public string TextResultOffline()
    {
        return "\n";
    }
}

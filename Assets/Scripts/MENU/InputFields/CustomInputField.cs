using UnityEngine;
using TMPro;

public class CustomInputField : MonoBehaviour, IReset
{
    [SerializeField] private TMP_InputField _inputField;

    public string Text
    {
        get
        {
            return _inputField.text;
        }
        set
        {
            _inputField.text = value;
        }
    }

    public void SetDefault()
    {
        Text = "";
    }
}

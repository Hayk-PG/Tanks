using UnityEngine;
using TMPro;

public class CustomInputField : MonoBehaviour
{
    [SerializeField] private TMP_InputField _inputField;

    public string Text
    {
        get => _inputField.text;
    }
}

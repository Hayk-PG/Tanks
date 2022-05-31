using UnityEngine;
using UnityEngine.UI;

public class ItemStatus : MonoBehaviour
{
    [SerializeField] private Text _textValue;
    public int Value
    {
        get => int.Parse(_textValue.text);
        set => _textValue.text = value.ToString();
    }


    private void Awake()
    {
        Value = 0;
    }
}

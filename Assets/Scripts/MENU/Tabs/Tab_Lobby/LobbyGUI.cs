using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LobbyGUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _txtTitle;
    [SerializeField] private Image _imgBackgroundEffect;

    [SerializeField] private string cntntTitle;
    [SerializeField] private Color _clrTitle;

    private Color TitleColor
    {
        get
        {
            return _txtTitle.color;
        }
        set
        {
            _txtTitle.color = value;
            _imgBackgroundEffect.color = value;
        }
    }
    public string LobbyName
    {
        get => cntntTitle;
    }


    private void Awake()
    {
        SetTitleColor(_clrTitle);
        SetText(_txtTitle.text, cntntTitle);
    }

    private void SetTitleColor(Color color) => TitleColor = color;

    private void SetText(string txt, string content) => _txtTitle.text = content;
}

using UnityEngine;
using TMPro;

public class EndGameStatsGroup : MonoBehaviour
{
    [SerializeField] [Space]
    private CanvasGroup _canvasGroup;

    [SerializeField] [Space]
    private TMP_Text _txtTitle, _txtValue;

    public CanvasGroup CanvasGroup => _canvasGroup;



    public void Initialize(string title, string value)
    {
        _txtTitle.text = title;

        _txtValue.text = value;
    }
}

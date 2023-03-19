using UnityEngine;
using TMPro;

public class EndGameItemsGroup : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup _canvasGroup;

    [SerializeField] [Space]
    private TMP_Text _txtValue;

    public CanvasGroup CanvasGroup => _canvasGroup;



    public void Initialize(string value)
    {
        _txtValue.text = value;
    }
}

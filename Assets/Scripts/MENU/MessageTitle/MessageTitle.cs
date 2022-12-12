using UnityEngine;
using UnityEngine.UI;

public class MessageTitle : Tab_Base
{
    [SerializeField] private Text _text;
    [SerializeField] private CanvasGroup _canvasGroup;


    public void Print(string title)
    {
        _text.text = title;
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, true);
    }
}

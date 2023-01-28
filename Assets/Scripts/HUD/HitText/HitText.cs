using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HitText : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Image _img;
    [SerializeField] private TMP_Text _txt;



    public void Display(Sprite sprt, string txt)
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, true);
        _img.sprite = sprt;
        _txt.text = txt;
    }

    public void OnAnimationEnd() => gameObject.SetActive(false);
}

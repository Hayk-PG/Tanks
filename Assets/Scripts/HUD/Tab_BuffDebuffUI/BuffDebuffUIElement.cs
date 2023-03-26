using UnityEngine;
using UnityEngine.UI;
using Coffee.UIEffects;

public class BuffDebuffUIElement : MonoBehaviour, IReset
{
    [SerializeField]
    private Image _imgBg, _imgIcon, _imgFill;

    [SerializeField] [Space]
    private UIShiny _uiShiny;

    private int _rounds;



    public void Set(Color color, Sprite icon, int rounds)
    {
        _imgBg.color = color;

        _imgIcon.sprite = icon;

        _rounds = rounds;
    }

    public void SetDefault()
    {
        
    }
}

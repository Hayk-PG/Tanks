using UnityEngine;
using UnityEngine.UI;
using Coffee.UIEffects;

public class BuffDebuffUIElement : MonoBehaviour, IReset
{
    [SerializeField] [Space]
    private Image _img, _imgIcon;

    [SerializeField] [Space]
    private UIShiny _uiShiny;




    public void Set()
    {
        
    }

    private void SetColor(Color color)
    {
        if (color == default)
            return;

        _img.color = color;
    }

    private void SetIcon(Sprite sprite)
    {
        if (sprite == null)
            return;

        _imgIcon.sprite = sprite;
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void SetDefault()
    {
        _uiShiny.Play();
    }
}

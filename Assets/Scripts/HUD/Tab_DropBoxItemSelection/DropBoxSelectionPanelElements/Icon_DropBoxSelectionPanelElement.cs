using UnityEngine;
using UnityEngine.UI;

public class Icon_DropBoxSelectionPanelElement : MonoBehaviour
{
    [SerializeField]
    private Image _imgIcon;

    public Sprite Icon
    {
        get => _imgIcon.sprite;
        set => _imgIcon.sprite = value;
    }
}

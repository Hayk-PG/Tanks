using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Btn_Tank : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private BtnTankLogic _btnTankLogic;
    [SerializeField] private Image _imgTank;
    [SerializeField] private Image _imgLock;
    [SerializeField] private TMP_Text _txtName;
    [SerializeField] private TMP_Text _txtLevel;
    [SerializeField] private Stars _stars;

    private int _relatedTankIndex;

    public int RelaedTankIndex { get => _relatedTankIndex; private set => _relatedTankIndex = value; }



    public void SetActivity(bool isActive)
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, isActive);
    }

    public void SetPicture(Sprite sprite)
    {
        _imgTank.sprite = sprite;
    }

    public void SetName(string name)
    {
        _txtName.text = name;
    }

    public void SetStars(int stars)
    {
        _stars.Display(stars);
    }

    public void SetRelatedTankIndex(int relatedtankIndex)
    {
        _relatedTankIndex = relatedtankIndex;
    }

    public void SetLevel(int level)
    {
        _txtLevel.text = level.ToString();
    }

    public void AutoSelect()
    {
        _btnTankLogic.Select(this);
    }
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Btn_Tank : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup _canvasGroup;

    [SerializeField] [Space]
    private Button _button;

    [SerializeField] [Space]
    private Image _imgTank, _imgLock;

    [SerializeField] [Space]
    private TMP_Text _txtName;

    [SerializeField] [Space]
    private TMP_Text _txtLevel;

    [SerializeField] [Space]
    private Stars _stars;

    private int _relatedTankIndex;

    public TankProperties TankProperties { get; private set; }

    public Button Button { get => _button; }

    public Image ImageTank { get => _imgTank; }

    public Color ColorBtn { get => _button.image.color; internal set => _button.image.color = value; }

    public bool IsLocked
    {
        get => _imgLock.gameObject.activeInHierarchy;
        set => _imgLock.gameObject.SetActive(value);
    }

    public event Action<int, int> _onAutoSelect;
    public event Action<int> _onClick;




    private void Start()
    {
        _button.onClick.AddListener(() => _onClick?.Invoke(_relatedTankIndex));
    }

    public void SetActivity(bool isActive)
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, isActive);
    }

    public void SetTankProprties(TankProperties tankProperties)
    {
        TankProperties = tankProperties;
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
        if (MyPhotonNetwork.IsOfflineMode)
        {
            _txtLevel.gameObject.SetActive(false);

            return;
        }

        _txtLevel.gameObject.SetActive(true);
        _txtLevel.text = Level(level);
    }

    public void SetLockState(bool isLocked)
    {
        IsLocked = isLocked;
    }

    public void AutoSelect(int relatedTankIndex, int horizontalGroupsLength)
    {
        if (_relatedTankIndex == relatedTankIndex)
            _onAutoSelect?.Invoke(relatedTankIndex, horizontalGroupsLength);
    }

    private string Level(int level)
    {
        return level == 1 ? "I" : level == 2 ? "II" : level == 3 ? "III" : level == 4 ? "IV" : level == 5 ? "V" : level == 6 ? "VI" :
               level == 7 ? "VII" : level == 8 ? "VIII" : level == 9 ? "IX" : level == 10 ? "X" : level == 11 ? "XI" :
               level == 12 ? "XII" : level == 13 ? "XIII" : level == 14 ? "XIV" : level == 15 ? "XV" : level == 16 ? "XVI" :
               level == 17 ? "XVII" : level == 18 ? "XVIII" : level == 19 ? "XIX" : level == 20 ? "XX" : level == 21 ? "XXI" :
               level == 22 ? "XXII" : level == 23 ? "XXIII" : level == 24 ? "XXIV" : level == 25 ? "XXV" : level == 26 ? "XXVI" :
               level == 27 ? "XXVII" : level == 28 ? "XXVIII" : level == 29 ? "XXIX" : level == 30 ? "XXX" : "";
    }
}

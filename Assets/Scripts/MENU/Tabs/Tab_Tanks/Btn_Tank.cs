using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.AddressableAssets;


//ADDRESSABLE
public class Btn_Tank : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup _canvasGroup;

    [SerializeField] [Space]
    private Btn _btn;

    [SerializeField] [Space]
    private Image _imgTank, _imgLock;

    [SerializeField] [Space]
    private BtnTxt _btnTxtTop, _btnTxtBottom;

    [SerializeField] [Space]
    private Stars _stars;

    private int _relatedTankIndex;

    public TankProperties TankProperties { get; private set; }

    public Image ImageTank { get => _imgTank; }

    public bool IsLocked
    {
        get
        {
            return _imgLock == null ? false : _imgLock.gameObject.activeInHierarchy;
        }
        set
        {
            if (_imgLock == null)
                return;

            _imgLock.gameObject.SetActive(value);
        }
    }

    public event Action<int, int> _onAutoSelect;
    public event Action<int> _onClick;





    private void OnEnable() => _btn.onSelect += () => _onClick?.Invoke(_relatedTankIndex);

    private void OnDisable() => _btn.onSelect -= () => _onClick?.Invoke(_relatedTankIndex);

    public void SetActivity(bool isActive) => GlobalFunctions.CanvasGroupActivity(_canvasGroup, isActive);

    public void SetTankProprties(TankProperties tankProperties) => TankProperties = tankProperties;

    public void SetPicture(AssetReferenceSprite assetReferenceSprite)
    {
        if (assetReferenceSprite.IsValid())
        {
            _imgTank.sprite = (Sprite)assetReferenceSprite.OperationHandle.Result;

            return;
        }

        if (_imgTank.sprite == null)
            assetReferenceSprite.LoadAssetAsync().Completed += asset => { _imgTank.sprite = asset.Result; };
    }

    public void SetName(string name) => _btnTxtTop.SetButtonTitle(name);

    public void SetStars(int stars) => _stars.Display(stars);

    public void SetRelatedTankIndex(int relatedtankIndex) => _relatedTankIndex = relatedtankIndex;

    public void SetLevel(int level)
    {
        _btnTxtBottom.SetButtonTitle(MyPhotonNetwork.IsOfflineMode ? $"Unlocked" : $"Lv. {level}");
    }

    public void SetLockState(bool isLocked) => IsLocked = isLocked;

    public void AutoSelect(int selectedTankIndex, int horizontalGroupsLength = 0)
    {
        if (_relatedTankIndex != selectedTankIndex)
            return;

        _btn.Select();

        _onAutoSelect?.Invoke(_relatedTankIndex, horizontalGroupsLength);
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

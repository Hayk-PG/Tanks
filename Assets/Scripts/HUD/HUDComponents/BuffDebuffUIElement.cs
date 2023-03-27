using UnityEngine;
using UnityEngine.UI;
using Coffee.UIEffects;

public class BuffDebuffUIElement : MonoBehaviour, IReset
{
    [SerializeField]
    private Image _imgBg, _imgIcon, _imgFill;

    [SerializeField] [Space]
    private UIShiny _uiShiny;

    [SerializeField] [Space]
    private Sprite[] _iconsXpMultiplier;

    private int _currentTurnCyclesCount;
    private int _activeDurationTime;

    public BuffDebuffType BuffDebuffType { get; private set; }




    public void Set(BuffDebuffType buffDebuffType, int activeDurationTime)
    {
        BuffDebuffType = buffDebuffType;

        SetIcon(buffDebuffType);

        ControlImageFill(0);

        GetCurrentTurnCyclesCount();

        GetActiveDurationTime(activeDurationTime);

        ManageTurnControllerSubscribtion(true);
    }

    private void SetIcon(BuffDebuffType buffDebuffType)
    {
        switch (buffDebuffType)
        {
            case BuffDebuffType.Xp2:
                _imgIcon.sprite = _iconsXpMultiplier[0];
                break;

            case BuffDebuffType.Xp3:
                _imgIcon.sprite = _iconsXpMultiplier[1];
                break;
        }
    }

    private void GetCurrentTurnCyclesCount() => _currentTurnCyclesCount = GameSceneObjectsReferences.TurnController.TurnCyclesCount;

    private void GetActiveDurationTime(int activeDurationTime) => _activeDurationTime = activeDurationTime;

    private void ControlImageFill(float fillAmount) => _imgFill.fillAmount = fillAmount;

    private float FillRate()
    {
        return Mathf.InverseLerp(_currentTurnCyclesCount, _activeDurationTime, GameSceneObjectsReferences.TurnController.TurnCyclesCount);
    }

    private void ManageTurnControllerSubscribtion(bool isSubscribing)
    {
        if (isSubscribing)
            GameSceneObjectsReferences.TurnController.OnTurnChanged += OnTurnController;
        else
            GameSceneObjectsReferences.TurnController.OnTurnChanged -= OnTurnController;
    }

    private void OnTurnController(TurnState turnState)
    {
        if(GameSceneObjectsReferences.TurnController.TurnCyclesCount > _activeDurationTime)
        {
            ManageTurnControllerSubscribtion(false);

            gameObject.SetActive(false);

            return;
        }

        ControlImageFill(FillRate());
    }

    public void SetDefault()
    {
        
    }
}

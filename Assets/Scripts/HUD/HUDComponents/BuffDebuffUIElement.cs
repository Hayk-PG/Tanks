using UnityEngine;
using UnityEngine.UI;
using Coffee.UIEffects;
using System.Collections;

public class BuffDebuffUIElement : MonoBehaviour, IReset
{
    [SerializeField] [Space]
    private CanvasGroup _canvasGroup;

    [SerializeField] [Space]
    private Image _imgBg, _imgIcon, _imgFill;

    [SerializeField] [Space]
    private UIShiny _uiShiny;

    [SerializeField] [Space]
    private Sprite[] _iconSprites;

    private int _currentTurnCyclesCount;
    private int _activeDurationTime;




    public void Set(BuffDebuffType buffDebuffType, IBuffDebuffUIElementController buffDebuffUIElement = null, object[] data = null)
    {
        // Set inactive in case Deactivate method was not called yet

        SetActive(false);

        StartCoroutine(ExecuteAfterDelay(buffDebuffType, buffDebuffUIElement, data));
    }

    private IEnumerator ExecuteAfterDelay(BuffDebuffType buffDebuffType, IBuffDebuffUIElementController buffDebuffUIElement = null, object[] data = null)
    {
        yield return new WaitForSeconds(1f);

        SetIcon(buffDebuffType);

        ControlImageFill(0);

        PlaySound(true);

        SetActive(true);

        if (buffDebuffUIElement != null)
        {
            buffDebuffUIElement.AssignBuffDebuffUIElement(this);

            yield break;
        }

        GetCurrentTurnCyclesCount();

        GetActiveDurationTime(data);

        ManageTurnControllerSubscribtion(true);
    }

    private void SetActive(bool isActive) => GlobalFunctions.CanvasGroupActivity(_canvasGroup, isActive);

    private void PlaySound(bool isActive)
    {
        UISoundController.PlaySound(9, isActive ? 0 : 1);
    }

    private void SetIcon(BuffDebuffType buffDebuffType)
    {
        switch (buffDebuffType)
        {
            case BuffDebuffType.Xp2:

                UpdateIconSprite(_iconSprites[0]);
                break;

            case BuffDebuffType.Xp3:

                UpdateIconSprite(_iconSprites[1]);
                break;

            case BuffDebuffType.Ability:

                UpdateIconSprite(_iconSprites[2]);
                break;
        }
    }

    private void UpdateIconSprite(Sprite sprite) => _imgIcon.sprite = sprite;

    private void GetCurrentTurnCyclesCount() => _currentTurnCyclesCount = GameSceneObjectsReferences.TurnController.TurnCyclesCount;

    private void GetActiveDurationTime(object[] data = null)
    {
        if (data == null)
            return;

        _activeDurationTime = (int)data[0];
    }

    public void ControlImageFill(float fillAmount) => _imgFill.fillAmount = fillAmount;

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
        if(GameSceneObjectsReferences.TurnController.TurnCyclesCount >= _activeDurationTime)
        {
            ManageTurnControllerSubscribtion(false);

            Deactivate();

            return;
        }

        ControlImageFill(FillRate());
    }

    public void Deactivate()
    {
        PlaySound(false);

        SetActive(false);

        gameObject.SetActive(false);
    }

    public void SetDefault()
    {
        
    }
}

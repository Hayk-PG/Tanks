using System;
using UnityEngine;

public abstract class BaseAbility : MonoBehaviour, IPlayerAbility, IBuffDebuffUIElementController
{
    protected enum AbilitiesOrders { First, Second }

    [SerializeField]
    protected AbilitiesOrders _abilityOrder;

    [SerializeField] [Space]
    protected PlayerTurn _playerTurn;

    protected IPlayerAbility _iPlayerAbility;

    [SerializeField] [Space]
    protected int _price, _quantity, _usageFrequency, _turns;

    protected virtual bool IsAbilityActive { get; set; } = false;
    
    protected virtual int Price => _price;
    protected virtual int Quantity => _quantity;
    protected virtual int UsageFrequency => _usageFrequency;
    protected virtual int Turns => _turns;
    protected virtual int UsedTime { get; set; } = 0;

    protected abstract string Title { get; }
    protected abstract string Ability { get; }

    public BuffDebuffUIElement BuffDebuffUIElement { get; set; }


    public event Action<object[]> onAbilityActive;





    protected virtual void Awake() => _iPlayerAbility = this;

    protected virtual void Start()
    {
        GameSceneObjectsReferences.DropBoxSelectionPanelPlayerAbilities[(int)_abilityOrder].Initialize(_iPlayerAbility, Title, Ability, Price, Quantity, UsageFrequency, Turns);
    }

    protected virtual void OnEnable()
    {
        DropBoxSelectionHandler.onItemSelect += OnAbilitySelect; ;

        GameSceneObjectsReferences.TurnController.OnTurnChanged += OnTurnChanged;
    }

    protected virtual void OnAbilitySelect(DropBoxItemType dropBoxItemType, object[] data)
    {
        if (dropBoxItemType == DropBoxItemType.Ability && (IPlayerAbility)data[0] == _iPlayerAbility)
            OnAbilityActivated();
    }

    protected virtual void OnAbilityActivated(object[] data = null)
    {
        IsAbilityActive = true;

        UsedTime = 0;

        ActivateBuffDebuffIcon(data);

        DisplayPlayerFeedbackText();
    }

    protected virtual void ActivateBuffDebuffIcon(object[] data = null) => BuffDebuffHandler.RaiseEvent(BuffDebuffType.Ability, _playerTurn.MyTurn, this, data);

    protected virtual void PlaySoundFX(int clipIndex) => SecondarySoundController.PlaySound(10, clipIndex);

    protected virtual void DisplayPlayerFeedbackText()
    {
        GameSceneObjectsReferences.PlayerFeedback.DisplayDropBoxItemText(gameObject.name, $"{Title} is activated!");
    }

    protected virtual void DeactivateAbilityAfterLimit()
    {
        CountAbilityUsage();

        UpdateAbilityBuffIcon();

        if (UsedTime >= Turns)
        {
            OnAbilityDeactivated();

            return;
        }
    }

    protected virtual void CountAbilityUsage()
    {
        UsedTime++;

        print($"{name}: {UsedTime}/{Turns}");
    }

    protected virtual void UpdateAbilityBuffIcon()
    {
        if (BuffDebuffUIElement == null)
            return;

        BuffDebuffUIElement.ControlImageFill(Mathf.InverseLerp(0, Turns, UsedTime));
    }

    protected virtual void OnAbilityDeactivated()
    {
        DeactivateBuffDebuffIcon();

        IsAbilityActive = false;
    }

    protected virtual void DeactivateBuffDebuffIcon()
    {
        if (BuffDebuffUIElement == null)
            return;

        BuffDebuffUIElement.Deactivate();
    }

    protected virtual void OnTurnChanged(TurnState turnState)
    {

    }

    protected virtual void RaiseAbilityEvent(object[] data = null) => onAbilityActive?.Invoke(data);

    public abstract void AssignBuffDebuffUIElement(BuffDebuffUIElement buffDebuffUIElement);
}

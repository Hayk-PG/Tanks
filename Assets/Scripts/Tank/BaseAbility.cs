using System;
using UnityEngine;

public abstract class BaseAbility : MonoBehaviour, IPlayerAbility
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

    public event Action<object[]> onAbilityActive;





    protected virtual void Awake() => _iPlayerAbility = this;

    protected virtual void Start() => GameSceneObjectsReferences.DropBoxSelectionPanelPlayerAbilities[(int)_abilityOrder].Initialize(_iPlayerAbility, Title, Ability, Price, Quantity, UsageFrequency, Turns);

    protected virtual void OnEnable()
    {
        DropBoxSelectionHandler.onItemSelect += OnAbilitySelect; ;

        GameSceneObjectsReferences.TurnController.OnTurnChanged += OnTurnChanged;
    }

    protected abstract void OnAbilitySelect(DropBoxItemType dropBoxItemType, object[] data);

    protected abstract void UseAbility(object[] data = null);

    protected virtual void DeactivateAbilityAfterLimit()
    {
        UsedTime++;

        print($"{name}: {UsedTime}/{UsageFrequency}");

        if (UsedTime >= UsageFrequency)
            IsAbilityActive = false;
    }

    protected abstract void OnTurnChanged(TurnState turnState);

    protected virtual void RaiseAbilityEvent(object[] data = null) => onAbilityActive?.Invoke(data);
}

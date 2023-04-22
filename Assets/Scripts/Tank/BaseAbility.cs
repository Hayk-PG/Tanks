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

    protected virtual bool IsAbilityActive { get; set; } = false;

    protected virtual int UsedTime { get; set; } = 0;

    protected abstract int Price { get; set; }
    protected abstract int Quantity { get; set; }
    protected abstract int UsageFrequency { get; set; }
    protected abstract int Turns { get; set; }

    protected abstract string Title { get; set; }
    protected abstract string Ability { get; set; }

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

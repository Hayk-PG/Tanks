using UnityEngine;


public class AbilityCloak : BaseAbility
{
    [SerializeField] [Space]
    private BaseShootController _baseShootController;

    private int[] _defaultLayers;

    private object[] _active = new object[] { true };
    private object[] _inactive = new object[] { false };

    private Transform[] _transforms;

    protected override string Title => "Cloak";
    protected override string Ability => $"Become invisible for {Turns} turn.";





    protected override void OnAbilityActivated(object[] data = null)
    {
        base.OnAbilityActivated(data);

        SetInvisible(true);

        RaiseAbilityEvent(_active);
    }

    protected override void OnTurnChanged(TurnState turnState)
    {
        if (IsAbilityActive && _playerTurn.IsMyTurn)
            DeactivateAbilityAfterLimit();
    }

    protected override void OnAbilityDeactivated()
    {
        base.OnAbilityDeactivated();

        SetInvisible(false);

        RaiseAbilityEvent(_inactive);
    }

    private void SetInvisible(bool isInvisible)
    {
        if (_transforms == null)
        {
            GetTransforms();

            CacheDefaultLayers();
        }

        if (_transforms == null && _defaultLayers == null)
            return;

        ChangeLayers(isInvisible);

        SetLayersDefault(isInvisible);
    }

    private  void GetTransforms() => _transforms = GetComponentsInChildren<Transform>(true);

    private void CacheDefaultLayers()
    {
        _defaultLayers = new int[_transforms.Length + 1];

        for (int i = -1; i < _transforms.Length; i++)
        {
            int index = i + 1;

            if(i < 0)
            {
                _defaultLayers[index] = transform.gameObject.layer;

                continue;
            }

            _defaultLayers[index] = _transforms[i].gameObject.layer;
        }
    }

    private void ChangeLayers(bool isInvisible)
    {
        if (!isInvisible)
            return;

        for (int i = -1; i < _transforms.Length; i++)
        {
            if(i < 0)
            {
                transform.gameObject.layer = 12;

                continue;
            }

            _transforms[i].gameObject.layer = 12;
        }
    }

    private void SetLayersDefault(bool isInvisible)
    {
        if (isInvisible)
            return;

        for (int i = -1; i < _transforms.Length; i++)
        {
            int index = i + 1;

            if(i < 0)
            {
                transform.gameObject.layer = _defaultLayers[index];

                continue;
            }

            _transforms[i].gameObject.layer = _defaultLayers[index];
        }
    }
}

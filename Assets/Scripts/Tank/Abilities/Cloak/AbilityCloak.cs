using UnityEngine;
using System.Collections;


public class AbilityCloak : BaseAbility
{
    [SerializeField] [Space]
    private BaseShootController _baseShootController;

    private Transform[] _transforms;

    private ParticleSystem _particle; 

    private int[] _defaultLayers;

    protected override string Title => "Cloak";
    protected override string Ability => $"Become invisible for {Turns} turn.";






    public void AssignAbilityParticle(ParticleSystem particle) => _particle = particle;

    protected override void OnAbilityActivated(object[] data = null)
    {
        base.OnAbilityActivated(data);

        SetInvisible(true);

        RaiseAbilityEvent();
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

        RaiseAbilityEvent(false);
    }

    private void SetInvisible(bool isInvisible)
    {
        if (_transforms == null)
        {
            GetAllChildTransforms();

            CacheDefaultLayersRecursively();
        }

        if (_transforms == null && _defaultLayers == null)
            return;

        StartCoroutine(PlayParticleAndHide(isInvisible));

        SetLayersDefault(isInvisible);

        PlaySoundFX(0);
    }

    private  void GetAllChildTransforms() => _transforms = GetComponentsInChildren<Transform>(true);

    private void CacheDefaultLayersRecursively()
    {
        _defaultLayers = new int[_transforms.Length + 1];

        for (int i = -1; i < _transforms.Length; i++)
        {
            int index = i + 1;

            if (IsIterationEmpty(i))
            {
                _defaultLayers[index] = transform.gameObject.layer;

                continue;
            }

            _defaultLayers[index] = _transforms[i].gameObject.layer;
        }
    }

    private IEnumerator PlayParticleAndHide(bool isInvisible)
    {
        if (_particle == null)
        {
            ChangeLayers(isInvisible);

            yield break;
        }

        yield return null;

        PlayParticles();

        ChangeLayers(isInvisible);
    }

    private void ChangeLayers(bool isInvisible)
    {
        if (!isInvisible)
            return;

        for (int i = -1; i < _transforms.Length; i++)
        {
            if (IsIterationEmpty(i))
            {
                transform.gameObject.layer = 12;

                continue;
            }

            if (IsParticlesTransform(_transforms[i]))
                continue;

            _transforms[i].gameObject.layer = 12;
        }
    }

    private void SetLayersDefault(bool isInvisible)
    {
        if (isInvisible)
            return;

        PlayParticles();

        for (int i = -1; i < _transforms.Length; i++)
        {
            int index = i + 1;

            if (IsIterationEmpty(i))
            {
                transform.gameObject.layer = _defaultLayers[index];

                continue;
            }

            if (IsParticlesTransform(_transforms[i]))
                continue;

            _transforms[i].gameObject.layer = _defaultLayers[index];
        }
    }

    private bool IsIterationEmpty(int i)
    {
        return i < 0;
    }

    private bool IsParticlesTransform(Transform transform)
    {
        return transform == _particle?.transform || transform.IsChildOf(_particle?.transform);
    }

    private void PlayParticles() => _particle.Play(true);

    public override void AssignBuffDebuffUIElement(BuffDebuffUIElement buffDebuffUIElement) => BuffDebuffUIElement = buffDebuffUIElement;
}

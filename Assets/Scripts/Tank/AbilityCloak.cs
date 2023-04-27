using UnityEngine;
using UnityEngine.AddressableAssets;
using System;
using System.Collections;


//ADDRESSABLE
public class AbilityCloak : BaseAbility
{
    [Serializable]
    private class AssetReferenceData
    {
        public AssetReference _assetReferenceParticles;
        public Vector3 localPosition;
        public Vector3 localScale;
    }

    [SerializeField] [Space]
    private BaseShootController _baseShootController;

    [SerializeField] [Space]
    private AssetReferenceData _assetReferenceData; // Note: When assigning this asset in the inspector, make sure to also set its local position and local scale accordingly.

    private Transform[] _transforms;

    private ParticleSystem _particles; // Cached on Addressable instantiate

    private int[] _defaultLayers;

    private object[] _active = new object[] { true };
    private object[] _inactive = new object[] { false };

    protected override string Title => "Cloak";
    protected override string Ability => $"Become invisible for {Turns} turn.";





    protected override void Start()
    {
        base.Start();

        InstantiateParticlesAsync();
    }

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

    private void InstantiateParticlesAsync()
    {
        if (_assetReferenceData == null)
            return;

        _assetReferenceData._assetReferenceParticles.InstantiateAsync(transform).Completed += asset =>
        {
            _particles = Get<ParticleSystem>.From(asset.Result);
            _particles.transform.localPosition = _assetReferenceData.localPosition;
            _particles.transform.localScale = _assetReferenceData.localScale;
        };
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
        if (_particles == null)
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
        return transform == _particles?.transform || transform.IsChildOf(_particles?.transform);
    }

    private void PlayParticles() => _particles.Play(true);

    public override void AssignBuffDebuffUIElement(BuffDebuffUIElement buffDebuffUIElement) => BuffDebuffUIElement = buffDebuffUIElement;
}

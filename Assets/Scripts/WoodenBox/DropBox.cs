using UnityEngine;

public class DropBox : MonoBehaviour
{
    [SerializeField] [Space]
    private ParticleSystem _particleFlash, _particleSmokePoof, _particleLightGlow;

    private WoodBoxDrop _woodBoxDrop;
    private WoodBox _woodBox;



    private void Awake()
    {
        _woodBoxDrop = Get<WoodBoxDrop>.From(transform.parent.gameObject);

        _woodBox = Get<WoodBox>.From(transform.parent.gameObject);
    }

    private void OnEnable()
    {
        SubscribeToWoodBoxDropEvents();

        SubscribeToWoodBoxEvents();
    }

    private void SubscribeToWoodBoxDropEvents()
    {
        if (_woodBoxDrop == null)
            return;

        _woodBoxDrop.onCollidedWithGround += OnCollidedWithGround;
    }

    private void SubscribeToWoodBoxEvents()
    {
        if (_woodBox == null)
            return;

        _woodBox.onPickDropBox += OnPickDropBox;
    }

    private void OnCollidedWithGround()
    {
        PlaySmokePoofParticle();

        PlayLightGlowParticle();
    }

    private void OnPickDropBox() => PlayFlashParticle();

    private void PlaySmokePoofParticle()
    {
        if (_particleSmokePoof.isPlaying)
            return;

        _particleSmokePoof.Play(true);
        _particleSmokePoof.transform.SetParent(null);
    }

    private void PlayLightGlowParticle()
    {
        if (_particleLightGlow.isPlaying)
            return;

        _particleLightGlow.Play(true);
    }

    private void PlayFlashParticle()
    {
        if (_particleFlash.isPlaying)
            return;

        _particleFlash.Play(true);
        _particleFlash.transform.SetParent(null);
    }
}

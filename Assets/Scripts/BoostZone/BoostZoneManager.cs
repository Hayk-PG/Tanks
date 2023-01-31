using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class BoostZoneManager : MonoBehaviour
{
    public enum Feature { None, XpBoost, SafeZone }
    public Feature _feature;
    [Space]

    [SerializeField] private Canvas _canvas;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Image _imgIcon;
    [SerializeField] private TMP_Text _txt;
    [Space]

    [SerializeField] private Sprite[] _sprites;
    [Space]

    [SerializeField] private ParticleSystem _particles;
    [Space]

    private TankController _tankController;

    private bool _isParticlesPlayed;

    public BaseBoostZoneFeatures BaseBoostZoneFeatures { get; private set; }

    public bool IsTriggered
    {
        get => _canvasGroup.alpha == 0;
        set => _canvasGroup.alpha = (value ? 0 : 1);
    }




    private void Awake()
    {
        _canvas.worldCamera = Camera.main;
    }

    private void OnTriggerEnter(Collider other)
    {
        bool isTankControllerNull = _tankController == null;
        bool isOtherGameobjectTankController = Get<TankController>.From(other.gameObject) != null;

        if (isTankControllerNull && isOtherGameobjectTankController)
        {
            _tankController = Get<TankController>.From(other.gameObject);
            BaseBoostZoneFeatures?.Use(this, _tankController);
        } 
    }

    private void OnTriggerExit(Collider other)
    {
        bool isTankControllerCached = _tankController != null;
        bool isOtherGameobjectTankController = Get<TankController>.From(other.gameObject) != null;

        if (isTankControllerCached && isOtherGameobjectTankController)
        {
            BaseBoostZoneFeatures?.Release(this, _tankController);
            _tankController = null;
        }
    }

    public void Init(Feature feature)
    {
        switch (feature)
        {
            case Feature.SafeZone:

                BaseBoostZoneFeatures = new BoostZoneSafe();
                _imgIcon.sprite = Sprite(Feature.SafeZone);

                break;

            case Feature.XpBoost:

                BaseBoostZoneFeatures = new BoostZoneXp();
                _imgIcon.sprite = Sprite(Feature.XpBoost);

                break;
        }
    }

    public void Trigger(bool isTriggered)
    {
        IsTriggered = isTriggered;

        if (!_isParticlesPlayed && IsTriggered)
        {
            PlayParticles();
            StartCoroutine(MakeParticlesReplayable());
            SecondarySoundController.PlaySound(6, 0);
            _isParticlesPlayed = true;
        }
    }

    private void PlayParticles() => _particles.Play();

    private IEnumerator MakeParticlesReplayable()
    {
        yield return new WaitForSeconds(3);
        _isParticlesPlayed = false;
    }

    private Sprite Sprite(Feature feature)
    {
        _imgIcon.gameObject.SetActive(true);
        _txt.gameObject.SetActive(false);

        return feature == Feature.SafeZone ? _sprites[0] : feature == Feature.XpBoost ? _sprites[1] : null;
    }

    private string Text(Feature feature)
    {
        _txt.gameObject.SetActive(true);
        _imgIcon.gameObject.SetActive(false);        

        return feature == Feature.XpBoost ? "x2 XP" : "";
    }
}

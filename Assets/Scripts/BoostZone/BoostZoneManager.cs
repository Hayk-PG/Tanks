using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BoostZoneManager : MonoBehaviour
{
    public enum Feature { None, XpBoost, SafeZone }
    public Feature _feature;
    [Space]

    [SerializeField] private Canvas _canvas;
    [SerializeField] private Image _imgIcon;
    [SerializeField] private TMP_Text _txt;
    [Space]

    [SerializeField] private Sprite[] _sprites;

    private BaseBoostZoneFeatures _boostZoneFeature;

    private GameObject _player;



    private void Awake()
    {
        _canvas.worldCamera = Camera.main;
    }

    private void OnTriggerEnter(Collider other)
    {
        _player = Get<TankController>.From(other.gameObject)?.gameObject;
        _boostZoneFeature?.Use(_player);
    }

    private void OnTriggerExit(Collider other)
    {
        if (_player == other.gameObject)
            _boostZoneFeature?.Release(other.gameObject);
    }

    public void Init(Feature feature)
    {
        switch (feature)
        {
            case Feature.SafeZone:
                _boostZoneFeature = new BoostZoneSafe();
                _imgIcon.sprite = Sprite(Feature.SafeZone);
                break;

            case Feature.XpBoost:
                _boostZoneFeature = new BoostZoneXp();
                _imgIcon.sprite = Sprite(Feature.XpBoost);
                break;
        }
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

using System;
using System.Linq;
using UnityEngine;

public class ScoreController : MonoBehaviour, IScore
{
    [SerializeField]
    private int _score;

    public PlayerTurn PlayerTurn { get; set; }
    public IDamage IDamage { get; set; }

    public int Score
    {
        get => _score;
        set => _score = value;
    }
    public Action<int, Vector3> OnDisplayTempPoints { get; set; }
    public Action<int> OnTempPointsWithoutDisplaying { get; set; }

    private AmmoTabCustomization _ammoTabCustomization;


    private void Awake()
    {       
        IDamage = Get<IDamage>.From(gameObject);
        PlayerTurn = Get<PlayerTurn>.From(gameObject);

        _ammoTabCustomization = FindObjectOfType<AmmoTabCustomization>();

        Score = 0;    
    }

    private void OnEnable()
    {
        _ammoTabCustomization.OnPlayerWeaponChanged += OnPlayerWeaponChanged;
    }

    private void OnDisable()
    {
        _ammoTabCustomization.OnPlayerWeaponChanged -= OnPlayerWeaponChanged;
    }

    private void OnPlayerWeaponChanged(AmmoTypeButton ammoTypeButton)
    {
        if (!ammoTypeButton._properties.IsUnlocked)
        {
            Score -= ammoTypeButton._properties.UnlockPoints;
            OnTempPointsWithoutDisplaying?.Invoke(-ammoTypeButton._properties.UnlockPoints);
        }
    }


    public void GetScore(int score, IDamage iDamage, Vector3 position)
    {
        Conditions<bool>.Compare(iDamage != IDamage || iDamage == null, () => UpdateScore(score, position), null);
    }

    private void UpdateScore(int score, Vector3 position)
    {
        Score += score;
        OnDisplayTempPoints?.Invoke(score, position);
    }
}

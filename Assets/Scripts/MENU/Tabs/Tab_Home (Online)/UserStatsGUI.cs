using UnityEngine;
using TMPro;

public class UserStatsGUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _txtLevel, _txtPoints, _txtWins, _txtLoses, _txtKills, _txtKdRatio, _txtTimePlayed, _txtQuits;

    private Tab_HomeOnline _tabHomeOnline;

    private bool _isStatsCached;


    private void Awake()
    {
        _tabHomeOnline = Get<Tab_HomeOnline>.From(gameObject);
    }

    private void OnEnable()
    {
        _tabHomeOnline.onTabOpen += CacheStats;
    }

    private void OnDisable()
    {
        _tabHomeOnline.onTabOpen -= CacheStats;
    }

    private void CacheStats()
    {
        if (!_isStatsCached)
        {
            User.GetStats(Data.Manager.PlayfabId, result =>
            {
                if (result.ContainsKey(Keys.Level))
                    _txtLevel.text = result[Keys.Level].ToString();

                if (result.ContainsKey(Keys.Points))
                    _txtPoints.text = result[Keys.Points].ToString();

                if (result.ContainsKey(Keys.Wins))
                    _txtWins.text = result[Keys.Wins].ToString();

                if (result.ContainsKey(Keys.Losses))
                    _txtLoses.text = result[Keys.Losses].ToString();

                if (result.ContainsKey(Keys.Kills))
                    _txtKills.text = result[Keys.Kills].ToString();

                if (result.ContainsKey(Keys.KD))
                    _txtKdRatio.text = result[Keys.KD].ToString();

                if (result.ContainsKey(Keys.TimePlayed))
                    _txtTimePlayed.text = result[Keys.TimePlayed].ToString();

                if (result.ContainsKey(Keys.Quits))
                    _txtQuits.text = result[Keys.Quits].ToString();

                _isStatsCached = true;
            });
        }
    }
}

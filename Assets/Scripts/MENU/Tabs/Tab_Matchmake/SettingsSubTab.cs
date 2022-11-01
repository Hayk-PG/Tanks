using UnityEngine;

public class SettingsSubTab : MatchmakeBaseSubTab
{
    [SerializeField] private Rounds _rounds;
    [SerializeField] private GameDifficultyLevel _gameDifficultyLevel;

    public override void SetDefault()
    {
        Conditions<bool>.Compare(MyPhotonNetwork.IsOfflineMode, ElementsInOfflineMode, ElementsInOnlineMode);
    }

    private void ElementsInOfflineMode()
    {
        if (_rounds.gameObject.activeInHierarchy)
        {
            _rounds.gameObject.SetActive(false);
            _gameDifficultyLevel.gameObject.SetActive(true);
        }
    }

    private void ElementsInOnlineMode()
    {
        if (_gameDifficultyLevel.gameObject.activeInHierarchy)
        {
            _gameDifficultyLevel.gameObject.SetActive(false);
            _rounds.gameObject.SetActive(true);
        }
    }
}

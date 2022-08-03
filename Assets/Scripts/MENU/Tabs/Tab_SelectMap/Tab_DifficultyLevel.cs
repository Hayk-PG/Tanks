using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tab_DifficultyLevel : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_Text _textDifficultyLevel;
    private CanvasGroup _canvasGroup;
    private Tab_SelectMaps _tabSelectMaps;


    private void Awake()
    {
        _canvasGroup = Get<CanvasGroup>.From(gameObject);
        _tabSelectMaps = FindObjectOfType<Tab_SelectMaps>();
    }

    private void OnEnable()
    {
        _tabSelectMaps.OnTabOpened += OnTabOpened;
    }

    private void OnDisable()
    {
        _tabSelectMaps.OnTabOpened -= OnTabOpened;
    }

    private void UpdateDifficultyLevelText(SingleGameDifficultyLevel singleGameDifficultyLevel)
    {
        switch (singleGameDifficultyLevel)
        {
            case SingleGameDifficultyLevel.Easy: _textDifficultyLevel.text = "Easy"; break;
            case SingleGameDifficultyLevel.Normal: _textDifficultyLevel.text = "Normal"; break;
            case SingleGameDifficultyLevel.Hard: _textDifficultyLevel.text = "Hard"; break;
        }
    }

    private void OnTabOpened()
    {
        GlobalFunctions.CanvasGroupActivity(_canvasGroup, MyPhotonNetwork.IsOfflineMode);
        _slider.value = PlayerPrefs.GetInt(Keys.DifficultyLevel, 0);
        Data.Manager.SetDifficultyLevel(Mathf.FloorToInt(_slider.value));
        UpdateDifficultyLevelText(Data.Manager.SingleGameDifficultyLevel);
    }

    public void OnSliderValueChanged()
    {
        Data.Manager.SetDifficultyLevel(Mathf.FloorToInt(_slider.value));
        UpdateDifficultyLevelText(Data.Manager.SingleGameDifficultyLevel);
    }
}

using UnityEngine;
using UnityEngine.UI;

public class UserInfo : MonoBehaviour
{
    [SerializeField] private InputField _inputFieldPlayerName;
    [SerializeField] private Text _textPlayerLevel;
    [SerializeField] private Text _sliderTextPoints;
    [SerializeField] private Slider _sliderPoints;
    [SerializeField] private Badge _badge;


    private void Start()
    {
        UpdateUserInfo();
    }

    private void UpdateUserInfo()
    {
        _inputFieldPlayerName.text = Data.Manager.Id;
        _textPlayerLevel.text = Data.Manager.Level.ToString();
        _sliderTextPoints.text = Data.Manager.Points.ToString() + "/" + Data.Manager.PointsSliderMinAndMaxValues[Data.Manager.Level, 1];
        _sliderPoints.minValue = Data.Manager.PointsSliderMinAndMaxValues[Data.Manager.Level, 0];
        _sliderPoints.maxValue = Data.Manager.PointsSliderMinAndMaxValues[Data.Manager.Level, 1];
        _sliderPoints.value = Data.Manager.Points;
        _badge.Set(Data.Manager.Level + 1 > Data.Manager.MaxLevel ? Data.Manager.Level : Data.Manager.Level + 1);
    }
}

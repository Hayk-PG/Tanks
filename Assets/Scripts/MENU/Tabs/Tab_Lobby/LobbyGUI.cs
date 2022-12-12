using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LobbyGUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _txtTitle;
    [SerializeField] private TMP_Text _txtRoundDuration;
    [SerializeField] private Image _imgBackgroundEffect;
    [SerializeField] private Image _imgBtn;
    [SerializeField] private Image _imgWind;
    [SerializeField] private Image _imbWindBlock;


    public void PrintLobbyName(string lobbyName)
    {
        _txtTitle.text = lobbyName;
    }
    public void PrintRoundDuration(int roundDuration)
    {
        _txtRoundDuration.text = roundDuration.ToString();
    }

    public void SetColor(Color color)
    {
        _txtTitle.color = color;
        _imgBackgroundEffect.color = color;
        _imgBtn.color = color;
    }

    public void SetWindBlockActivity(bool isWindOn)
    {
        _imgWind.gameObject.SetActive(isWindOn);
        _imbWindBlock.gameObject.SetActive(!isWindOn);
    }
}

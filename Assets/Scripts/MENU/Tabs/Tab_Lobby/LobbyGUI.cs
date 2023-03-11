using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LobbyGUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _txtTitle, _txtRoundDuration;

    [SerializeField] [Space]
    private Image _imgBackgroundEffect, _imgBtn, _imgWind, _imbWindBlock;



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

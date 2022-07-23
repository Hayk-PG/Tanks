using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tab_GameStartPlayerInfo : MonoBehaviour
{
    [SerializeField] private Badge _badge;
    [SerializeField] private Image _imageTank;
    [SerializeField] private TMP_Text _textName;
    [SerializeField] private CanvasGroup _badgeCanvasGroup;


    public void InitializePlayerInfoTab(int? playerRank, Sprite tank, string name)
    {
        if(playerRank != null)
        {
            _badge.Set(playerRank.Value);
            GlobalFunctions.CanvasGroupActivity(_badgeCanvasGroup, true);
        }
        _imageTank.sprite = tank;
        _textName.text = name;
    }
}

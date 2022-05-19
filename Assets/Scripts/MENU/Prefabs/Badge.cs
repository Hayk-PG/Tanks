using UnityEngine;
using UnityEngine.UI;

public class Badge : MonoBehaviour
{
    [SerializeField] ScriptableObject_BadgesSprites _scriptableObjectBadges;
    [SerializeField] private Image _imageBadge;
    [SerializeField] private Text _textRank;


    public void Set(int rank)
    {
        if (_scriptableObjectBadges != null && _imageBadge != null && _textRank != null)
        {
            _imageBadge.sprite = _scriptableObjectBadges.BadgeSprite(rank);
            _textRank.text = rank.ToString();
        }
    }
}

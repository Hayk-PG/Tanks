using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable objects/Data/Badges")]
public class ScriptableObject_BadgesSprites : ScriptableObject
{
    [SerializeField] private Sprite[] _badges;

    public Sprite BadgeSprite(int rank)
    {
        return rank < _badges.Length ? _badges[rank] : _badges[_badges.Length - 1];
    }
}
